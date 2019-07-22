using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using PygmyMonkey.AdvancedBuilder.Utils;

namespace PygmyMonkey.AdvancedBuilder
{
    internal class PlatformBuilder
    {
        private readonly AdvancedBuilder m_advancedBuilder;
        private readonly DateTime m_buildDate;
        private readonly IAdvancedCustomBuild m_advancedCustomBuild;

        public PlatformBuilder(AdvancedBuilder advancedBuilder, DateTime buildDate)
        {
            m_advancedBuilder = advancedBuilder;
            m_buildDate = buildDate;

            if (m_advancedBuilder.getAdvancedSettings().customBuildMonoScript != null)
            {
                m_advancedCustomBuild = (IAdvancedCustomBuild)System.Activator.CreateInstance(m_advancedBuilder.getAdvancedSettings().customBuildMonoScript.GetClass());
            }
        }

        public void performBuild(Configuration configuration)
        {
            /*
			 * Update AppParameters file
			 */
            AppParametersHelper.SaveParameters();


            /*
			 * Set Player Settings
			 */
            configuration.applyConfiguration();


            /*
			 * Get destination path
			 */
            string buildDestinationPath = configuration.getBuildDestinationPath(m_advancedBuilder.getAdvancedSettings(), m_buildDate, m_advancedBuilder.getProductParameters());


            /*
			 * Perform the build
			 */
            performPreBuild(configuration, m_buildDate);
            bool success = performBuild(configuration, buildDestinationPath, configuration.platformArchitecture.binarySuffix);
            performPostBuild(configuration, m_buildDate);


            // Hack to prevent violation access when editing AppParameters.cs script
            System.Threading.Thread.Sleep(500);


            /*
			 * Restore AppParameters
			 */
            if (success)
            {
                AppParametersHelper.RestoreParameters();
            }

            EditorUtility.ClearProgressBar();
        }

        private void performPreBuild(Configuration configuration, DateTime buildDate)
        {
            /*
			 * Before building, we refresh all assets
			 */
            GUIUtils.RefreshAssets();

            if (m_advancedCustomBuild != null)
            {
                EditorUtility.DisplayProgressBar("Advanced Builder", "Doing custom pre build stuff...", 0.0f);
                m_advancedCustomBuild.OnPreBuild(configuration, buildDate);
            }
        }

        private bool performBuild(Configuration configuration, string buildDestinationPath, string fileName)
        {
            /*
			 * Create the build folder if it does not exist
			 */
            string buildDirectory = Application.dataPath.Replace("Assets", "Builds");
            Directory.CreateDirectory(buildDirectory);

            /*
			 * Create the subfolder that will contain the final build
			 */
            string finalBuildPath = buildDirectory + "/" + buildDestinationPath + fileName;
            Directory.CreateDirectory(finalBuildPath.Substring(0, finalBuildPath.LastIndexOf("/")));

            /*
			 * Start the build through Unity process
			 */
            EditorUtility.DisplayProgressBar("Advanced Builder", "Building " + finalBuildPath.Replace("/", " - ").Replace(buildDirectory, string.Empty) + "...", 0.5f);

            EditorUserBuildSettings.SwitchActiveBuildTarget(configuration.platformArchitecture.buildTargetGroup, configuration.platformArchitecture.buildTarget);

            // Set Build Options
            BuildOptions buildOptions = BuildOptions.None;

            if (configuration.openBuildFolder)
            {
                buildOptions |= BuildOptions.ShowBuiltPlayer;
            }

            if (configuration.isDevelopmentBuild)
            {
                buildOptions |= BuildOptions.Development;
            }

            if (configuration.shouldAutoconnectProfiler)
            {
                buildOptions |= BuildOptions.ConnectWithProfiler;
            }

            if (configuration.shouldAutoRunPlayer)
            {
                buildOptions |= BuildOptions.AutoRunPlayer;
            }

            if (configuration.allowDebugging)
            {
                buildOptions |= BuildOptions.AllowDebugging;
            }

            if (configuration.appendProject)
            {
                if (configuration.platformType == PlatformType.iOS)
                {
                    if (Directory.Exists(finalBuildPath))
                    {
                        buildOptions |= BuildOptions.AcceptExternalModificationsToPlayer;
                    }
                }
                else
                {
                    if (configuration.platformType == PlatformType.Android)
                    {
                        finalBuildPath = Directory.GetParent(finalBuildPath).FullName;
                    }

                    buildOptions |= BuildOptions.AcceptExternalModificationsToPlayer;
                }
            }

            if (configuration.platformType == PlatformType.Linux && configuration.isHeadlessMode)
            {
                buildOptions |= BuildOptions.EnableHeadlessMode;
            }

            if (m_advancedBuilder.getAdvancedSettings().useSymlinkLibraries)
            {
                buildOptions |= BuildOptions.SymlinkLibraries;
            }

            // Build Player
            bool isBuildSuccess = false;

#if UNITY_2018_1_OR_NEWER
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = GetActiveScenePathArray();
            buildPlayerOptions.locationPathName = finalBuildPath;
            buildPlayerOptions.target = configuration.platformArchitecture.buildTarget;
            buildPlayerOptions.options = buildOptions;
            UnityEditor.Build.Reporting.BuildReport buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);
            isBuildSuccess = buildReport.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded;
#else
			string output = BuildPipeline.BuildPlayer(GetActiveScenePathArray(), finalBuildPath, configuration.platformArchitecture.buildTarget, buildOptions);
            isBuildSuccess = string.IsNullOrEmpty(output);
#endif

            if (!isBuildSuccess)
			{
				Debug.LogError("An error has occurred while building... Check the console to see what happened");

				EditorUtility.ClearProgressBar();
				return false;
			}

			EditorUtility.DisplayProgressBar("Advanced Builder", "Doing post build stuff...", 1.0f);

			return true;
		}

		private void performPostBuild(Configuration configuration, DateTime buildDate)
		{
			if (m_advancedCustomBuild != null)
			{
				EditorUtility.DisplayProgressBar("Advanced Builder", "Doing custom build stuff...", 1.0f);
				m_advancedCustomBuild.OnPostBuild(configuration, buildDate);
			}

			/*
			 * After building, we increment the buildNumber
			 */
			m_advancedBuilder.getProductParameters().buildNumber++;
			EditorUtility.SetDirty(m_advancedBuilder);

			/*
			 * After building, we refresh all assets
			 */
			GUIUtils.RefreshAssets();
			EditorUtility.ClearProgressBar();
		}

		public static string[] GetActiveScenePathArray()
		{
			return EditorBuildSettings.scenes.Where(x => x.enabled).Select(x => x.path).ToArray();
		}
	}
}
