using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PygmyMonkey.AdvancedBuilder
{
	[Serializable]
	public class PlatformWindows : IPlatform
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
		public PlatformWindows()
		{
			m_platformProperties = new PlatformProperties(
				PlatformType.Windows,
				"Windows",
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
                new PlatformArchitecture("Windows x86", ".exe", BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows),
                new PlatformArchitecture("Windows x86_64", ".exe", BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64, true),
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