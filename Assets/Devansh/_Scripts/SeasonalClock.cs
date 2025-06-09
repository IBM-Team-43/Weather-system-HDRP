using UnityEngine;
using TMPro;

public class SeasonalClock : MonoBehaviour
{
    [Header("Time Settings")]
    [Range(0, 24)] public float timeOfDay = 12f;
    [Range(1, 365)] public int dayOfYear = 1;

    [Header("Day Progression")]
    public float dayLengthInSeconds = 60f;
    private float timeSpeed => 24f / dayLengthInSeconds;

    [Header("Sun Settings")]
    public Light sun;
    public Vector3 sunRotationAxis = Vector3.right;

    [Header("UI Display (Optional)")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI seasonText;
    public TextMeshProUGUI monthText;

    [System.Serializable]
    public struct SeasonRange
    {
        public Season season;
        public int startDay;
        public int endDay;
    }

    [Header("Custom Season Ranges")]
    public SeasonRange[] customSeasons = new SeasonRange[4]
    {
        new SeasonRange { season = Season.Spring, startDay = 80, endDay = 171 },
        new SeasonRange { season = Season.Summer, startDay = 172, endDay = 265 },
        new SeasonRange { season = Season.Autumn, startDay = 266, endDay = 354 },
        new SeasonRange { season = Season.Winter, startDay = 355, endDay = 79 }
    };
    public enum Season { Spring, Summer, Autumn, Winter }
    public enum Month
    {
        January, February, March, April, May, June,
        July, August, September, October, November, December
    }

    public Season currentSeason;
    public Month currentMonth;

    private readonly int[] daysInMonths = new int[]
    {
        31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31
    };

    void Update()
    {
        timeOfDay += Time.deltaTime * timeSpeed;
        if (timeOfDay >= 24f)
        {
            timeOfDay = 0f;
            dayOfYear++;
            if (dayOfYear > 365) dayOfYear = 1;
        }

        currentSeason = GetSeason(dayOfYear);
        currentMonth = GetMonth(dayOfYear);

        UpdateSunRotation();

        UpdateUI();
    }

    Season GetSeason(int day)
    {
        foreach (var range in customSeasons)
        {
            if (range.startDay > range.endDay)
            {
                if (day >= range.startDay || day <= range.endDay)
                    return range.season;
            }
            else
            {
                if (day >= range.startDay && day <= range.endDay)
                    return range.season;
            }
        }
        return Season.Winter;
    }

    string FormatTime12Hour(float time)
    {
        int hour = Mathf.FloorToInt(time) % 24;
        int minute = Mathf.FloorToInt((time - hour) * 60f);

        string period = hour >= 12 ? "PM" : "AM";
        int hour12 = hour % 12;
        if (hour12 == 0) hour12 = 12;

        return $"{hour12:D2}:{minute:D2} {period}";
    }
    Month GetMonth(int dayOfYear)
    {
        int cumulative = 0;
        for (int i = 0; i < daysInMonths.Length; i++)
        {
            cumulative += daysInMonths[i];
            if (dayOfYear <= cumulative)
                return (Month)i;
        }
        return Month.December; // Fallback
    }

    void UpdateSunRotation()
    {
        if (sun)
        {
            float sunAngle = (timeOfDay / 24f) * 360f;
            sun.transform.rotation = Quaternion.Euler(sunAngle, 0f, 0f);
        }
    }
    void UpdateUI()
    {
        if (timeText) timeText.text = $"Time: {FormatTime12Hour(timeOfDay)}\nDay: {dayOfYear}";
        if (seasonText) seasonText.text = $"Season: {currentSeason}";
        if (monthText) monthText.text = $"Month: {currentMonth}";
    }
}
