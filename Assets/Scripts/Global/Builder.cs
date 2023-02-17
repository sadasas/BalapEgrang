using System.Linq;
using UnityEditor;

#if UNITY_EDITOR
public static class Builder
{
    public static void BuildProject()
    {
        var options = new BuildPlayerOptions
        {
            scenes = EditorBuildSettings.scenes
             .Where(scene => scene.enabled)
             .Select(scene => scene.path)
             .ToArray(),

            target = BuildTarget.Android,
            locationPathName = "E:/un/build/BalapEgrang",
        };

        BuildPipeline.BuildPlayer(options);
    }
}
#endif
