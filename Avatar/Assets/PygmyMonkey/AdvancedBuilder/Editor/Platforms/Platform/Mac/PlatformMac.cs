using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PygmyMonkey.AdvancedBuilder
{
    [Serializable]
    public class PlatformMac : IPlatform
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
		 * Constructor
		 */
        public PlatformMac()
        {
            m_platformProperties = new PlatformProperties(
                PlatformType.Mac,
                "Mac",
                new List<DistributionPlatform> { },
                GetDefaultPlatformArchitectures(),
				new List<ITextureCompression>() { new DefaultTextureCompression() }
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
                #if UNITY_2017_3_OR_NEWER
                new PlatformArchitecture("OSX", ".app", BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX, true),
                #else
                new PlatformArchitecture("OSX x86", ".app", BuildTargetGroup.Standalone, BuildTarget.StandaloneOSXIntel, true),
                new PlatformArchitecture("OSX x86_64", ".app", BuildTargetGroup.Standalone, BuildTarget.StandaloneOSXIntel64),
                new PlatformArchitecture("OSX Universal", ".app", BuildTargetGroup.Standalone, BuildTarget.StandaloneOSXUniversal),
                #endif
            };
        }
		
		
		/*
		 * Set up additional parameters
		 */
		public void setupAdditionalParameters(ProductParameters productParameters, Configuration configuration)
		{
		}
		
		
		/*
		 * Format destination file path
		 */
		public string formatDestinationPath(string filePath)
		{
			return filePath;
		}
		
		
		/*
		 * Return specific platform errors
		 */
		public void checkWarningsAndErrors(ErrorReporter errorReporter)
		{
		}
	}
}