using UnityEngine;
using UnityEngine.Events;

public class DailyWeatherGenerator : MonoBehaviour
{
    [System.Serializable]
    public struct WeatherProbability
    {
        public SeasonalClock.Season season;
       public SeasonWeatherData weatherData;
    }

    [Header("Dependencies")]
    public SeasonalClock clock;

    [Header("Weather Probabilities By Season")]
    public WeatherProbability[] seasonalWeatherChances;

    [Header("Current Weather")]
    private WeatherType _todayWeather;
    public UnityEvent<WeatherType> onWeatherChanged;
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
    public enum WeatherType
    {
        Sunny, Cloudy, Rainy, Snow, Hail, Thunder,
        Windy, Fog, Smog, DustStorm
    }
    private int lastCheckedDay = -1;
    
    

    void Update()
    {
        if (clock.dayOfYear != lastCheckedDay)
        {
            GenerateDailyWeather();
            lastCheckedDay = clock.dayOfYear;
        }
    }

    void GenerateDailyWeather()
    {
        var season = clock.currentSeason;
        SeasonWeatherData probs = GetSeasonProbabilities(season).weatherData;

        float roll = Random.value;
        float sum = 0f;

        sum += probs.sunny;
        if (roll < sum) { todayWeather = WeatherType.Sunny; return; }

        sum += probs.cloudy;
        if (roll < sum) { todayWeather = WeatherType.Cloudy; return; }

        sum += probs.rainy;
        if (roll < sum) { todayWeather = WeatherType.Rainy; return; }

        sum += probs.snow;
        if (roll < sum) { todayWeather = WeatherType.Snow; return; }

        sum += probs.hail;
        if (roll < sum) { todayWeather = WeatherType.Hail; return; }

        sum += probs.thunder;
        if (roll < sum) { todayWeather = WeatherType.Thunder; return; }

        sum += probs.windy;
        if (roll < sum) { todayWeather = WeatherType.Windy; return; }

        sum += probs.fog;
        if (roll < sum) { todayWeather = WeatherType.Fog; return; }

        sum += probs.smog;
        if (roll < sum) { todayWeather = WeatherType.Smog; return; }

        sum += probs.dustStorm;
        if (roll < sum) { todayWeather = WeatherType.DustStorm; return; }

        // Fallback
        todayWeather = WeatherType.Sunny;
    }

    WeatherProbability GetSeasonProbabilities(SeasonalClock.Season season)
    {
        foreach (var p in seasonalWeatherChances)
        {
            if (p.season == season) return p;
        }
        Debug.LogWarning("No weather probabilities set for season: " + season);
        return new WeatherProbability(); // default all 0
    }
    [ContextMenu("Reset Weather")]
    public void ResetWeather()
    {
        todayWeather = WeatherType.DustStorm; 
        lastCheckedDay = clock.dayOfYear; 
    }
}
