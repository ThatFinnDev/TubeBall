using UnityEngine;
using UnityEngine.UI;

public class CheckEntry : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private Button resetButton;
    [SerializeField] private string saveString;
    [SerializeField] private bool defaultValue;
    public void Start()
    {
        toggle.isOn = PlayerPrefs.GetInt("prefs." + saveString, defaultValue ? 1 : 0) == 1;
        resetButton.gameObject.SetActive(toggle.isOn!=defaultValue);
            
    }

    public void Reset()
    {
        toggle.isOn = defaultValue;
    }

    public void OnChange(bool newNumber)
    {
        resetButton.gameObject.SetActive(toggle.isOn!=defaultValue);
        PlayerPrefs.SetInt("prefs." + saveString, newNumber ? 1 : 0);
        PreferenceManager.OnValueChange("prefs."+saveString,newNumber);
    }
}