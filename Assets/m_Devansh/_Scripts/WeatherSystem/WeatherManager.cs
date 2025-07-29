using System;
using System.Collections.Generic;
using nminhhoangit.SunCalculator;
using PrimeTween;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Serialization;

namespace m_Devansh._Scripts
{
    public class WeatherManager : MonoBehaviour
    {
        private WeatherBase _currentWeather;
        public List<WeatherBase> weathers;
        
        [Header("Environment")]
        public SunCalculator sunCalculator;
        public Vector2 windDirection;
        public Volume globalVolume;
        private VolumeProfile globalVolumeProfile=> globalVolume.sharedProfile;
        private VolumetricClouds _globalClouds;
        private void OnEnable()
        {
            //dailyWeatherGenerator.onWeatherChanged.AddListener(SwitchWeather);
            /*
             foreach (var weather in weathers)
            {
                weather.weatherManager = this;
            }
            */
        }
        public void SwitchWeather(WeatherType weather)
        {
            if(_currentWeather && _currentWeather.weatherType != weather)
                _currentWeather.DisableWeather();
            if (!_currentWeather || _currentWeather.weatherType != weather)
            {
                foreach (var w in weathers)
                {
                    if (w.weatherType == weather)
                    {
                        _currentWeather = w;
                        _currentWeather.EnableWeather();
                       ApplyCloudPresets(_currentWeather.cloudPreset);
                        Debug.Log("Switching weather to: " + weather);
                        break;
                    }
                }
                
            }
        }
        public void SetTime(DateTime time)
        {
            if (sunCalculator)
            {
                sunCalculator.UpdateDateTimeInputDatas( time);
            }
        }
        public void SetTime(DateTime time,Vector2 loc )
        {
            if (sunCalculator)
            {
                sunCalculator.UpdateInputDatas(loc.x,loc.y, time);
            }
        }
        private void OnDisable()
        {
           // dailyWeatherGenerator.onWeatherChanged.RemoveListener(SwitchWeather);
        }
        
        
        
        private void ApplyCloudPresets(VolumetricClouds.CloudPresets preset)
        {
            if (globalVolumeProfile.TryGet(out _globalClouds))
            {
                CloudSettings cloud = new CloudSettings(preset);
                cloud.LerpCloudSettings(_globalClouds, 1f);
            }
        }
        
