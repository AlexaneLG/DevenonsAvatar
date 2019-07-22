using System.Collections.Generic;

namespace PygmyMonkey.AdvancedBuilder
{
	public interface IPlatform
	{
		/*
		 * Platform common properties
		 */
		PlatformProperties getPlatformProperties();


        void resetPlatformArchitectures();


        List<PlatformArchitecture> GetDefaultPlatformArchitectures();

		/*
		 * Set up additional parameters
		 */
		void setupAdditionalParameters(ProductParameters productParameters, Configuration configuration);


		/*
		 * Format destination file path
		 */
		string formatDestinationPath(string filePath);
	}
}