using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using PrimeTween;

public class DustStorm : MonoBehaviour
{
    public DailyWeatherGenerator dailyWeatherGenerator;
    public DailyWeatherGenerator.WeatherType weatherType = DailyWeatherGenerator.WeatherType.DustStorm;
    public Volume volume;
    private VolumeProfile _globalVolumeProfile;
    private Fog _globalFog;
    private FogSettings _cachedGlobalFog;
    
    [SerializeField] private VolumeProfile fogVolume;
    [SerializeField] private float fadeDuration = 1f;
    
    [SerializeField] private LocalVolumetricFog localFog;
    private Fog _fog;
    private FogSettings fog
    {
        get
        {
            if (fogVolume != null)
            {
                fogVolume.TryGet(out _fog);
            }
            return new FogSettings(_fog);
        }
    }

    private struct FogSettings
    {
        private bool state;
        bool active;
        float meanFreePath;
        float baseHeight;
        float maximumHeight;
        bool enableVolumetricFog;
        private FogColorParameter colorMode;
        ColorParameter TintColor;
        float mipFogMaxMipLevel;
        private Color albedo;
        private float volumetricFogDistance;
        float anisotropy;
        private float multipleScatteringIntensity;
        public FogSettings(Fog fog)
        {
            fog.SetAllOverridesTo(true);
            state = fog.enabled.value;
            active = fog.active;
            meanFreePath = fog.meanFreePath.value;
            baseHeight = fog.baseHeight.value;
            maximumHeight = fog.maximumHeight.value;
            enableVolumetricFog = fog.enableVolumetricFog.value;
            colorMode = fog.colorMode;
            TintColor = fog.tint;
            mipFogMaxMipLevel = fog.mipFogMaxMip.value;
            albedo = fog.albedo.value;
            volumetricFogDistance = fog.depthExtent.value;
            anisotropy = fog.anisotropy.value;
            multipleScatteringIntensity = fog.multipleScatteringIntensity.value;
        }
        public void ApplyTo(Fog fog)
        {
            fog.enabled.value = state;
            fog.active = active;
            fog.meanFreePath.value = meanFreePath;
            fog.baseHeight.value = baseHeight;
            fog.maximumHeight.value = maximumHeight;
            fog.tint = TintColor;
            fog.mipFogMaxMip.value = mipFogMaxMipLevel;
            fog.enableVolumetricFog.value = enableVolumetricFog;
            fog.albedo.value = albedo;
            fog.depthExtent.value = volumetricFogDistance;
            fog.anisotropy.value = anisotropy;
            fog.multipleScatteringIntensity.value = multipleScatteringIntensity;
        }
        public void LerpFogSettings(Fog fog, float duration)
        {
            fog.enabled.value = state;
            fog.active = active;
            fog.enableVolumetricFog.value = enableVolumetricFog;
            Tween.Custom(fog.meanFreePath.value, meanFreePath, duration, newVal => fog.meanFreePath.value = newVal);
            Tween.Custom(fog.baseHeight.value, baseHeight, duration, newVal => fog.baseHeight.value = newVal);
            Tween.Custom(fog.maximumHeight.value, maximumHeight, duration, newVal => fog.maximumHeight.value = newVal);
            Tween.Custom(fog.tint.value, TintColor.value, duration, newVal => fog.tint.value = newVal);
            Tween.Custom(fog.anisotropy.value, anisotropy, duration, newVal => fog.anisotropy.value = newVal);
            Tween.Custom(fog.multipleScatteringIntensity.value, multipleScatteringIntensity, duration, newVal => fog.multipleScatteringIntensity.value = newVal);
            Tween.Custom(fog.albedo.value, albedo, duration,newVal => fog.albedo.value = newVal);
            Tween.Custom(fog.depthExtent.value,volumetricFogDistance,duration, newVal => fog.depthExtent.value = newVal);
            Tween.Custom(fog.mipFogMaxMip.value, mipFogMaxMipLevel, duration, newVal => fog.mipFogMaxMip.value = newVal);
            // Non-float or non-tweenable properties should be set immediately or handled differently
            
        }
    }
    
    private bool _isEnabled = false;
    private void Start()
    {
        dailyWeatherGenerator = dailyWeatherGenerator?dailyWeatherGenerator: FindFirstObjectByType<DailyWeatherGenerator>();
        _globalVolumeProfile = volume ? volume.sharedProfile : FindFirstObjectByType<Volume>().sharedProfile;
    }
    private void OnEnable()
    {
        dailyWeatherGenerator.onWeatherChanged.AddListener(OnWeatherChanged);
    }
    private void OnDisable()
    {
        dailyWeatherGenerator.onWeatherChanged.RemoveListener(OnWeatherChanged);
        _cachedGlobalFog.ApplyTo(_globalFog);
    }

    private void OnWeatherChanged(DailyWeatherGenerator.WeatherType arg0)
    {
        if (arg0 == weatherType)
        {
            EnableDustStorm();
        }
        else
        {
            DisableDustStorm();
        }
    }

    private void DisableDustStorm()
    {
        if (!_isEnabled) return;
        _isEnabled = false;
        _cachedGlobalFog.LerpFogSettings(_globalFog,fadeDuration);
    }
   
    private void EnableDustStorm()
    {
        if(_isEnabled) return;
        _isEnabled = true;
        if (!_globalVolumeProfile.TryGet(out _globalFog))
            _globalFog =_globalVolumeProfile.Add<Fog>();
        _cachedGlobalFog = new FogSettings(_globalFog);
        fog.LerpFogSettings(_globalFog,fadeDuration);
    }
}
