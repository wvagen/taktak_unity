﻿#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.Editor;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.Editor.NativePlugins;
using VoxelBusters.CoreLibrary.Editor.NativePlugins.Build;
using VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Xcode;
using VoxelBusters.EssentialKit;
using UnityEditor.Build.Reporting;
using UnityEditor.Build;
using VoxelBusters.EssentialKit.AddressBookCore.iOS;
using VoxelBusters.EssentialKit.BillingServicesCore.iOS;
using VoxelBusters.EssentialKit.CloudServicesCore.iOS;
using VoxelBusters.EssentialKit.WebViewCore.iOS;
using VoxelBusters.EssentialKit.SharingServicesCore.iOS;
using VoxelBusters.EssentialKit.NotificationServicesCore.iOS;
using VoxelBusters.EssentialKit.NativeUICore.iOS;
using VoxelBusters.EssentialKit.GameServicesCore.iOS;
using VoxelBusters.EssentialKit.MediaServicesCore.iOS;
using VoxelBusters.EssentialKit.NetworkServicesCore.iOS;
using VoxelBusters.EssentialKit.DeepLinkServicesCore.iOS;
using VoxelBusters.EssentialKit.ExtrasCore.iOS;

namespace VoxelBusters.EssentialKit.Editor.Build.Xcode
{
    public class PBXNativePluginsProcessor : CoreLibrary.Editor.NativePlugins.Build.Xcode.PBXNativePluginsProcessor
    {
#region Properties

        private EssentialKitSettings Settings { get; set; }

#endregion

#region Base class methods

        public override void OnUpdateExporterObjects()
        {
            // Check whether plugin is configured
            if (!EnsureInitialised()) return;

            DebugLogger.Log(EssentialKitDomain.Default, "Updating native plugins exporter settings.");

            var     currentPlatform     = PlatformMappingServices.GetActivePlatform();
            foreach (var exporter in NativePluginsExporterObject.FindObjects<PBXNativePluginsExporterObject>(includeInactive: true))
            {
                switch (exporter.name)
                {
                    case NativeFeatureType.kAddressBook:
                    case NativeFeatureType.kNativeUI:
                    case NativeFeatureType.kSharingServices:
                    case NativeFeatureType.kCloudServices:
                    case NativeFeatureType.kGameServices:
                    case NativeFeatureType.kBillingServices:
                    case NativeFeatureType.kNetworkServices:
                    case NativeFeatureType.kWebView:
                    case NativeFeatureType.kMediaServices:
                        exporter.IsEnabled  = Settings.IsFeatureUsed(exporter.name); 
                        break;
                        
                    case NativeFeatureType.kNotificationServices:
                        var     notificationServicesSettings    = Settings.NotificationServicesSettings;
                        exporter.IsEnabled  = notificationServicesSettings.IsEnabled;
                        exporter.ClearCapabilities();
                        exporter.ClearMacros();
                        exporter.AddMacro(name: "NATIVE_PLUGINS_USES_NOTIFICATION", value: "1");
                        exporter.RemoveFramework(new PBXFramework("CoreLocation.framework"));
                        if ((PushNotificationServiceType.Custom == notificationServicesSettings.PushNotificationServiceType) && exporter.IsEnabled)
                        {
                            exporter.AddMacro(name: "NATIVE_PLUGINS_USES_PUSH_NOTIFICATION", value: "1");
                            exporter.AddCapability(PBXCapability.PushNotifications());
                        }
                        if (notificationServicesSettings.UsesLocationBasedNotification)
                        {
                            exporter.AddMacro(name: "NATIVE_PLUGINS_USES_CORE_LOCATION", value: "1");
                            exporter.AddFramework(new PBXFramework("CoreLocation.framework"));
                        }
                        break;

                    case NativeFeatureType.kDeepLinkServices:
                        var     deepLinkSettings    = Settings.DeepLinkServicesSettings;
                        var     associatedDomains   = deepLinkSettings.GetUniversalLinksForPlatform(currentPlatform);
                        exporter.IsEnabled          = deepLinkSettings.IsEnabled;
                        exporter.ClearCapabilities();
                        if (deepLinkSettings.IsEnabled && associatedDomains.Length > 0)
                        {
                            var     domains         = Array.ConvertAll(associatedDomains, (item) =>
                            {
                                string  serviceType = string.IsNullOrEmpty(item.ServiceType) ? "applinks" : item.ServiceType;
                                return string.Format("{0}:{1}", serviceType, item.Host);
                            });
                            exporter.AddCapability(PBXCapability.AssociatedDomains(domains));
                        }
                        break;
                        
                    default:
                        break;
                }
                EditorUtility.SetDirty(exporter);
            }
        }

