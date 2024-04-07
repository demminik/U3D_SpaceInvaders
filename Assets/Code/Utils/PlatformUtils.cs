using UnityEngine;

namespace SpaceInvaders.Utils {

    public static class PlatformUtils {

        public enum EPlatformType {

            Unknown = 0,
            Mobile,
            Standalone,
        }

        public static EPlatformType CurrentPlatformType {
            get {
                if (Application.isEditor) {
                    return EPlatformType.Standalone;
                }
                if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
                    return EPlatformType.Mobile;
                }
                return EPlatformType.Standalone;
            }
        }
    }
}