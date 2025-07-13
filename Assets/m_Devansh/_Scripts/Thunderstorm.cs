using UnityEngine;

public class Thunderstorm : WeatherBase
{
    public GameObject lightningEffect;
    public GameObject hailstormEffect;
    public AudioSource thunderSound;
    protected override void StartWeather()
    {
        hailstormEffect.SetActive(true);
    }
    protected override void StopWeather()
    {
        hailstormEffect.SetActive(false);
    }
    void TurnOnLightning()
    {
        if (lightningEffect != null)
            lightningEffect.SetActive(true);


        if (thunderSound != null && !thunderSound.isPlaying)
            thunderSound.Play();
    }
    void TurnOffLightning()
    {
        if (lightningEffect != null)
            lightningEffect.SetActive(false);
        

        if (thunderSound != null && thunderSound.isPlaying)
            thunderSound.Stop();
    }
}