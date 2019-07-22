using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace PygmyMonkey.AdvancedBuilder
{
    [Serializable]
    public class PlatformAndroid : IPlatform
    {
        /*
		 * Platform common properties
		 */
        [SerializeField] private PlatformProperties m_platformProperties;

        public PlatformProperties getPlatformProperties()
        {
            return m_platformProperties;
        }


        /*
		 * List of supported texture compression
		 */
        [SerializeField]
        private List<TextureCompressionAndroid> m_textureCompressionList = new List<TextureCompressionAndroid>()
        {
            new TextureCompressionAndroid(MobileTextureSubtarget.ASTC,      16),
            new TextureCompressionAndroid(MobileTextureSubtarget.ETC2,      15),
            new TextureCompressionAndroid(MobileTextureSubtarget.PVRTC,     14),
            new TextureCompressionAndroid(MobileTextureSubtarget.DXT,       13),
            #if !UNITY_2018_1_OR_NEWER
			new TextureCompressionAndroid(MobileTextureSubtarget.ATC,		12),
            #endif
			new TextureCompressionAndroid(MobileTextureSubtarget.ETC,       11),
            new TextureCompressionAndroid(MobileTextureSubtarget.Generic,   10, true),
        };


        /*
		 * Android device filters
		 */
#if UNITY_2018_1_OR_NEWER
		private const string m_all3DeviceFilter = "ALL (ARM64 + ARMv7 + x86)";
        private const string m_arm64DeviceFilter = "ARM64";
#endif

		private const string m_fatDeviceFilter = "FAT (ARMv7 + x86)";
        private const string m_armv7DeviceFilter = "ARMv7";
        private const string m_x86DeviceFilter = "x86";


        /*
		 * Constructor
		 */
        public PlatformAndroid()
        {
            m_platformProperties = new PlatformProperties(
                PlatformType.Android,
                "Android",
                new List<DistributionPlatform>
                {
                    new DistributionPlatform("Google Play", true),
                    new DistributionPlatform("Amazon Store"),
                    new DistributionPlatform("Samsung AppStore")
                },
                GetDefaultPlatformArchitectures(),
                m_textureCompressionList.Cast<ITextureCompression>().ToList()
            );
        }


        public void resetPlatformArchitectures()
        {
            m_platformProperties.setPlatformArchitectureList(GetDefaultPlatformArchitectures());
        }


        public List<PlatformArchitecture> GetDefaultPlatformArchitectures()
        {
            return new List<PlatformArchitecture>
            {
                #if UNITY_2018_1_OR_NEWER
                new PlatformArchitecture(m_all3DeviceFilter, ".apk", BuildTargetGroup.Android, BuildTarget.Android, true),
                #endif
				new PlatformArchitecture(m_fatDeviceFilter, ".apk", BuildTargetGroup.Android, BuildTarget.Android, true),
                #if UNITY_2018_1_OR_NEWER
                new PlatformArchitecture(m_arm64DeviceFilter, ".apk", BuildTargetGroup.Android, BuildTarget.Android, false),
                #endif
                new PlatformArchitecture(m_armv7DeviceFilter, ".apk", BuildTargetGroup.Android, BuildTarget.Android, false),
                new PlatformArchitecture(m_x86DeviceFilter, ".apk", BuildTargetGroup.Android, BuildTarget.Android, false),
            };
        }


        /*
		 * Return Android BundleVersion based on TextureCompression and BundleVersion
		 * Format: xxyyzzz
		 * xx: Device filter number
		 * yy: Texture compression number
		 * zzz: BundleVersion number (0.3.0 -> 030)
		 */
        public int getAndroidBundleVersionCode(PlatformArchitecture platformArchitecture, TextureCompressionAndroid textureCompressionAndroid, ProductParameters productParameters)
        {
            int deviceFilterNumber = 0;

            #if UNITY_2018_1_OR_NEWER
            if (platformArchitecture.name.Equals(m_all3DeviceFilter))
            {
                deviceFilterNumber = 14;
            }
            if (platformArchitecture.name.Equals(m_arm64DeviceFilter))
            {
                deviceFilterNumber = 13;
            }
            #endif
            if (platformArchitecture.name.Equals(m_x86DeviceFilter))
            {
                deviceFilterNumber = 12;
            }
            if (platformArchitecture.name.Equals(m_armv7DeviceFilter))
            {
                deviceFilterNumber = 11;
            }
            if (platformArchitecture.name.Equals(m_fatDeviceFilter))
            {
                deviceFilterNumber = 10;
            }

            return int.Parse(deviceFilterNumber.ToString() + textureCompressionAndroid.versionCodePrefix + Regex.Replace(productParameters.bundleVersion, "[^0-9]", ""));
        }


        /*
		 * Set up additional parameters
		 */
        public void setupAdditionalParameters(ProductParameters productParameters, Configuration configuration)
        {
            //EditorUserBuildSettings.exportAsGoogleAndroidProject = true;

            #if UNITY_2018_1_OR_NEWER
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.None;

            if (configuration.platformArchitecture.name.Equals(m_all3DeviceFilter))
            {
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64 | AndroidArchitecture.X86;
            }
            else if (configuration.platformArchitecture.name.Equals(m_fatDeviceFilter))
            {
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.X86;
            }
            else if (configuration.platformArchitecture.name.Equals(m_arm64DeviceFilter))
            {
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            }
            else if (configuration.platformArchitecture.name.Equals(m_armv7DeviceFilter))
            {
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
            }
            else if (configuration.platformArchitecture.name.Equals(m_x86DeviceFilter))
            {
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.X86;
            }
            #else
			if (configuration.platformArchitecture.name.Equals(m_fatDeviceFilter))
			{
				PlayerSettings.Android.targetDevice = AndroidTargetDevice.FAT;
			}
			else if (configuration.platformArchitecture.name.Equals(m_armv7DeviceFilter))
			{
				PlayerSettings.Android.targetDevice = AndroidTargetDevice.ARMv7;
			}
			else if (configuration.platformArchitecture.name.Equals(m_x86DeviceFilter))
			{
				PlayerSettings.Android.targetDevice = AndroidTargetDevice.x86;
			}
            #endif

			TextureCompressionAndroid textureCompressionAndroid = (TextureCompressionAndroid)configuration.textureCompression;
			PlayerSettings.Android.bundleVersionCode = getAndroidBundleVersionCode(configuration.platformArchitecture, textureCompressionAndroid, productParameters);
			EditorUserBuildSettings.androidBuildSubtarget = textureCompressionAndroid.subTarget;
		}
		
		
		/*
		 * Format destination file path
		 */
		public string formatDestinationPath(string filePath)
		{
			return filePath;
		}
	}
}