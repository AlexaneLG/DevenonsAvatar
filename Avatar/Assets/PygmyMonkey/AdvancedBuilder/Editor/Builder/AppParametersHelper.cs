using UnityEditor;
using PygmyMonkey.AdvancedBuilder.Utils;

namespace PygmyMonkey.AdvancedBuilder
{
	public static class AppParametersHelper
	{
		private static int m_androidBundleVersionCode;
		private static MobileTextureSubtarget m_androidTextureSubTarget;
		private static string m_iOSApplicationDisplayName;
		private static string m_bundleIdentifier;
		private static string m_productName;
		private static string m_bundleVersion;

		private static BuildTarget m_buildTarget;
		private static BuildTargetGroup m_buildTargetGroup;

		private static string m_releaseType = "";
		private static string m_platformType = "";
		private static string m_distributionPlatform = "";
		private static string m_platformArchitecture = "";
		private static string m_textureCompression = "";
		private static string m_appParamBundleIdentifier;
		private static string m_appParamProductName;

		public static void SaveParameters()
		{
			m_androidBundleVersionCode = PlayerSettings.Android.bundleVersionCode;
			m_androidTextureSubTarget = EditorUserBuildSettings.androidBuildSubtarget;

			m_iOSApplicationDisplayName = PlayerSettings.iOS.applicationDisplayName;
			m_bundleIdentifier = PlayerSettings.applicationIdentifier;
			m_productName = PlayerSettings.productName;
			m_bundleVersion = PlayerSettings.bundleVersion;

			m_releaseType = AppParameters.Get.releaseType;
			m_platformType = AppParameters.Get.platformType;
			m_distributionPlatform = AppParameters.Get.distributionPlatform;
			m_platformArchitecture = AppParameters.Get.platformArchitecture;
			m_textureCompression = AppParameters.Get.textureCompression;
			m_appParamBundleIdentifier = AppParameters.Get.bundleIdentifier;
			m_appParamProductName = AppParameters.Get.productName;
		}

		public static void RestoreParameters()
		{
			PlayerSettings.Android.bundleVersionCode = m_androidBundleVersionCode;
			EditorUserBuildSettings.androidBuildSubtarget = m_androidTextureSubTarget;

            PlayerSettings.iOS.applicationDisplayName = m_iOSApplicationDisplayName;
			PlayerSettings.SetApplicationIdentifier(EditorUserBuildSettings.selectedBuildTargetGroup, m_bundleIdentifier);
			PlayerSettings.productName = m_productName;
			PlayerSettings.bundleVersion = m_bundleVersion;

			AppParameters.Get.updateParameters(m_releaseType, m_platformType, m_distributionPlatform, m_platformArchitecture, m_textureCompression, m_appParamBundleIdentifier, m_appParamProductName, AppParameters.Get.bundleVersion, AppParameters.Get.buildNumber);

			GUIUtils.RefreshAssets();
		}

		public static void SaveBuildTarget()
		{
			m_buildTarget = EditorUserBuildSettings.activeBuildTarget;
			m_buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
		}

		public static void RestoreBuildTarget()
		{
			EditorUserBuildSettings.SwitchActiveBuildTarget(m_buildTargetGroup, m_buildTarget);
		}
	}
}