        public override void OnUpdateLinkXml(LinkXmlWriter writer)
        {
            // Check whether plugin is configured
            if (!EnsureInitialised()) return;

            // Add active configurations
            var     usedFeaturesMap = ImplementationSchema.GetAllRuntimeConfigurations(settings: Settings);
            if (EssentialKitBuildUtility.IsReleaseBuild() && usedFeaturesMap.Length > 0)
            {
                var     platform    = EditorApplicationUtility.ConvertBuildTargetToRuntimePlatform(BuildTarget);
                foreach (var current in usedFeaturesMap)
                {
                    string  name    = current.Key;
                    var     config  = current.Value;
                    writer.AddConfiguration(name, config, platform, useFallbackPackage : !Settings.IsFeatureUsed(name));
                }
            }
        }

        public override void OnUpdateConfiguration()
        {
            // Check whether plugin is configured
            if (!EnsureInitialised()) return;

            UpdateUnityPreprocessor();
        }

        public override void OnAddFiles()
        {
            // Check whether plugin is configured
            if (!EnsureInitialised()) return;

            var     buildTarget = Manager.BuildTarget;
            if ((buildTarget == BuildTarget.iOS) ||
                (buildTarget == BuildTarget.tvOS))
            {
                var     pluginsManager  = Manager as PBXNativePluginsManager;
                var     compilerFlags   = new string[0];
                var     files           = GeneratePBXBindingsFilesForUnusedFeatures(buildTarget);
                foreach (var item in files)
                {
                    pluginsManager.AddFile(item, "VoxelBusters/EssentialKit/Unused/", compilerFlags);
                }
            }
        }

        public override void OnUpdateInfoPlist(PlistDocument doc)
        {
            // Check whether plugin is configured
            if (!EnsureInitialised()) return;

            // Add usage permissions
            var     rootDict    = doc.root;
            var     permissions = GetUsagePermissions();
            foreach (string key in permissions.Keys)
            {
                rootDict.SetString(key, permissions[key]);
            }

            // Add LSApplicationQueriesSchemes
            string[]    appQuerySchemes = GetApplicationQueriesSchemes();
            if (appQuerySchemes.Length > 0)
            {
                PlistElementArray   array;
                if (!rootDict.TryGetElement(InfoPlistKey.kNSQuerySchemes, out array))
                {
                    array = rootDict.CreateArray(InfoPlistKey.kNSQuerySchemes);
                }

                for (int iter = 0; iter < appQuerySchemes.Length; iter++)
                {
                    if (false == array.Contains(appQuerySchemes[iter]))
                    {
                        array.AddString(appQuerySchemes[iter]);
                    }
                }
            }

            // Add deeplinks
            var     deepLinkCustomSchemeUrls    = GetDeepLinkCustomSchemeUrls();
            if (deepLinkCustomSchemeUrls.Length > 0)
            {
                PlistElementArray   urlTypes;
                if (false == rootDict.TryGetElement(InfoPlistKey.kCFBundleURLTypes, out urlTypes))
                {
                    urlTypes    = rootDict.CreateArray(InfoPlistKey.kCFBundleURLTypes);
                }

                foreach (var current in deepLinkCustomSchemeUrls)
                {
                    var     newElement      = urlTypes.AddDict();
                    newElement.SetString(InfoPlistKey.kCFBundleURLName, current.Identifier);

                    var     schemeArray     = newElement.CreateArray(InfoPlistKey.kCFBundleURLSchemes);
                    schemeArray.AddString(current.Scheme);
                }
            }
        }

#endregion

#region Private methods

        private bool EnsureInitialised()
        {
            if (Settings != null) return true;

            if (EssentialKitSettingsEditorUtility.TryGetDefaultSettings(out EssentialKitSettings settings))
            {
                Settings    = settings;
                return true;
            }

            return false;
        }

        private string[] GeneratePBXBindingsFilesForUnusedFeatures(BuildTarget buildTarget)
        {
            var     files               = new List<string>();
            string  outputPath          = IOServices.CombinePath(Manager.OutputPath, "VoxelBusters", "EssentialKit", "Unused");
            var     generator           = new NativeBindingsGenerator(outputPath: outputPath, options: NativeBindingsGeneratorOptions.Source)
                .SetAuthor("Ashwin Kumar")
                .SetProduct("Native Plugins")
                .SetCopyrights("Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.");
            var     targetPlatform      = ApplicationServices.ConvertBuildTargetToRuntimePlatform(buildTarget);
            foreach (var item in ImplementationSchema.GetAllRuntimeConfigurations())
            {
                if (Settings.IsFeatureUsed(item.Key)) continue;

                if (item.Key.Equals(NativeFeatureType.kExtras)) continue;

                var     runtimePackage  = item.Value.GetPackageForPlatform(targetPlatform);
                foreach (var bindingType in runtimePackage.GetBindingTypeReferences())
                {
                    Type[]  customTypes = null;
                    if (bindingType == typeof(BillingServicesBinding))
                    {
                        customTypes     = new Type[] { typeof(SKPaymentTransactionData) };
                    }
                    generator.Generate(bindingType,
                                       $"NP{bindingType.Name}",
                                       buildTarget,
                                       customTypes,
                                       out string[] outputFiles);
                    files.AddRange(outputFiles);
                }
            }
            return files.ToArray();
        }

