using UnityEngine;

public class lightning_script : MonoBehaviour
{
    public GameObject lightningEffect;
    public Light lightningFlash;
    public AudioSource thunderSound;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            TurnOnLightning();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            TurnOffLightning();
        }
    }

    void TurnOnLightning()
    {
        if (lightningEffect != null)
            lightningEffect.SetActive(true);

        if (lightningFlash != null)
            lightningFlash.enabled = true;

        if (thunderSound != null && !thunderSound.isPlaying)
            thunderSound.Play();
    }

    void TurnOffLightning()
    {
        if (lightningEffect != null)
            lightningEffect.SetActive(false);

        if (lightningFlash != null)
            lightningFlash.enabled = false;

        if (thunderSound != null && thunderSound.isPlaying)
            thunderSound.Stop();
    }
}
