using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using PrimeTween;

public class DustStorm : WeatherBase
{
    public Volume volume;
    private VolumeProfile _globalVolumeProfile;
    private Fog _globalFog;
    private FogSettings _cachedGlobalFog ;
    
    [SerializeField] private VolumeProfile fogVolume;
    [SerializeField] private float fadeDuration = 1f;
    
    [SerializeField] private LocalVolumetricFog localFog;
    private Fog _fog;
    private FogSettings fog;
    private struct FogSettings
    {
        private readonly bool _isValid;
        private readonly bool _state;
        private readonly bool _active;
        private readonly float _meanFreePath;
        private readonly float _baseHeight;
        private readonly float _maximumHeight;
        private readonly bool _enableVolumetricFog;
        private FogColorParameter _colorMode;
        private readonly ColorParameter _tintColor;
        private readonly float _mipFogMaxMipLevel;
        private readonly Color _albedo;
        private readonly float _volumetricFogDistance;
        private readonly float _anisotropy;
        private readonly float _multipleScatteringIntensity;
        public FogSettings(Fog fog)
        {
            _isValid = true;
            fog.SetAllOverridesTo(true);
            _state = fog.enabled.value;
            _active = fog.active;
            _meanFreePath = fog.meanFreePath.value;
            _baseHeight = fog.baseHeight.value;
            _maximumHeight = fog.maximumHeight.value;
            _enableVolumetricFog = fog.enableVolumetricFog.value;
            _colorMode = fog.colorMode;
            _tintColor = fog.tint;
            _mipFogMaxMipLevel = fog.mipFogMaxMip.value;
            _albedo = fog.albedo.value;
            _volumetricFogDistance = fog.depthExtent.value;
            _anisotropy = fog.anisotropy.value;
            _multipleScatteringIntensity = fog.multipleScatteringIntensity.value;
        }
        public void ApplyTo(Fog fog)
        {
            if (!_isValid) return;
            fog.enabled.value = _state;
            fog.active = _active;
            fog.meanFreePath.value = _meanFreePath;
            fog.baseHeight.value = _baseHeight;
            fog.maximumHeight.value = _maximumHeight;
            fog.tint = _tintColor;
            fog.mipFogMaxMip.value = _mipFogMaxMipLevel;
            fog.enableVolumetricFog.value = _enableVolumetricFog;
            fog.albedo.value = _albedo;
            fog.depthExtent.value = _volumetricFogDistance;
            fog.anisotropy.value = _anisotropy;
            fog.multipleScatteringIntensity.value = _multipleScatteringIntensity;
        }
        public void LerpFogSettings(Fog fog, float duration)
        {
            fog.enabled.value = _state;
            fog.active = _active;
            fog.enableVolumetricFog.value = _enableVolumetricFog;
            Tween.Custom(fog.meanFreePath.value, _meanFreePath, duration, newVal => fog.meanFreePath.value = newVal);
            Tween.Custom(fog.baseHeight.value, _baseHeight, duration, newVal => fog.baseHeight.value = newVal);
            Tween.Custom(fog.maximumHeight.value, _maximumHeight, duration, newVal => fog.maximumHeight.value = newVal);
            Tween.Custom(fog.tint.value, _tintColor.value, duration, newVal => fog.tint.value = newVal);
            Tween.Custom(fog.anisotropy.value, _anisotropy, duration, newVal => fog.anisotropy.value = newVal);
            Tween.Custom(fog.multipleScatteringIntensity.value, _multipleScatteringIntensity, duration, newVal => fog.multipleScatteringIntensity.value = newVal);
            Tween.Custom(fog.albedo.value, _albedo, duration,newVal => fog.albedo.value = newVal);
            Tween.Custom(fog.depthExtent.value,_volumetricFogDistance,duration, newVal => fog.depthExtent.value = newVal);
            Tween.Custom(fog.mipFogMaxMip.value, _mipFogMaxMipLevel, duration, newVal => fog.mipFogMaxMip.value = newVal);
            // Non-float or non-tweenable properties should be set immediately or handled differently
            
        }
    }
    protected void Start()
    {
        _globalVolumeProfile = volume ? volume.sharedProfile : FindFirstObjectByType<Volume>().sharedProfile;
    }
    protected void OnDisable()
    {
        _cachedGlobalFog.ApplyTo(_globalFog);
    }
    protected override void StartWeather()
    {
        if (!_globalVolumeProfile.TryGet(out _globalFog))
            _globalFog =_globalVolumeProfile.Add<Fog>();
        _cachedGlobalFog = new FogSettings(_globalFog);
        if (fogVolume != null)
        {
            fogVolume.TryGet(out _fog);
        }
        fog = new FogSettings(_fog);
        fog.LerpFogSettings(_globalFog,fadeDuration);
    }
    protected override void StopWeather()
    {
        _cachedGlobalFog.LerpFogSettings(_globalFog, fadeDuration);
    }
}
