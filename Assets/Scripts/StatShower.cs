using TMPro;
using UnityEngine;

public class StatShower : MonoBehaviour
{
    public string key;
    public string readableName = "";
    private void OnEnable()
    { GetComponent<TextMeshProUGUI>().text = "  "+readableName+": "+PlayerPrefs.GetString(key,"0"); }
    private void Awake()
    { OnEnable(); }
}
