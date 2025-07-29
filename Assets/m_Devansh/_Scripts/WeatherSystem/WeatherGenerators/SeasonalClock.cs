using System;
using nminhhoangit.SunCalculator;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class SeasonalClock : MonoBehaviour
{
    [Header("Time Settings")]
    public float dayLengthInSeconds = 60f;

    [Header("Time Components (Read Only)")]
    [SerializeField] private DateTime currentDateTime;
    [SerializeField] private int currentYear;
    [SerializeField] [Range(1,12)]private int currentMonth;
    [SerializeField] [Range(1,31)]private int currentDay;
    [SerializeField] [Range(0,24)] private int currentHour;
    [SerializeField] [Range(0,60)]private int currentMinute;
    [SerializeField] [Range(0,60)]private int currentSecond;
    [SerializeField] public int currentDayOfYear;

    [Header("Environment")]
    public Light sun;
    private SunCalculator _sunCalculator;

    [Header("UI Display")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dateText;

    [Header("Season Configuration")]
    public SeasonRange[] customSeasons =
    {
        new() { season = Season.Spring, startDay = 80, endDay = 171 },
        new() { season = Season.Summer, startDay = 172, endDay = 265 },
        new() { season = Season.Autumn, startDay = 266, endDay = 354 },
        new() { season = Season.Winter, startDay = 355, endDay = 79 }
    };

    public Season currentSeason { get; private set; }

    private float timeSpeed => 24f / dayLengthInSeconds;
    
    public UnityEvent<DateTime> onTimeChanged;
    private void Awake()
    {
        currentDateTime = new DateTime(currentYear, currentMonth, currentDay, 
            Mathf.FloorToInt(currentHour), 
            Mathf.FloorToInt(currentMinute), 
            Mathf.FloorToInt(currentSecond));
        
        if (sun)
        {
            if(sun.TryGetComponent(out SunCalculator suncal))
            {
                this._sunCalculator = suncal;
            }
        }
    }
    void Update()
    {
        ProgressTime();
        UpdateTimeComponents();
        UpdateUI();
    }
    private void ProgressTime()
    {
        double elapsedSeconds = Time.deltaTime * timeSpeed * 3600f;
        currentDateTime = currentDateTime.AddSeconds(elapsedSeconds);
        onTimeChanged?.Invoke(currentDateTime);
    }
    private void UpdateTimeComponents()
    {
        currentSeason = GetSeason(currentDayOfYear);
        currentYear = currentDateTime.Year;
        currentMonth = currentDateTime.Month;
        currentDay = currentDateTime.Day;
        currentHour = currentDateTime.Hour;
        currentMinute = currentDateTime.Minute;
        currentSecond = currentDateTime.Second;
        currentDayOfYear = currentDateTime.DayOfYear;
    }
    private void UpdateUI()
    {
        if (timeText)
        {
            timeText.text = currentDateTime.ToString("hh:mm:ss tt");
        }
        
        if (dateText)
        {
            dateText.text = $"{currentDateTime.ToString("MMMM dd, yyyy")}\n{currentSeason}\nDay {currentDayOfYear}";
        }
    }
    private Season GetSeason(int day)
    {
        foreach (var range in customSeasons)
        {
            bool isInRange = range.startDay > range.endDay 
                ? (day >= range.startDay || day <= range.endDay)
                : (day >= range.startDay && day <= range.endDay);
                
            if (isInRange) return range.season;
        }
        return Season.Winter;
    }

    [System.Serializable]
    public struct Range
    {
        public int start;
        public int end;
    }
    public struct SeasonRange
    {
        public Season season;
        public int startDay;
        public int endDay;
    }
    public enum Season { Spring, Summer, Autumn, Winter }
}
