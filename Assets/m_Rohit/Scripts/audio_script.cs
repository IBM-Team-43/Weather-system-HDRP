using UnityEngine;

public class audio_script : MonoBehaviour
{
    public AudioSource audioSource;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}

