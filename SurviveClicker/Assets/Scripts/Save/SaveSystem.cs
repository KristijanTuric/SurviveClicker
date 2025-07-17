using System;
using UnityEngine;

/// <summary>
/// This class can be used globally to save and get these 3 values from <see cref="PlayerPrefs"/>
/// </summary>
[Serializable]
public static class SaveSystem
{
    private const string FloatKey = "floatKey";
    private const string IntKey = "intKey";
    private const string StringKey = "stringKey";

    public static void SaveFloatValue(float value)
    {
        PlayerPrefs.SetFloat(FloatKey, value);
        PlayerPrefs.Save();
    }

    public static void SaveIntValue(int value)
    {
        PlayerPrefs.SetInt(IntKey, value);
        PlayerPrefs.Save();
    }

    public static void SaveStringKey(string value)
    {
        PlayerPrefs.SetString(StringKey, value);
        PlayerPrefs.Save();
    }

    public static float GetFloatValue()
    {
        return PlayerPrefs.GetFloat(FloatKey);
    }
    
    public static int GetIntValue()
    {
        return PlayerPrefs.GetInt(IntKey);
    }
    
    public static string GetStringValue()
    {
        return PlayerPrefs.GetString(StringKey);
    }
}
