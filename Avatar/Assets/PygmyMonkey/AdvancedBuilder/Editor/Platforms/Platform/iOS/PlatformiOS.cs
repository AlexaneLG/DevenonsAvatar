using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PygmyMonkey.AdvancedBuilder
{
	[Serializable]
	public class PlatformiOS : IPlatform
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
		public PlatformiOS()
		{
			m_platformProperties = new PlatformProperties(
				PlatformType.iOS,
				"iOS",
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
                new PlatformArchitecture(null, "", BuildTargetGroup.iOS, BuildTarget.iOS, true),
            };
        }


		/*
		 * Set up additional parameters
		 */
		public void setupAdditionalParameters(ProductParameters productParameters, Configuration configuration)
		{
			PlayerSettings.iOS.applicationDisplayName = configuration.releaseType.productName;
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