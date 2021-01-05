﻿using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class PreBuildVersioning : IPreprocessBuildWithReport
{
    private const string toolName = "i5 Build Versioning Tool";
    private const string placeholder = "$git";

    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        if (!PlayerSettings.bundleVersion.ToLower().Contains(placeholder))
        {
            Debug.Log($"[{toolName}] Version placeholder not found. To use automatic semantic versioning with Git, write the placeholder {placeholder} into the application's version");
            return;
        }


        Debug.Log($"[{toolName}] Version placeholder found. Running versioning tool to calculate semantic version number from Git tags");
        if (!GitVersion.TryGetVersion(out string version))
        {
            Debug.LogWarning($"[{toolName}] Could not get version name. Version placeholder will be replaced with default {version}");
        }

         PlayerSettings.bundleVersion.Replace(placeholder, version);
    }
}