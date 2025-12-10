using UnityEditor;

public class BuildScript {
    public static void PerformBuild() {
        // Setting kompatibilitas Android 12 – 16
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel35;
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.companyname.gamename");

        // Tambahkan semua scene di Build Settings
        string[] scenes = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < scenes.Length; i++) {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }

        // Path hasil build (GameCI butuh ini)
        BuildPipeline.BuildPlayer(
            scenes,
            "build/Android/app.apk",
            BuildTarget.Android,
            BuildOptions.None
        );
    }
}
