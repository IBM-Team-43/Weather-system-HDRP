using UnityEngine;

[CreateAssetMenu(fileName = "SeasonWeatherData ", menuName = "Scriptable Objects/SeasonWeatherData ")]
public class SeasonWeatherData  : ScriptableObject
{

    [Range(0, 1)] public float sunny;
    [Range(0, 1)] public float cloudy;
    [Range(0, 1)] public float rainy;
    [Range(0, 1)] public float snow;
    [Range(0, 1)] public float hail;
    [Range(0, 1)] public float thunder;
    [Range(0, 1)] public float windy;
    [Range(0, 1)] public float fog;
    [Range(0, 1)] public float smog;
    [Range(0, 1)] public float dustStorm;
}
