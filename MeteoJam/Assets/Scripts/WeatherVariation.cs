using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeatherVariation : MonoBehaviour
{
    public static WeatherVariation instance;

    [HideInInspector]
    public WeatherIndex weatherIndex = WeatherIndex.PERFECT;
    public WeatherIndex weatherMax = WeatherIndex.VERY_COLD;
    public Precision precision = Precision.PRECISE;

    public enum State
    {
        WEATHER,
        RESPITE
    }
    public float weatherDuration = 8f;
    public float respiteDuration = 2f;
    private State state = State.RESPITE;
    private float duration = 0f;

    public float freezingUnlockDuration = 60f;
    public float absoluteZeroUnlockDuration = 90f;

    public uint weatherDelta = 2u;
    
    private WeatherIndex previousWeather = WeatherIndex.PERFECT;

    public enum Precision
    {
        PRECISE,
        FLAWED,
        RANDOMISH
    }

    public enum WeatherIndex
    {
        HEATWAVE,
        VERY_HOT,
        HOT,
        PERFECT,
        CHILLY,
        COLD,
        VERY_COLD,
        FREEZING,//locked at start
        ABSOLUTE_ZERO,//locked at start, unlocked after freezing
        _COUNT
    }

    // Use this for initialization
    void Start()
    {
        instance = this;
        previousWeather = weatherIndex;

        GoToRespite();
    }
	
	// Update is called once per frame
	void Update()
    {
        TryUnlock();

        duration -= Time.deltaTime;
        if(duration < 0f)
        {
            ChangeState();
        }
    }

    private void ChangeState()
    {
        if(state==State.RESPITE)
        {
            state = State.WEATHER;
            duration = weatherDuration;
        }
        else if(state == State.WEATHER)
        {
            GoToRespite();
        }

        Debug.Assert(duration > 0f);
    }

    private void GoToRespite()
    {
        state = State.RESPITE;
        duration = respiteDuration;
        DrawNextWeather();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 30, 200, 20), "Current Weather:" + weatherIndex.ToString());
    }

    // Checks conditions for unlocking weather indexes
    void TryUnlock()
    {
        //TODO: handle deaths of players to unlock indexes sooner

        if (weatherMax == WeatherIndex.ABSOLUTE_ZERO)
            return;

        if (Time.realtimeSinceStartup >= absoluteZeroUnlockDuration)
        {
            weatherMax = WeatherIndex.ABSOLUTE_ZERO;
            return;
        }

        if (weatherMax != WeatherIndex.FREEZING && Time.realtimeSinceStartup >= freezingUnlockDuration)
            weatherMax = WeatherIndex.FREEZING;
    }

    //Draw a WeatherIndex and apply WeatherPrecision on it
    void DrawNextWeather()
    {
        //"real" pick before forecast variation
        WeatherIndex firstPick;
        do
        {
            firstPick = weatherIndex;
            bool positive = true;
            if (weatherIndex == weatherMax)
                positive = false;
            else if(weatherIndex != WeatherIndex.HEATWAVE)
                positive = Random.Range(0, 2) == 0;

            int delta = weatherDelta == 0u ? 0 : (Random.Range(0, (int)weatherDelta) + 1);
            
            if (positive)
                firstPick = (WeatherIndex)Mathf.Min((int)weatherIndex + delta, (int)weatherMax);
            else
                firstPick = (WeatherIndex)Mathf.Max((int)weatherIndex - delta, (int)WeatherIndex.HEATWAVE);
        } //ignore the previous value
        while (firstPick == previousWeather);

        previousWeather = weatherIndex;
        weatherIndex = firstPick;
    }
}
