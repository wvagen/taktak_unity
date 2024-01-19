// Copyright (c) 2015 - 2023 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

//.........................
//.....Generated Class.....
//.........................
//.......Do not edit.......
//.........................

using System.Collections.Generic;
// ReSharper disable All
namespace Doozy.Runtime.UIManager.Containers
{
    public partial class UIView
    {
        public static IEnumerable<UIView> GetViews(UIViewId.Auth id) => GetViews(nameof(UIViewId.Auth), id.ToString());
        public static void Show(UIViewId.Auth id, bool instant = false) => Show(nameof(UIViewId.Auth), id.ToString(), instant);
        public static void Hide(UIViewId.Auth id, bool instant = false) => Hide(nameof(UIViewId.Auth), id.ToString(), instant);

        public static IEnumerable<UIView> GetViews(UIViewId.Home id) => GetViews(nameof(UIViewId.Home), id.ToString());
        public static void Show(UIViewId.Home id, bool instant = false) => Show(nameof(UIViewId.Home), id.ToString(), instant);
        public static void Hide(UIViewId.Home id, bool instant = false) => Hide(nameof(UIViewId.Home), id.ToString(), instant);

        public static IEnumerable<UIView> GetViews(UIViewId.PopUp id) => GetViews(nameof(UIViewId.PopUp), id.ToString());
        public static void Show(UIViewId.PopUp id, bool instant = false) => Show(nameof(UIViewId.PopUp), id.ToString(), instant);
        public static void Hide(UIViewId.PopUp id, bool instant = false) => Hide(nameof(UIViewId.PopUp), id.ToString(), instant);
    }
}

namespace Doozy.Runtime.UIManager
{
    public partial class UIViewId
    {
        public enum Auth
        {
            SignInEmail,
            SignInOptions
        }

        public enum Home
        {
            HistoryAndStats,
            LiveMission,
            Mission,
            Profile,
            Shop
        }

        public enum PopUp
        {
            error,
            info,
            Panel,
            warning
        }    
    }
}
