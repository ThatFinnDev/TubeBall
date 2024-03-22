using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Application = UnityEngine.Device.Application;
using Debug = UnityEngine.Debug;

public class EditorMenu : MonoBehaviour
{
    [MenuItem("Build Player/Android Release Build")]
    public static void BuildAndroidRelease()
    {
        if (!File.Exists(releaseExportPath))
        {
            Debug.LogError("Specify valid Export path!");
            return;
        }
        string[] scenes = new string[EditorBuildSettings.scenes.Length - 1];
        for (int i = 0; i < EditorBuildSettings.scenes.Length - 1; i++)
            scenes[i] = EditorBuildSettings.scenes[i].path;
        BuildPlayerOptions options = new BuildPlayerOptions();
        options.scenes = scenes;
        options.target = BuildTarget.Android;
        options.locationPathName = File.ReadAllText(releaseExportPath).Replace("\n","");
        options.targetGroup = BuildTargetGroup.Android;
        options.options = BuildOptions.None;
        BuildPipeline.BuildPlayer(options);
    }

    public static string lastdevBundleVerPath = Application.dataPath.Replace("/Assets", "/lastdevbundleVer");
    public static string devExportPath = Application.dataPath.Replace("/Assets", "/devExportPath");
    public static string devScriptOnExport = Application.dataPath.Replace("/Assets", "/devScriptOnExport.sh");
    public static string releaseExportPath = Application.dataPath.Replace("/Assets", "/releaseExportPath");
    [MenuItem("Build Player/Android Development Build")]
    public static void BuildAndroidDevPlayer()
    {
        if (!File.Exists(devExportPath))
        {
            Debug.LogError("Specify valid Export path!");
            return;
        }
        PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, new Texture2D[] {Resources.Load<Texture2D>("icon.dev")});
        string[] scenes = new string[EditorBuildSettings.scenes.Length - 1];
        for (int i = 0; i < EditorBuildSettings.scenes.Length - 1; i++)
            scenes[i] = EditorBuildSettings.scenes[i].path;
        EditorUserBuildSettings.buildAppBundle = false;
        PlayerSettings.Android.minifyDebug = true;
        BuildPlayerOptions options = new BuildPlayerOptions();
        string productName = PlayerSettings.productName;
        string identifier = PlayerSettings.applicationIdentifier;
        string bundleVersion = PlayerSettings.bundleVersion;
        int versionCode = PlayerSettings.Android.bundleVersionCode;
        PlayerSettings.productName = "Tubeball Dev";
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android,"at.Fnimj.Tubeball.dev");
        if(!File.Exists(lastdevBundleVerPath)) File.WriteAllText(lastdevBundleVerPath,2.ToString());
        PlayerSettings.Android.bundleVersionCode = int.Parse(File.ReadAllText(lastdevBundleVerPath))+1;
        File.WriteAllText(lastdevBundleVerPath,PlayerSettings.Android.bundleVersionCode.ToString());
        PlayerSettings.bundleVersion = "dev."+PlayerSettings.Android.bundleVersionCode.ToString();
        string exportPath = File.ReadAllText(devExportPath).Replace("$versioncode$", PlayerSettings.Android.bundleVersionCode.ToString()).Replace("\n","");
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;
        options.scenes = scenes;
        options.target = BuildTarget.Android;
        options.locationPathName = exportPath;
        options.targetGroup = BuildTargetGroup.Android;
        options.options = BuildOptions.Development;
        BuildPipeline.BuildPlayer(options);
        //RESET
        EditorUserBuildSettings.buildAppBundle = true;
        PlayerSettings.Android.bundleVersionCode = versionCode;
        PlayerSettings.Android.minifyDebug = false;
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;
        PlayerSettings.productName = productName;
        PlayerSettings.bundleVersion = bundleVersion;
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android,identifier);
        PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, new Texture2D[] {Resources.Load<Texture2D>("icon")});
        if (File.Exists(exportPath))
        {
            if (File.Exists(exportPath.Replace(".apk", "_mapping.txt"))) File.Delete(exportPath.Replace(".apk", "_mapping.txt"));            
        }
        else
        { 
            File.WriteAllText(lastdevBundleVerPath, (int.Parse(File.ReadAllText(lastdevBundleVerPath)) - 1).ToString());
        }
    }
}
