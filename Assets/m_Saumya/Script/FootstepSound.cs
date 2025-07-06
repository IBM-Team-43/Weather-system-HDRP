using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class FootstepSound : MonoBehaviour
{
    public AudioClip footstepClip;   // Your footstep audio
    public float stepInterval = 0.5f;

    private CharacterController controller;
    private AudioSource audioSource;
    private float stepTimer;
    private bool isMoving;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void Update()
    {
        // Check for movement input (W, A, S, D or arrow keys)
        isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (isMoving)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                // Play footstep sound
                audioSource.PlayOneShot(footstepClip);
                stepTimer = stepInterval;
            }
        }
        else
        {
            // Stop the footstep sound if not moving
            audioSource.Stop();
            stepTimer = 0f;  // Reset the step timer
        }
    }
}
