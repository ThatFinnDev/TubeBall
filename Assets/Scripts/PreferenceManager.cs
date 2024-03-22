using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public static class PreferenceManager
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void AfterSceneLoad()
    {
        //Application.targetFrameRate = 60;
    }

    private static bool isInLandscape = true;
    public static void OnOrientationChange(ScreenOrientation orientation) 
    {
        try
        {
            switch (orientation)
            {
                case ScreenOrientation.LandscapeLeft:
                case ScreenOrientation.LandscapeRight:
                    isInLandscape = true;
                    GameController.instance.camera.fieldOfView = PlayerPrefs.GetFloat("prefs.fov", 60);
                    break;
                case ScreenOrientation.Portrait:
                case ScreenOrientation.PortraitUpsideDown:
                    isInLandscape = false;
                    GameController.instance.camera.fieldOfView = PlayerPrefs.GetFloat("prefs.fovVertical", 110);
                    break;
            }
        } catch {}
    }
     
    public static void OnValueChange(string saveString, object value)
    {
        switch (saveString)
        {
            case "prefs.fov":
                if (value is int) { if (isInLandscape) GameController.instance.camera.fieldOfView = float.Parse(value + ".0"); }
                else if (value is float) { if (isInLandscape) GameController.instance.camera.fieldOfView = float.Parse(value.ToString()); }
                break;
            case "prefs.fovVertical":
                if (value is int) { if (!isInLandscape) GameController.instance.camera.fieldOfView = float.Parse(value + ".0"); }
                else if (value is float) { if (!isInLandscape) GameController.instance.camera.fieldOfView = float.Parse(value.ToString()); }
                break;
            case "prefs.bloom":
                if (value is bool)
                {
                    List<VolumeComponent> components = GameController.instance.GetComponent<Volume>().profile.components;
                    foreach (VolumeComponent profile in components)
                    {
                        if (profile is Bloom)
                        {
                            Bloom bloom = profile as Bloom;
                            bloom.intensity.value = bool.Parse(value.ToString()) ? 3.96f : 0f;
                        }
                    }
                }
                break;
            case "prefs.contrast":
                if (value is int)
                {
                    List<VolumeComponent> components = GameController.instance.GetComponent<Volume>().profile.components;
                    foreach (VolumeComponent profile in components)
                    {
                        if (profile is ColorAdjustments)
                        {
                            ColorAdjustments adjustments = profile as ColorAdjustments;
                            adjustments.contrast.value=float.Parse(value+".0");
                        }
                    }
                }
                else if (value is float)
                {
                    List<VolumeComponent> components = GameController.instance.GetComponent<Volume>().profile.components;
                    foreach (VolumeComponent profile in components)
                    {
                        if (profile is ColorAdjustments)
                        {
                            ColorAdjustments adjustments = profile as ColorAdjustments;
                            adjustments.contrast.value=float.Parse(value.ToString());
                        }
                    }
                }
                break;
            case "prefs.saturation":
                if (value is int)
                {
                    List<VolumeComponent> components = GameController.instance.GetComponent<Volume>().profile.components;
                    foreach (VolumeComponent profile in components)
                    {
                        if (profile is ColorAdjustments)
                        {
                            ColorAdjustments adjustments = profile as ColorAdjustments;
                            adjustments.saturation.value=float.Parse(value+".0");
                        }
                    }
                }
                else if (value is float)
                {
                    List<VolumeComponent> components = GameController.instance.GetComponent<Volume>().profile.components;
                    foreach (VolumeComponent profile in components)
                    {
                        if (profile is ColorAdjustments)
                        {
                            ColorAdjustments adjustments = profile as ColorAdjustments;
                            adjustments.saturation.value=float.Parse(value.ToString());
                        }
                    }
                }
                break;
        }
    }

    public static bool shouldSave = true;
    public static bool freeShopping = false;

    public static void CorrectTotalCoins()
    {
        try
        {
            GameController.totalCoins = GameController.coins;
            GameController.totalCoins += CosmeticsManager.instance.GetComponent<CosmeticsManager>().GetSpendCoins();
        }
        catch { }
    }
    public static void LoadSaveData()
    {
        try
        {
            string potantialXML = GUIUtility.systemCopyBuffer;
            XElement.Parse(potantialXML);
            SetSavaData(potantialXML);
            shouldSave = false;
            Application.Quit();
        }
        catch { }
    }
    public static void ExportSaveData()
    {
        GUIUtility.systemCopyBuffer = GetSavaData();
    }
    static string GetSavaData()
    {
        PlayerPrefs.Save();
        return File.ReadAllText("/data/data/"+Application.identifier+"/shared_prefs/"+Application.identifier+".v2.playerprefs.xml");
    }
    
    static void SetSavaData(string saveData)
    {
        File.WriteAllText("/data/data/"+Application.identifier+"/shared_prefs/"+Application.identifier+".v2.playerprefs.xml",saveData);
    }
}
