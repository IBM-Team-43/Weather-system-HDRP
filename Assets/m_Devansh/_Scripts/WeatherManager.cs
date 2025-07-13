using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace m_Devansh._Scripts
{
    public class WeatherManager : MonoBehaviour
    {
        private WeatherBase _currentWeather;
        public List<WeatherBase> weathers;
        public DailyWeatherGenerator dailyWeatherGenerator;
        private void Start()
        {
            dailyWeatherGenerator.onWeatherChanged.AddListener(SwitchWeather);
        }

        public void SwitchWeather(WeatherType weather)
        {
            if(_currentWeather && _currentWeather.weatherType != weather)
            {
                _currentWeather.DisableWeather();
                foreach (var w in weathers)
                {
                    if (w.weatherType == weather)
                        _currentWeather = w;
                }
                if(_currentWeather)
                {
                    _currentWeather.EnableWeather();
                }
            }
        }
        
    }
}
