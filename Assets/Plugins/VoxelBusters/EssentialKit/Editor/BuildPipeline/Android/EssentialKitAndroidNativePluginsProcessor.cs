﻿#if UNITY_EDITOR && UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEditor;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.Editor;
using VoxelBusters.CoreLibrary.Editor.NativePlugins.Build;


namespace VoxelBusters.EssentialKit.Editor.Build.Android
{
    public class EssentialKitAndroidNativePluginsProcessor : VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Android.AndroidNativePluginsProcessor
    {
        #region Properties

        private EssentialKitSettings Settings { get; set; }

        #endregion

        #region Overriden methods

        public override void OnCheckConfiguration()
        {
            if (!EnsureInitialized())
                return;

            StringBuilder builder = new StringBuilder();

            if (Settings.BillingServicesSettings.IsEnabled)
            {
                if (string.IsNullOrEmpty(Settings.BillingServicesSettings.AndroidProperties.PublicKey))
                {
                    builder.Append("Billing feature needs Public Key to be set in EssentialKit Settings inspector \n");
                }
            }

            if (Settings.DeepLinkServicesSettings.IsEnabled)
            {
                if (Settings.DeepLinkServicesSettings.AndroidProperties.CustomSchemeUrls.Length == 0 && Settings.DeepLinkServicesSettings.AndroidProperties.UniversalLinks.Length == 0)
                {
                    builder.Append("Deep Link Services feature needs atleast one entry of Url schemes or Universal links in EssentialKit Settings inspector \n");
                    builder.Append("In-case if you don't want to use Deep Link services, disable them in EssentialKit settings inspector \n\n");
                }
            }

            if (Settings.CloudServicesSettings.IsEnabled || Settings.GameServicesSettings.IsEnabled)
            {
                if (string.IsNullOrEmpty(Settings.GameServicesSettings.AndroidProperties.PlayServicesApplicationId))
                {
                    builder.Append("Game Services or Cloud Services need PlayServicesApplicationId field to be specified on Android\n");
                }
            }

            if (Settings.MediaServicesSettings.IsEnabled)
            {
                if (!(Settings.MediaServicesSettings.SavesFilesToGallery || Settings.MediaServicesSettings.UsesCamera || Settings.MediaServicesSettings.UsesGallery))
                {
                    builder.Append("Media Services needs atleast one of the listed flags enabled in EssentialKit Settings inspector \n");
                }
            }

            if (Settings.NotificationServicesSettings.IsEnabled && Settings.NotificationServicesSettings.PushNotificationServiceType == PushNotificationServiceType.Custom)
            {
                if (!IOServices.FileExists(FirebaseSettingsGenerator.kGoogleServicesJsonPath))
                {
                    builder.Append("Please add google-services.json under Assets folder for using Firebase Cloud Messaging(Push Notifications). You can fetch the file from Firebase console under your project -> Project Settings -> General : https://console.firebase.google.com.\n");
                    builder.Append("In-case if you don't want to use Push notifications/Remote notifications, set Push notification service type to \"None\" in EssentialKit settings inspector \n");
                }

            }

            if (builder.Length != 0)
            {
                string error = string.Format("[VoxelBusters : {0}] \n{1}", EssentialKitSettings.DisplayName, builder.ToString());
                if (Debug.isDebugBuild)
                {
                    Debug.LogError(error, Settings);
                }
                else
                {
                    Debug.LogWarning(error, Settings);
                }
            }
        }

        public override void OnUpdateLinkXml(LinkXmlWriter writer)
        {
            if (!EnsureInitialized())
                return;

            // Add active configurations
            var allFeaturesMap = ImplementationSchema.GetAllRuntimeConfigurations(settings: Settings);
            var platform = EditorApplicationUtility.ConvertBuildTargetToRuntimePlatform(BuildTarget);
            foreach (var current in allFeaturesMap)
            {
                string name = current.Key;
                var config = current.Value;
                writer.AddConfiguration(name, config, platform, useFallbackPackage : !Settings.IsFeatureUsed(name));
            }
        }

        public override void OnAddFiles()
        {
            if (!EnsureInitialized())
                return;


            // generate manifest
            AndroidManifestGenerator.GenerateManifest(Settings);

            if (IsNotificationServiceEnabled() && IsPushNotificationServiceTypeCustom())
            {
                // generate google-services xml
                FirebaseSettingsGenerator.Execute();
            }

            // generate dependencies
            AndroidLibraryDependenciesGenerator.CreateLibraryDependencies();

        }



