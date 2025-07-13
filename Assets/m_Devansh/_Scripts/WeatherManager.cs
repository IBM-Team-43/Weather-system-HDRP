using System;
using System.Collections.Generic;
using nminhhoangit.SunCalculator;
using UnityEngine;
using UnityEngine.Serialization;

namespace m_Devansh._Scripts
{
    public class WeatherManager : MonoBehaviour
    {
        private WeatherBase _currentWeather ;
        public List<WeatherBase> weathers;
        //public DailyWeatherGenerator dailyWeatherGenerator;
        
        [Header("Environment")]
        public Light sun;
        private SunCalculator _sunCalculator;

        private void OnEnable()
        {
            //dailyWeatherGenerator.onWeatherChanged.AddListener(SwitchWeather);
        }

        public void SwitchWeather(WeatherType weather)
        {
            
            if (!_currentWeather)
            {
                foreach (var w in weathers)
                {
                    if (w.weatherType == weather)
                    {
                        _currentWeather = w;
                        _currentWeather.EnableWeather();
                    }
                }
            }
            else if (_currentWeather.weatherType != weather)
            {
                _currentWeather.DisableWeather();

                foreach (var w in weathers)
                {
                    if (w.weatherType == weather)
                    {
                        _currentWeather = w;
                        _currentWeather.EnableWeather();
                        Debug.Log("Switching weather to: " + weather);
                        return;
                    }
                }
            }
        }
        public void SetTime(DateTime time)
        {
            if (_sunCalculator)
            {
                _sunCalculator.UpdateDateTimeInputDatas(time);
            }
            else if(sun)
            {
                float hour = time.Hour + time.Minute / 60f + time.Second / 3600f;
                float sunAngle = (hour / 24f) * 360f;
                sun.transform.rotation = Quaternion.Euler(sunAngle, 0f, 0f);
            }
        }

        private void OnDisable()
        {
           // dailyWeatherGenerator.onWeatherChanged.RemoveListener(SwitchWeather);
        }
    }
}
