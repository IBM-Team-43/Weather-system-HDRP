using UnityEngine;

namespace m_Devansh.WeatherBase
{
    public abstract class WeatherBase : MonoBehaviour
    {
        private WeatherType weatherType;
        private bool _isEnabled;

        protected virtual void OnWeatherChanged(WeatherType arg0)
        {
            if (arg0 == weatherType)
            {
                EnableWeather();
            }
            else
            {
                DisableWeather();
            }
        }
        protected virtual void DisableWeather()
        {
            if (!_isEnabled) return;
            _isEnabled = false;
        }
        protected virtual void EnableWeather()
        {
            if(_isEnabled) return;
        }
    }
    
}

public enum WeatherType
{
    Sunny, Cloudy, Rainy, Snow, Hail, Thunder,
    Windy, Fog, Smog, DustStorm,Others
}
