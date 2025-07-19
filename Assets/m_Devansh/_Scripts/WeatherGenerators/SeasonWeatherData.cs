using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SeasonWeatherData ", menuName = "Scriptable Objects/SeasonWeatherData ")]
public class SeasonWeatherData  : ScriptableObject
{
    public List<WeatherWeight> _weatherWeights = new();
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if(_weatherWeights.Count == Enum.GetValues(typeof(WeatherType)).Length)return;
        var existingWeights = new HashSet<WeatherType>();
        _weatherWeights.Clear();

        // Get all possible weather types (assuming WeatherType is an enum)
        foreach (WeatherType type in Enum.GetValues(typeof(WeatherType)))
        {
            float weight = 0f;
            _weatherWeights.Add(new WeatherWeight
            {
                type = type,
                weight = weight
            });
        }
    }
#endif
}

[System.Serializable]
public struct WeatherWeight
{
    public WeatherType type;
    [Range(0f, 1f)]
    public float weight;
}
