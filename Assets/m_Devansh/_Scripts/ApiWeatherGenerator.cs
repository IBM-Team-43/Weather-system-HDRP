using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

namespace m_Devansh._Scripts
{
    public class ApiWeatherGenerator : WeatherGenerator
    {
        [SerializeField]ApiKey apiKeySo;
        string apiKey=> apiKeySo.apikey;
        public string city = "Delhi";
        
        public bool useMyLocation = false;
        public float latitude = 28.6139f;
        public float longitude = 77.2090f;
        private string url => useMyLocation ? 
            $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={apiKey}" 
            : $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}";

        private async void Start()
        {
            if(useMyLocation)
            {
                var location = await FetchLocation();
                city = location.city;
                    latitude = location.lat;

                    longitude = location.lon;
            }

            WeatherType weather = await FetchWeather();
            onWeatherChanged.Invoke(weather);
        }

        private async Task<LocationInfo> FetchLocation()
        {
            string url = "https://ipinfo.io/json";
            UnityWebRequest www = UnityWebRequest.Get(url);
            await www.SendWebRequest();
            Debug.Log("location: ");
            LocationInfo location = new LocationInfo("somewhere",0,0);
            if (www.result == UnityWebRequest.Result.Success)
            {
                 location = JsonUtility.FromJson<LocationInfo>(www.downloadHandler.text);
                 
                 var parts = location.loc.Split(',');
                 if (parts.Length == 2)
                 {
                     float.TryParse(parts[0].Trim(), out location.lat);
                     float.TryParse(parts[1].Trim(), out location.lon);
                 }
                 Debug.Log(location.lat + ", " + location.lon);
            }
            return location;
        }

        [ContextMenu("Call OpenWeatherMap API")]
        private async Task<WeatherType> FetchWeather()
        {
            
            UnityWebRequest www = UnityWebRequest.Get(url);
            await www.SendWebRequest();
            WeatherType type = WeatherType.Sunny;
            if (www.result == UnityWebRequest.Result.Success)
            {
                WeatherData data = JsonUtility.FromJson<WeatherData>(www.downloadHandler.text);
                var main = data.weather[0].main;
                var desc = data.weather[0].description;
                var wind = data.wind.speed;

                type = MapToWeatherType(main, desc, wind);
                
            }
            else
            {
                Debug.LogError("Failed to fetch weather: " + www.error);
            }

            return type;
        }
        WeatherType MapToWeatherType(string main, string desc, float windSpeed)
        {
            main = main.ToLower();
            desc = desc.ToLower();

            if (main.Contains("clear"))
                return WeatherType.Sunny;
            if (main.Contains("cloud"))
                return WeatherType.Cloudy;
            if (main.Contains("rain") || main.Contains("drizzle"))
                return WeatherType.Rainy;
            if (main.Contains("thunderstorm"))
                return WeatherType.Thunder;
            if (main.Contains("snow"))
                return WeatherType.Snow;
            if (main.Contains("fog") || desc.Contains("mist") || desc.Contains("haze"))
                return WeatherType.Fog;
            if (main.Contains("dust") || desc.Contains("sand") || windSpeed > 15f)
                return WeatherType.DustStorm;

            return WeatherType.Sunny;
        }
        [Serializable]
        public class WeatherData
        {
            public WeatherInfo[] weather;
            public Wind wind;
        }

        [Serializable]
        public class WeatherInfo
        {
            public string main;
            public string description;
        }
        [Serializable]
        public class LocationInfo
        {
            public LocationInfo()
            {
            }
            public LocationInfo(string city, float lat, float lon)
            {
                this.city = city;
                this.lat = lat;
                this.lon = lon;
            }
            public string city;

            public float lat;

            public float lon;
            public string loc;
        }

    }
    [Serializable]
    public class Wind
    {
        public float speed;
    }
}
