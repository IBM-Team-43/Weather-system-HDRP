using UnityEngine;
using UnityEngine.Events;

namespace m_Devansh._Scripts
{
    public abstract class WeatherGenerator : MonoBehaviour
    {
        public UnityEvent<WeatherType> onWeatherChanged;
    }
}
