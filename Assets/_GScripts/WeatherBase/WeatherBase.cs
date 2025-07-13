using UnityEngine;


public abstract class WeatherBase : MonoBehaviour
    {
        [SerializeField]
        public WeatherType weatherType;
        protected bool IsEnabled = false;
        
        protected abstract void StartWeather();
        protected abstract void StopWeather();
        public void EnableWeather()
        {
            if (!CanEnableWeather()) return;
            IsEnabled = true;
            StartWeather();
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
