using m_Devansh._Scripts;
using UnityEngine;
using UnityEngine.Events;

public class DailyWeatherGenerator : WeatherGenerator
{
    [System.Serializable]
    public struct WeathersProbability
    {
        public SeasonalClock.Season season;
       public SeasonWeatherData weatherData;
    }
    [Header("Dependencies")]
    public SeasonalClock clock;
    [Header("Weather Probabilities By Season")]
    public WeathersProbability[] seasonalWeatherChances;

    [Header("Current Weather")]
    private WeatherType _todayWeather;
    public WeatherType todayWeather
    {
        get => _todayWeather;
        set
        {
            if (_todayWeather == value) return;
            _todayWeather = value;
            onWeatherChanged?.Invoke(_todayWeather);
        }
    }
    private int lastCheckedDay = -1;
    void Update()
    {
        if (clock.currentDayOfYear != lastCheckedDay)
        {
            lastCheckedDay = clock.currentDayOfYear;
            GenerateDailyWeather();
        }
    }
    void GenerateDailyWeather()
    {
        var season = clock.currentSeason;
        SeasonWeatherData probs = GetSeasonProbabilities(season).weatherData;
        float total = 0;
        foreach (var weight in probs._weatherWeights)
        {
            total+= weight.weight;
        }

        float roll = Random.Range(0f, total);
        float sum  = 0f;

        foreach (var entry in probs._weatherWeights)
        {
            sum += entry.weight;
            if (roll < sum)
            {
                todayWeather = entry.type;
                return;
            }
        }

        // Fallback
        todayWeather = WeatherType.Sunny;
    }
    WeathersProbability GetSeasonProbabilities(SeasonalClock.Season season)
    {
        foreach (var p in seasonalWeatherChances)
        {
            if (p.season == season) return p;
        }
        return new WeathersProbability(); // default all 0
    }
    [ContextMenu("Reset Weather")]
    public void ResetWeather()
    {
        todayWeather = WeatherType.DustStorm; 
        lastCheckedDay = clock.currentDayOfYear; 
    }
}
