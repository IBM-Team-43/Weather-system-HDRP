using UnityEngine;

public class ToggleParticle: MonoBehaviour
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

    void Update()
    {
        // Toggle on spacebar press
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isPlaying)
            {
                StopParticles();
            }
            else
            {
                StartParticles();
            }
        }
    }

    public void StartParticles()
    {
        if (particleSystem != null)
        {
            particleSystem.Play();
            isPlaying = true;
        }
    }

    public void StopParticles()
    {
        if (particleSystem != null)
        {
            particleSystem.Stop();
            isPlaying = false;
        }
    }
}
