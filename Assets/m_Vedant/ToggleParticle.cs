using UnityEngine;

public class ToggleParticle: WeatherBase
{
    // Reference to the Particle System
    public ParticleSystem particleSystem;

    // Track if the particle system is playing
    private bool isPlaying = false;
    void Start()
    {
        // Auto-assign if not manually set in inspector
        if (particleSystem == null)
        {
            particleSystem = GetComponent<ParticleSystem>();
        }

        if (particleSystem == null)
        {
            Debug.LogError("No ParticleSystem assigned or found on GameObject!");
        }
    }

    protected override void StartWeather()
    {
        if (particleSystem != null)
        {
            particleSystem.Play();
            isPlaying = true;
        }
    }

    protected override void StopWeather()
    {
        if (particleSystem != null)
        {
            particleSystem.Stop();
            isPlaying = false;
        }
    }
}
