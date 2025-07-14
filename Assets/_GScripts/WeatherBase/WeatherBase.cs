using m_Devansh._Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Serialization;


public abstract class WeatherBase : MonoBehaviour
    {
        [SerializeField]
        public WeatherType weatherType;
        protected bool IsEnabled = false;
        public VolumetricClouds.CloudPresets cloudPreset = VolumetricClouds.CloudPresets.Cloudy;
        
        public UnityEvent onWeatherEnabled;
        public UnityEvent onWeatherDisabled;
        protected abstract void StartWeather();
        protected abstract void StopWeather();
        public void EnableWeather()
        {
            if (!CanEnableWeather()) return;
            IsEnabled = true;
            StartWeather();
            onWeatherEnabled?.Invoke();
        }
        private bool CanEnableWeather()
        {
            return !IsEnabled;
        }
        public void DisableWeather()
        {
            if (!CanDisableWeather()) return; 
            IsEnabled = false;
            StopWeather();
            onWeatherDisabled?.Invoke();
        }
        private bool CanDisableWeather()
        {
            return IsEnabled;
        }
    }
    


public enum WeatherType
{
    Sunny, Rainy, Snow, Thunder,
    Fog, DustStorm
}
