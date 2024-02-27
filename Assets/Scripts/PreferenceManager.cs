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
        }
    }
}
