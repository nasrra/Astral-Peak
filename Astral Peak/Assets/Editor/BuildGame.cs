using UnityEditor;
using UnityEngine;


public class BuildGame
{
    [MenuItem("Build/Build Game %#b")] // Ctrl+Shift+B (Windows) / Cmd+Shift+B (Mac)
    public static void Build()
    {
        // Get the project folder and move two levels up
        string projectFolder = Application.dataPath; // This is "<project>/Assets"
        string twoLevelsUp = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectFolder, "../../../")); // Two folders above the project

        // Set the build path to the "builds" folder in the two-levels-up directory
        string buildPath = System.IO.Path.Combine(twoLevelsUp, "Builds/AstralPeak.exe");

        // Perform the build
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, buildPath, BuildTarget.StandaloneWindows64, BuildOptions.None);

        // Display a confirmation dialog
        EditorUtility.DisplayDialog("Build Complete", $"Your game has been built at: {buildPath}", "OK");
    }
}

