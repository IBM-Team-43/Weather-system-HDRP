using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SnowManager : WeatherBase
{
    [Header("Snow Shader & Speed")]
    public float snowLevel = 0f;
    public float snowCoverSpeed = 0.05f;
    public Slider snowSpeedSlider;

    [Header("Snow Controls")]
    public Button startSnowButton;
    public Button stopSnowButton;
    public ParticleSystem snowParticles;

    private bool isSnowing = false;

    [Header("Day/Night")]
    public Button dayNightToggleButton;
    public Material skyboxMaterial;
    public Color dayColor = new Color(0.4f, 0.6f, 1f);    // light blue
    public Color nightColor = new Color(0.02f, 0.02f, 0.05f); // deep night

    private bool isDay = true;
    private float colorLerpTime = 2f;

    void Start()
{
    // Force snow off at beginning
    isSnowing = false;
    snowLevel = 0f;
    Shader.SetGlobalFloat("_SnowLevel", 0f);
    UpdateSnowSpeed(100);
}
    void Update()
    {
        if (isSnowing && snowLevel < 1f)
        {
            snowLevel += Time.deltaTime * snowCoverSpeed;
            snowLevel = Mathf.Clamp01(snowLevel);
            Shader.SetGlobalFloat("_SnowLevel", snowLevel);
        }
        else if (!isSnowing && snowLevel > 0f)
        {
            snowLevel -= Time.deltaTime * snowCoverSpeed;
            snowLevel = Mathf.Clamp01(snowLevel);
            Shader.SetGlobalFloat("_SnowLevel", snowLevel);
        }
    }

void UpdateSnowSpeed(float value)
{
    var emission = snowParticles.emission;
    emission.rateOverTime = value * 200f; // Increased from 100 to 200 for more particles

    snowCoverSpeed = Mathf.Lerp(0.01f, 0.2f, value); // Adjustable range
}

protected override void StartWeather()
{
    isSnowing = true;

    // Shader starts fresh
    snowLevel = 0f;
    Shader.SetGlobalFloat("_SnowLevel", snowLevel);

    // Reset + play particles
    snowParticles.Clear(true); // true = stop instantly and clear
    snowParticles.Play();

    // Optional: fade in particle emission rate
    StartCoroutine(FadeInSnowParticles());
}

IEnumerator FadeInSnowParticles(float duration = 3f)
{
    var emission = snowParticles.emission;
    float targetRate = 50f; // or whatever max you want
    float time = 0f;

    while (time < duration)
    {
        float rate = Mathf.Lerp(0f, targetRate, time / duration);
        emission.rateOverTime = rate;
        time += Time.deltaTime;
        yield return null;
    }

    emission.rateOverTime = targetRate;
}
    protected override void StopWeather()
    {
        isSnowing = false;

        if (snowParticles.isPlaying)
            snowParticles.Stop();
    }
     void Toggle()
    {
        isDay = !isDay;
        StopAllCoroutines();

        // Fade between night and day
        StartCoroutine(FadeSkyColor(
            isDay ? nightColor : dayColor, 
            isDay ? dayColor : nightColor));
    }

    IEnumerator FadeSkyColor(Color from, Color to)
    {
        float elapsed = 0f;
        while (elapsed < colorLerpTime)
        {
            Color current = Color.Lerp(from, to, elapsed / colorLerpTime);
            skyboxMaterial.SetColor("_Tint", current); // Make sure "_Tint" exists
            elapsed += Time.deltaTime;
            yield return null;
        }
        skyboxMaterial.SetColor("_Tint", to);
    }
}