        private Dictionary<string, string> GetUsagePermissions()
        {
            var     requiredPermissionsDict     = new Dictionary<string, string>(4);
            var     permissionSettings          = Settings.ApplicationSettings.UsagePermissionSettings;

            // AddressBook permissions
            var     abSettings                  = Settings.AddressBookSettings;
            if (abSettings.IsEnabled)
            {
                requiredPermissionsDict[InfoPlistKey.kNSContactsUsage] = permissionSettings.AddressBookUsagePermission.GetDescription(RuntimePlatform.IPhonePlayer);
            }

            // Media permissions
            var     mediaSettings               = Settings.MediaServicesSettings;
            if (mediaSettings.IsEnabled)
            {
                if (mediaSettings.UsesCamera)
                {
                    requiredPermissionsDict[InfoPlistKey.kNSCameraUsage]       = permissionSettings.CameraUsagePermission.GetDescription(RuntimePlatform.IPhonePlayer);
                }
                if (mediaSettings.UsesGallery)
                {
                    requiredPermissionsDict[InfoPlistKey.kNSPhotoLibraryUsage] = permissionSettings.GalleryUsagePermission.GetDescription(RuntimePlatform.IPhonePlayer);
                }
                if (mediaSettings.SavesFilesToGallery)
                {
                    requiredPermissionsDict[InfoPlistKey.kNSPhotoLibraryAdd]   = permissionSettings.GalleryWritePermission.GetDescription(RuntimePlatform.IPhonePlayer);
                }
            }

            // Notification permissions
            var     notificationSettings        = Settings.NotificationServicesSettings;
            if (notificationSettings.IsEnabled)
            {
                if (notificationSettings.UsesLocationBasedNotification)
                {
                    requiredPermissionsDict[InfoPlistKey.kNSLocationWhenInUse] = permissionSettings.LocationWhenInUsePermission.GetDescription(RuntimePlatform.IPhonePlayer);
                }
            }

            // Sharing permissions
            var sharingSettings = Settings.SharingServicesSettings;
            if (sharingSettings.IsEnabled)
            {
                // added for supporting sharing/saving to gallery when share sheet is shown
                requiredPermissionsDict[InfoPlistKey.kNSPhotoLibraryAdd]   = permissionSettings.GalleryWritePermission.GetDescription(RuntimePlatform.IPhonePlayer);
            }

            return requiredPermissionsDict;
        }

        private string[] GetApplicationQueriesSchemes()
        {
            var     sharingSettings = Settings.SharingServicesSettings;
            var     schemeList      = new List<string>();
            if (sharingSettings.IsEnabled)
            {
                schemeList.Add("fb");
                schemeList.Add("twitter");
                schemeList.Add("whatsapp");
            }

            return schemeList.ToArray();
        }

        private DeepLinkDefinition[] GetDeepLinkCustomSchemeUrls()
        {
            var     deepLinkSettings    = Settings.DeepLinkServicesSettings;
            if (deepLinkSettings.IsEnabled)
            {
                var     currentPlatform = PlatformMappingServices.GetActivePlatform();
                return deepLinkSettings.GetCustomSchemeUrlsForPlatform(currentPlatform);
            }
            
            return new DeepLinkDefinition[0];
        }

        private void UpdateUnityPreprocessor()
        {
            var     notificationSettings    = Settings.NotificationServicesSettings;
            if (notificationSettings.IsEnabled && notificationSettings.PushNotificationServiceType == PushNotificationServiceType.Custom)
            {
                string  preprocessorPath    = OutputPath + "/Classes/Preprocessor.h";
                string  text                = File.ReadAllText(preprocessorPath);
                text                        = text.Replace("UNITY_USES_REMOTE_NOTIFICATIONS 0", "UNITY_USES_REMOTE_NOTIFICATIONS 1");
                File.WriteAllText(preprocessorPath, text);
            }
        }

#endregion
    }
}
#endif