        private struct CloudSettings
    {
        private readonly bool _isValid;
        private readonly bool _active;
        private readonly float densityMultiplier;
        private readonly float shapeFactor;
        private readonly float shapeScale;
        private readonly float erosionFactor;
        private readonly float erosionScale;
        public AnimationCurve densityCurve;
        public AnimationCurve erosionCurve;
        public AnimationCurve aoCurve;
        public float bottomAltitude;
        public float altitudeRange;
        public CloudSettings(VolumetricClouds cloud)
        {
            _isValid = true;
            cloud.SetAllOverridesTo(true);
            _active = cloud.active;
            densityMultiplier = cloud.densityMultiplier.value;
            shapeFactor = cloud.shapeFactor.value;
            shapeScale = cloud.shapeScale.value;
            erosionFactor = cloud.erosionFactor.value;
            erosionScale = cloud.erosionScale.value;
            densityCurve = cloud.densityCurve.value;
            erosionCurve = cloud.erosionCurve.value;
            aoCurve = cloud.ambientOcclusionCurve.value;
            bottomAltitude = cloud.bottomAltitude.value;
            altitudeRange = cloud.altitudeRange.value;
        }
        public CloudSettings(VolumetricClouds.CloudPresets preset)
        {
            _isValid = true;
            _active = true;
            bottomAltitude = 0;
            altitudeRange = 0;
            switch (preset)
        {
            case VolumetricClouds.CloudPresets.Sparse:
                densityMultiplier = 0.4f;
                    shapeFactor = 0.95f; shapeScale = 5f;
                    erosionFactor = 0.8f; erosionScale = 107f;
                densityCurve = new AnimationCurve(
                    new Keyframe(0f,0f), new Keyframe(.05f,1f),
                    new Keyframe(.75f,1f), new Keyframe(1f,0f));
                erosionCurve = new AnimationCurve(
                    new Keyframe(0f,1f), new Keyframe(.1f,.9f),
                    new Keyframe(1f,1f));
                aoCurve = new AnimationCurve(
                    new Keyframe(0f,0f), new Keyframe(.25f,.5f),
                    new Keyframe(1f,0f));
                bottomAltitude = 3000f; altitudeRange = 1000f;
                break;
            case VolumetricClouds.CloudPresets.Overcast:
                densityMultiplier = 0.3f;
                    shapeFactor = 0.5f;  shapeScale = 5f;
                    erosionFactor = 0.5f; erosionScale = 107f;
                densityCurve = new AnimationCurve(
                    new Keyframe(0f,0f), new Keyframe(.05f,1f),
                    new Keyframe(.9f,0f), new Keyframe(1f,0f));
                erosionCurve = new AnimationCurve(
                    new Keyframe(0f,1f), new Keyframe(.1f,.9f),
                    new Keyframe(1f,1f));
                aoCurve = new AnimationCurve(
                    new Keyframe(0f,0f), new Keyframe(1f,0f));
                bottomAltitude = 1500f; altitudeRange = 2500f;
                break;
            case VolumetricClouds.CloudPresets.Stormy:
                densityMultiplier = 0.35f;
                shapeFactor = 0.85f; shapeScale = 5f;
                erosionFactor = 0.75f; erosionScale = 107f;
                
                densityCurve = new AnimationCurve(
                    new Keyframe(0f,0f), new Keyframe(.037f,1f),
                    new Keyframe(.6f,1f), new Keyframe(1f,0f));
                erosionCurve = new AnimationCurve(
                    new Keyframe(0f,1f), new Keyframe(.05f,.8f),
                    new Keyframe(.2438f,.9498f), new Keyframe(.5f,1f),
                    new Keyframe(.93f,.9268f), new Keyframe(1f,1f));
                aoCurve = new AnimationCurve(
                    new Keyframe(0f,0f), new Keyframe(.1f,.4f),
                    new Keyframe(1f,0f));
                bottomAltitude = 1000.0f;
                altitudeRange = 5000.0f;
                break;
            default: 
                densityMultiplier = 0.4f;
                
                shapeFactor = 0.9f;   shapeScale = 5f;
                erosionFactor = 0.8f; erosionScale = 107f;
                
                densityCurve = new AnimationCurve(
                    new Keyframe(0f,0f), new Keyframe(.15f,1f),
                    new Keyframe(1f,.1f));
                erosionCurve = new AnimationCurve(
                    new Keyframe(0f,1f), new Keyframe(.1f,.9f),
                    new Keyframe(1f,1f));
                aoCurve = new AnimationCurve(
                    new Keyframe(0f,0f), new Keyframe(.25f,.4f),
                    new Keyframe(1f,0f));
                bottomAltitude = 1200f; 
                altitudeRange = 2000f;
                break;
        }
        }
        public void ApplyTo(VolumetricClouds cloud)
        {
            if (!_isValid) return;
            cloud.active = _active;
            cloud.densityMultiplier.value = densityMultiplier;
            cloud.shapeFactor.value = shapeFactor;
            cloud.shapeScale.value = shapeScale;
            cloud.erosionFactor.value = erosionFactor;
            cloud.erosionScale.value = erosionScale;
        }
        public void LerpCloudSettings(VolumetricClouds cloud, float duration)
        {
            cloud.active = _active;
            Tween.Custom(cloud.densityMultiplier.value, densityMultiplier, duration, newVal => cloud.densityMultiplier.value = newVal);
            Tween.Custom(cloud.shapeFactor.value, shapeFactor, duration, newVal => cloud.shapeFactor.value = newVal);
            Tween.Custom(cloud.shapeScale.value, shapeScale, duration, newVal => cloud.shapeScale.value = newVal);
            Tween.Custom(cloud.erosionFactor.value, erosionFactor, duration, newVal => cloud.erosionFactor.value = newVal);
            Tween.Custom(cloud.erosionScale.value, erosionScale, duration, newVal => cloud.erosionScale.value = newVal);
            Tween.Custom(cloud.bottomAltitude.value, bottomAltitude, duration, newVal => cloud.bottomAltitude.value = newVal);
            Tween.Custom(cloud.altitudeRange.value, altitudeRange, duration, newVal => cloud.altitudeRange.value = newVal);
            
            
            cloud.densityCurve.value = densityCurve;
            cloud.erosionCurve.value = erosionCurve;
            cloud.ambientOcclusionCurve.value = aoCurve;
            // Non-float or non-tweenable properties should be set immediately or handled differently
            
        }
    }
    }
}
