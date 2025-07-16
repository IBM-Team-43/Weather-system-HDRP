using UnityEngine;

namespace m_Devansh._Scripts
{
    [CreateAssetMenu(fileName = "ApiKey", menuName = "Scriptable Objects/ApiKey", order = 0)]
    public class ApiKey : ScriptableObject
    {
        [Header("API Key")] [Tooltip("API Key for OpenWeatherMap or any other weather service.")]
        public string apikey;
    }
}
