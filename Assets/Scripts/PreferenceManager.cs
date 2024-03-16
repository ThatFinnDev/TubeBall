using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public static class PreferenceManager
{
    public static void OnValueChange(string saveString, object value)
    {
        switch (saveString)
        {
            case "prefs.fov":
                if (value is int) GameController.instance.camera.fieldOfView = float.Parse(value+".0");
                else if (value is float) GameController.instance.camera.fieldOfView = float.Parse(value.ToString());
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
}
