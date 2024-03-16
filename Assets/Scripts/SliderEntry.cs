using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderEntry : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Button resetButton;
    [SerializeField] private string saveString;
    [SerializeField] private float defaultValue;
    public void Start()
    {
        if (slider.wholeNumbers) 
            slider.value = PlayerPrefs.GetInt("prefs."+saveString, int.Parse(defaultValue.ToString().Split(".")[0]));
        else
            slider.value = PlayerPrefs.GetFloat("prefs."+saveString, defaultValue);
        resetButton.gameObject.SetActive(slider.value!=defaultValue);
            
    }

    public void Reset()
    {
        slider.value = defaultValue;
    }

    public void OnChange(float newNumber)
    {
        resetButton.gameObject.SetActive(slider.value!=defaultValue);
        if (slider.wholeNumbers) PlayerPrefs.SetInt("prefs."+saveString, int.Parse(newNumber.ToString().Split(".")[0]));
        else PlayerPrefs.SetFloat("prefs."+saveString, newNumber);
        PreferenceManager.OnValueChange("prefs."+saveString,newNumber);
    }
}
