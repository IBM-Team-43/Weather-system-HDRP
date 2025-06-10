using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RainController : MonoBehaviour
{
    [Header("Rain Settings")]
    public ParticleSystem rainParticleSystem;

    [SerializeField]
    [Range(-45, 145)]

    public int direction;

    [Range(10, 100)]
     public int minVelocity;
    [Range(20, 200)]
    public int maxVelocity;
    [Header("Emission Control")]
    public float maxEmissionRate = 10000f;
    public int maxParticles = 100000;

    public int _gravitymodifier = 2;
    [Header("Transition Settings")]
    public float transitionDuration = 3f;  // Time to fully start/stop rain
    
    
    [Header("Rain Sound")]
    public AudioSource rainAudioSource;    // Main rain sound
    public AudioClip rainSoundClip;        // Rain sound clip
    [Range(0f, 1f)]
    public float maxRainVolume = 0.7f;     // Maximum volume for rain sound
    public bool fadeWithIntensity = true;   // Whether sound volume follows rain intensity
    
    private bool isRaining = false;
    private bool isTransitioning = false;
    
    // Store original values
    private float originalEmissionRate;
    private int originalMaxParticles;

    private int[] originalSubMaxParticles;
    
    // Direction tracking
    private int previousDirection;
    private int previousminvalue;
    private int previousmaxvalue;

    
    private void Start()
    {
        // Ensure cursor is visible and unlocked
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Store original values
        if (rainParticleSystem != null)
        {
            var emission = rainParticleSystem.emission;
            originalEmissionRate = emission.rateOverTime.constant;
            originalMaxParticles = rainParticleSystem.main.maxParticles;

            // Set max values if not already set
            if (maxEmissionRate == 0) maxEmissionRate = originalEmissionRate;
            if (maxParticles == 0) maxParticles = originalMaxParticles;
        }

        // Store sub emitter original values
    
        // Setup rain audio
        SetupRainAudio();

        // Initialize direction
        previousminvalue = minVelocity;
        previousmaxvalue= maxVelocity;
        UpdateVelocity();
        previousDirection = direction;
        UpdateRainDirection();

        // Start with rain off
        StopRainImmediate();
    }
    
    private void SetupRainAudio()
    {
        // Create AudioSource if not assigned
        if (rainAudioSource == null)
        {
            rainAudioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Configure audio source
        if (rainAudioSource != null)
        {
            rainAudioSource.clip = rainSoundClip;
            rainAudioSource.loop = true;
            rainAudioSource.playOnAwake = false;
            rainAudioSource.volume = 0f;
            
            // Optional: Set 3D sound settings for more immersive experience
            rainAudioSource.spatialBlend = 0f; // 0 = 2D, 1 = 3D
        }
    }
    
    private void Update()
    {
        var main = rainParticleSystem.main;
        main.gravityModifier=_gravitymodifier;
        // Check if direction has changed
        if (direction != previousDirection)
        {
            UpdateRainDirection();
            previousDirection = direction;
        }
        if (minVelocity != previousminvalue || maxVelocity != previousmaxvalue)
        {
            UpdateVelocity();
            previousminvalue = minVelocity;
             previousmaxvalue = maxVelocity;
        }
        
        // Keyboard shortcut for testing (R key)
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleRain();
        }
        
        // Ensure cursor stays visible (safety check)
        if (!Cursor.visible)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
        // Performance monitoring (F4 key)
        if (Input.GetKeyDown(KeyCode.F4))
        {
            if (rainParticleSystem != null)
            {
                Debug.Log($"Rain Particles: {rainParticleSystem.particleCount}/{rainParticleSystem.main.maxParticles}");
                Debug.Log($"Emission Rate: {rainParticleSystem.emission.rateOverTime.constant}");
                Debug.Log($"Rain Volume: {rainAudioSource?.volume:F2}");
                Debug.Log($"Rain Direction: {direction}° (X-axis rotation)");
                Debug.Log($"FPS: {1f / Time.unscaledDeltaTime:F1}");
            }
        }
    }
    
    private void UpdateRainDirection()
    {
        if (rainParticleSystem != null)
        {
            // Set the X-axis rotation based on the direction value
            Vector3 currentRotation = rainParticleSystem.transform.eulerAngles;
            rainParticleSystem.transform.rotation = Quaternion.Euler(direction, currentRotation.y, currentRotation.z);
            

        }
    }
    private void UpdateVelocity()
    {
        if (rainParticleSystem != null)
        {
            var velocityModule = rainParticleSystem.velocityOverLifetime;
            velocityModule.z = new ParticleSystem.MinMaxCurve(maxVelocity, minVelocity);
            
        }
    }
    
    public void ToggleRain()
    {
        if (isTransitioning) return;

        if (isRaining)
        {
            StopRain();
        }
        else
        {
            StartRain();
        }
    }
    
    public void StartRain()
    {
        if (isTransitioning || isRaining) return;
        
        StartCoroutine(GraduallyStartRain());
    }
    
    public void StopRain()
    {
        if (isTransitioning || !isRaining) return;
        
        StartCoroutine(GraduallyStopRain());
    }
    
    private IEnumerator GraduallyStartRain()
    {
        isTransitioning = true;
        isRaining = true;
        
        // Enable particle systems
        if (rainParticleSystem != null && !rainParticleSystem.isPlaying)
        {
            rainParticleSystem.Play();
        }
        
        // Enable sub emitters
        // Start rain audio
        if (rainAudioSource != null && rainSoundClip != null)
        {
            rainAudioSource.volume = 0f;
            rainAudioSource.Play();
        }
        
        float elapsedTime = 0f;
        
        while (elapsedTime < transitionDuration)
        {
            float progress = elapsedTime / transitionDuration;
            float easeProgress = EaseInOut(progress);
            
            // Gradually increase emission rate and max particles
            SetRainIntensity(easeProgress);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Ensure we reach maximum values
        SetRainIntensity(1f);
        isTransitioning = false;
    }
    
    private IEnumerator GraduallyStopRain()
    {
        isTransitioning = true;
        
        float elapsedTime = 0f;
        
        while (elapsedTime < transitionDuration)
        {
            float progress = elapsedTime / transitionDuration;
            float easeProgress = EaseInOut(progress);
            
            // Gradually decrease emission rate and max particles
            SetRainIntensity(1f - easeProgress);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Stop completely
        StopRainImmediate();
        isRaining = false;
        isTransitioning = false;
    }
    
    private void SetRainIntensity(float intensity)
    {
        intensity = Mathf.Clamp01(intensity);
        
        // Main rain particle system
        if (rainParticleSystem != null)
        {
            var emission = rainParticleSystem.emission;
            var main = rainParticleSystem.main;
            
            emission.rateOverTime = maxEmissionRate * intensity;
            main.maxParticles = Mathf.RoundToInt(maxParticles * intensity);
        }
        
    
        
        // Update rain sound volume
        if (rainAudioSource != null && fadeWithIntensity)
        {
            rainAudioSource.volume = maxRainVolume * intensity;
        }
    }
    
    private void StopRainImmediate()
    {
        // Stop emission but let existing particles finish
        if (rainParticleSystem != null)
        {
            var emission = rainParticleSystem.emission;
            emission.rateOverTime = 0;
        }
        
        
        // Stop rain audio
        if (rainAudioSource != null)
        {
            rainAudioSource.volume = 0f;
            rainAudioSource.Stop();
        }
    }
    
    // Smooth easing function for natural transitions
    private float EaseInOut(float t)
    {
        return t * t * (3f - 2f * t);
    }
    
    // Public properties for UI or other scripts
    public bool IsRaining => isRaining;
    public bool IsTransitioning => isTransitioning;
    public float RainIntensity
    {
        get
        {
            if (rainParticleSystem != null)
            {
                return rainParticleSystem.emission.rateOverTime.constant / maxEmissionRate;
            }
            return 0f;
        }
    }
    
    // Audio-specific properties
    public float CurrentRainVolume => rainAudioSource?.volume ?? 0f;
    
    // Direction property for external access
    public int RainDirection
    {
        get => direction;
        set
        {
            direction = Mathf.Clamp(value, -45, 145);
            UpdateRainDirection();
        }
    }
    
    // Method to change rain sound at runtime
    public void SetRainSound(AudioClip newRainClip)
    {
        if (rainAudioSource != null)
        {
            bool wasPlaying = rainAudioSource.isPlaying;
            float currentVolume = rainAudioSource.volume;
            
            rainAudioSource.Stop();
            rainAudioSource.clip = newRainClip;
            rainSoundClip = newRainClip;
            
            if (wasPlaying && newRainClip != null)
            {
                rainAudioSource.volume = currentVolume;
                rainAudioSource.Play();
            }
        }
    }
}