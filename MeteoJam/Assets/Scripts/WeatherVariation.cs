using UnityEngine;
using System.Collections;

public class WeatherVariation : MonoBehaviour
{
    public WeatherIndex weatherIndex = WeatherIndex.PERFECT;
    private WeatherIndex previousWeather = WeatherIndex.NEUTRAL;
    public WeatherIndex weatherMax = WeatherIndex.VERY_COLD;
    public WeatherPrecision precision = WeatherPrecision.PRECISE;

    public float weatherDuration = 8f;
    public float transitionDuration = 2f;

    public uint weatherDelta = 2u;

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

        NEUTRAL
    }

    public enum WeatherPrecision
    {
        PRECISE,
        FLAWED,
        RANDOMISH
    }

    // Use this for initialization
    void Start()
    {
    }
	
	// Update is called once per frame
	void Update()
    {
        DrawNextWeather();
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
        while (firstPick != previousWeather);

        previousWeather = weatherIndex;
        weatherIndex = firstPick;
        Debug.Log(weatherIndex.ToString());
    }
}
