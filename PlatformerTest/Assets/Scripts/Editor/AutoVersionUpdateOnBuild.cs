using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor;

public class IncrementBuildNumber : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(BuildReport report)
    {
        string[] versionParts = PlayerSettings.bundleVersion.Split('.');

        int buildNumber;
        if (versionParts.Length != 3 || !int.TryParse(versionParts[2], out buildNumber))
        {
            Debug.LogError("BuildPostprocessor failed to update version " + PlayerSettings.bundleVersion);

            return;
        }

        versionParts[2] = (buildNumber + 1).ToString();

        PlayerSettings.bundleVersion = versionParts[0] + '.' + versionParts[1] + '.' + versionParts[2];
    }
}