namespace PygmyMonkey.AdvancedBuilder
{
	public interface IAdvancedCustomBuild
	{
		void OnEveryBuildStart();
		void OnPreBuild(Configuration configuration, System.DateTime buildDate);
		void OnPostBuild(Configuration configuration, System.DateTime buildDate);
		void OnEveryBuildDone();
		void OnPreApplyConfiguration(Configuration configuration, bool isBuilding);
		void OnPostApplyConfiguration(Configuration configuration, bool isBuilding);
	}
}
