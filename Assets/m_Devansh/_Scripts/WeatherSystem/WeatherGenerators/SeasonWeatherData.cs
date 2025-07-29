using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SeasonWeatherData ", menuName = "Scriptable Objects/SeasonWeatherData ")]
public class SeasonWeatherData  : ScriptableObject
{
    [FormerlySerializedAs("_weatherWeights")] public List<WeatherWeight> weatherWeights = new();
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if(weatherWeights.Count == Enum.GetValues(typeof(WeatherType)).Length)return;
        var existingWeights = new HashSet<WeatherType>();
        weatherWeights.Clear();

        // Get all possible weather types (assuming WeatherType is an enum)
        foreach (WeatherType type in Enum.GetValues(typeof(WeatherType)))
        {
            float weight = 0f;
            weatherWeights.Add(new WeatherWeight
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