        public override void OnAddFolders()
        {
            if (!EnsureInitialized())
                return;
        }

        public override void OnAddResources()
        {
            if (!EnsureInitialized())
                return;

            if (IsNotificationServiceEnabled())
            {
                CopyNotificationIcons();
                CopySoundsFromStreamingAssets();
            }

            UpdateStringsXml();
        }

        public override void OnUpdateConfiguration()
        {
            if (!EnsureInitialized())
                return;
        }

        #endregion

        #region Helper methods

        private bool EnsureInitialized()
        {
            if (Settings != null) return true;

            if (EssentialKitSettingsEditorUtility.TryGetDefaultSettings(out EssentialKitSettings settings))
            {
                Settings = settings;
                return true;
            }

            return false;
        }

        private void CopyNotificationIcons()
        {
            // copying white and colored notification icons if exist
            string assetPath = AssetDatabase.GetAssetPath(Settings.NotificationServicesSettings.AndroidProperties.WhiteSmallIcon);
            if (!string.IsNullOrEmpty(assetPath))
            {
                IOServices.CopyFile(assetPath, IOServices.CombinePath(EssentialKitPackageLayout.AndroidProjectResDrawablePath, "app_icon_custom_white.png"));
            }

            assetPath = AssetDatabase.GetAssetPath(Settings.NotificationServicesSettings.AndroidProperties.ColouredSmallIcon);
            if (!string.IsNullOrEmpty(assetPath))
                IOServices.CopyFile(assetPath, IOServices.CombinePath(EssentialKitPackageLayout.AndroidProjectResDrawablePath, "app_icon_custom_coloured.png"));
        }

        private void CopySoundsFromStreamingAssets()
        {
            // Copy audio files from streaming assets if any to Raw folder
            string path = UnityEngine.Application.streamingAssetsPath;
            if (IOServices.DirectoryExists(path))
            {
                string[] files = System.IO.Directory.GetFiles(path);
                
                string destinationFolder = EssentialKitPackageLayout.AndroidProjectResRawPath;
                IOServices.CreateDirectory(destinationFolder);

                string[] formats = new string[]
                {
                    ".mp3",
                    ".wav",
                    ".ogg"
                };

                for (int i = 0; i < files.Length; i++)
                {
                    string extension = System.IO.Path.GetExtension(files[i]);
                    if (formats.Contains(extension.ToLower()))
                    {
                        IOServices.CopyFile(files[i], IOServices.CombinePath(destinationFolder, IOServices.GetFileName(files[i]).ToLower()));
                    }
                }
            }
        }

        private void UpdateStringsXml()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();

