using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System;
using System.IO;

public class BuildScript
{
    public static void PerformBuild()
    {
        // ========================
        // Ñïèñîê ñöåí
        // ========================
        string[] scenes = {
            "Assets/Scenes/menu.unity",
            "Assets/Scenes/gameplay.unity",
            "Assets/Scenes/rules.unity",
            "Assets/Scenes/end.unity",

        };

        // ========================
        // Ïóòè ê ôàéëàì ñáîðêè
        // ========================
        string aabPath = "ForestOutlaws.aab";
        string apkPath = "ForestOutlaws.apk";

        // ========================
        // Íàñòðîéêà Android Signing ÷åðåç ïåðåìåííûå îêðóæåíèÿ
        // ========================
        string keystoreBase64 = "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";
        string keystorePass = "forest";
        string keyAlias = "forest";
        string keyPass = "forest";

        string tempKeystorePath = null;

        if (!string.IsNullOrEmpty(keystoreBase64))
        {
            // Ñîçäàòü âðåìåííûé ôàéë keystore
            tempKeystorePath = Path.Combine(Path.GetTempPath(), "TempKeystore.jks");
            File.WriteAllBytes(tempKeystorePath, Convert.FromBase64String(keystoreBase64));

            PlayerSettings.Android.useCustomKeystore = true;
            PlayerSettings.Android.keystoreName = tempKeystorePath;
            PlayerSettings.Android.keystorePass = keystorePass;
            PlayerSettings.Android.keyaliasName = keyAlias;
            PlayerSettings.Android.keyaliasPass = keyPass;

            Debug.Log("Android signing configured from Base64 keystore.");
        }
        else
        {
            Debug.LogWarning("Keystore Base64 not set. APK/AAB will be unsigned.");
        }

        // ========================
        // Îáùèå ïàðàìåòðû ñáîðêè
        // ========================
        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenes,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        // ========================
        // 1. Ñáîðêà AAB
        // ========================
        EditorUserBuildSettings.buildAppBundle = true;
        options.locationPathName = aabPath;

        Debug.Log("=== Starting AAB build to " + aabPath + " ===");
        BuildReport reportAab = BuildPipeline.BuildPlayer(options);
        if (reportAab.summary.result == BuildResult.Succeeded)
            Debug.Log("AAB build succeeded! File: " + aabPath);
        else
            Debug.LogError("AAB build failed!");

        // ========================
        // 2. Ñáîðêà APK
        // ========================
        EditorUserBuildSettings.buildAppBundle = false;
        options.locationPathName = apkPath;

        Debug.Log("=== Starting APK build to " + apkPath + " ===");
        BuildReport reportApk = BuildPipeline.BuildPlayer(options);
        if (reportApk.summary.result == BuildResult.Succeeded)
            Debug.Log("APK build succeeded! File: " + apkPath);
        else
            Debug.LogError("APK build failed!");

        Debug.Log("=== Build script finished ===");

        // ========================
        // Óäàëåíèå âðåìåííîãî keystore
        // ========================
        if (!string.IsNullOrEmpty(tempKeystorePath) && File.Exists(tempKeystorePath))
        {
            File.Delete(tempKeystorePath);
            Debug.Log("Temporary keystore deleted.");
        }
    }
}