            if (Settings.AddressBookSettings.IsEnabled)
            {
                config.Add("ADDRESS_BOOK_PERMISSON_REASON", Settings.ApplicationSettings.UsagePermissionSettings.AddressBookUsagePermission.GetDescriptionForActivePlatform());
            }
            if (Settings.NotificationServicesSettings.IsEnabled)
            {
                config.Add("NOTIFICATION_SERVICES_CONTENT_TITLE_KEY", Settings.NotificationServicesSettings.AndroidProperties.PayloadKeys.ContentTitleKey);
                config.Add("NOTIFICATION_SERVICES_CONTENT_TEXT_KEY", Settings.NotificationServicesSettings.AndroidProperties.PayloadKeys.ContentTextKey);
                config.Add("NOTIFICATION_SERVICES_TICKER_TEXT_KEY", Settings.NotificationServicesSettings.AndroidProperties.PayloadKeys.TickerTextKey);
                config.Add("NOTIFICATION_SERVICES_BADGE_KEY", Settings.NotificationServicesSettings.AndroidProperties.PayloadKeys.BadgeKey);
                config.Add("NOTIFICATION_SERVICES_PRIORITY_KEY", Settings.NotificationServicesSettings.AndroidProperties.PayloadKeys.PriorityKey);
                config.Add("NOTIFICATION_SERVICES_SOUND_FILE_NAME_KEY", Settings.NotificationServicesSettings.AndroidProperties.PayloadKeys.SoundFileNameKey);
                config.Add("NOTIFICATION_SERVICES_BIG_PICTURE_KEY", Settings.NotificationServicesSettings.AndroidProperties.PayloadKeys.BigPictureKey);
                config.Add("NOTIFICATION_SERVICES_LARGE_ICON_KEY", Settings.NotificationServicesSettings.AndroidProperties.PayloadKeys.LargeIconKey);
                config.Add("NOTIFICATION_SERVICES_USER_INFO_KEY", Settings.NotificationServicesSettings.AndroidProperties.PayloadKeys.UserInfoKey);


                config.Add("NOTIFICATION_SERVICES_USES_PUSH_NOTIFICATION_SERVICE", (Settings.NotificationServicesSettings.PushNotificationServiceType == PushNotificationServiceType.Custom) ? "true" : "false");
                config.Add("NOTIFICATION_SERVICES_USES_EXTERNAL_SERVICE", (Settings.NotificationServicesSettings.PushNotificationServiceType != PushNotificationServiceType.Custom) ? "true" : "false");
                config.Add("NOTIFICATION_SERVICES_NEEDS_VIBRATION", Settings.NotificationServicesSettings.AndroidProperties.AllowVibration ? "true" : "false");
                config.Add("NOTIFICATION_SERVICES_NEEDS_CUSTOM_ICON", Settings.NotificationServicesSettings.AndroidProperties.WhiteSmallIcon ? "true" : "false");
                config.Add("NOTIFICATION_SERVICES_ALLOW_NOTIFICATION_DISPLAY_WHEN_APP_IS_FOREGROUND", Settings.NotificationServicesSettings.AndroidProperties.AllowNotificationDisplayWhenForeground ? "true" : "false");
                config.Add("NOTIFICATION_SERVICES_ACCENT_COLOR", Settings.NotificationServicesSettings.AndroidProperties.AccentColor);
                config.Add("NOTIFICATION_SERVICES_ALLOW_IN_EXACT_TIME_SCHEDULING", Settings.NotificationServicesSettings.AndroidProperties.AllowInExactTimeScheduling ? "true" : "false");
            }

            if (Settings.GameServicesSettings.IsEnabled)
            {
                config.Add("GAME_SERVICES_SERVER_CLIENT_ID", Settings.GameServicesSettings.AndroidProperties.ServerClientId?.Trim());
                config.Add("GAME_SERVICES_SHOW_ERROR_DIALOGS", Settings.GameServicesSettings.AndroidProperties.ShowErrorDialogs ? "true" : "false");
                config.Add("GAME_SERVICES_NEEDS_POPUPS_AT_TOP", Settings.GameServicesSettings.AndroidProperties.DisplayPopupsAtTop ? "true" : "false");
                config.Add("GAME_SERVICES_NEEDS_PROFILE_SCOPE", Settings.GameServicesSettings.AndroidProperties.NeedsProfileScope ? "true" : "false");
                config.Add("GAME_SERVICES_NEEDS_EMAIL_SCOPE", Settings.GameServicesSettings.AndroidProperties.NeedsEmailScope ? "true" : "false");
            }

            if (Settings.WebViewSettings.IsEnabled)
            {
                config.Add("WEB_VIEW_ALLOW_BACK_NAVIGATION_KEY", Settings.WebViewSettings.AndroidProperties.AllowBackNavigationKey ? "true" : "false");
            }

            config.Add("USES_CLOUD_SERVICES", Settings.CloudServicesSettings.IsEnabled ? "true" : "false");

            XmlDocument xml = new XmlDocument();
            xml.Load(EssentialKitPackageLayout.AndroidProjectResValuesStringsPath);
            XmlNodeList nodes = xml.SelectNodes("/resources/string");

            foreach (XmlNode node in nodes)
            {
                XmlAttribute xmlAttribute = node.Attributes["name"];
                string key = xmlAttribute.Value;

                if (config.ContainsKey(key))
                {
                    node.InnerText = config[key];
                }
            }

            xml.Save(EssentialKitPackageLayout.AndroidProjectResValuesStringsPath);
        }

        private bool IsNotificationServiceEnabled()
        {
            return Settings.NotificationServicesSettings.IsEnabled;
        }
        private bool IsPushNotificationServiceTypeCustom()
        {
            return Settings.NotificationServicesSettings.PushNotificationServiceType == PushNotificationServiceType.Custom;
        }

        #endregion
    }
}
#endif