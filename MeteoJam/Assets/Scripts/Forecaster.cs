using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Forecaster : MonoBehaviour {
    private WeatherVariation _weatherVariation;
    public Precision precision = Precision.PRECISE;
    public float flawedUnlockDuration = 60f;
    public float randomUnlockDuration = 90f;
    public Text forecastText;

    public enum Precision
    {
        PRECISE,
        FLAWED,
        RANDOM
    }

    public List<string> listWeatherText = new List<string>() {
        "Actually, I'm not wearing any pants right now...",
        "Time to go topless!",
        "Our advice? A baseball cap and a cool sweater",
        "Perfect weather for a healthy stroll in the park!",
        "Take your jacket before going out!",
        "Don't forget the hood yo!",
        "Atcha! Where are my scarf and gloves?",
        "Put on all your things or freeze to death!",
        "Who turned the sun off?!"
    };

    public List<string> listPreciseText = new List<string>() {
        "My Name is Steven Sbatar and welcome to the Weather Channel!",
        "We're very confident in our forecasts",
        "100% accurate"
    };

    public List<string> listFlawedText = new List<string>() {
        "Meteor shower?! ... Just kidding!!",
        "Winter is comming, or is it?"
    };

    public List<string> listRandomText = new List<string>() {
        "Some heavy rain or a happy sky? ... May be?",
        "Who's playing with my climat-tact?"
    };

    public List<string> listExcuseText = new List<string>() {
        "It never happened before...",
        "That damned intern again...",
        "Someone hacked our sattelite",
        "Oops.. I did it again!",
        "But that's what I said!"
    };

    void Awake()
    {
        _weatherVariation = GetComponent<WeatherVariation>();
    }

    private WeatherVariation.WeatherIndex forecast;

    // Use this for initialization
    void Start () {
     
    }

    // Update is called once per frame
    void Update () {
        TryUnlock();
    }

    /*
    void OnGUI()
    {
        GUI.Label(new Rect(10, 50, 200, 20), "Current Forecast:" + forecast.ToString());
    }
    */

    public void ComputeForecast()
    {
        switch (precision)
        {
            case Precision.PRECISE:
                if (Random.Range(0, 100) > 75)
                    forecast = VariationWeather(1,1);
                else
                    forecast = _weatherVariation.weatherIndex;
                break;
            case Precision.FLAWED:
                if (Random.Range(0, 10) > 4)
                    forecast = VariationWeather(1,3);
                else
                    forecast = _weatherVariation.weatherIndex;
                break;
            case Precision.RANDOM:
                int percent = Random.Range(0, 10);
                if (percent <= 2)
                    forecast = _weatherVariation.weatherIndex;
                else if (percent < 7)
                    forecast = VariationWeather(1,3);
                else
                    forecast = VariationWeather(2,4);

                break;
            default:
                break;
        }
        
        forecastText.text = "The incoming weather is : " + forecast;
    }

    private WeatherVariation.WeatherIndex VariationWeather(int min, int max)
    {
        uint weatherDelta;
        if (min < max)
            weatherDelta = (uint)Random.Range(min, max);
        else
            weatherDelta = (uint)min;

        bool positive = true;
        if (_weatherVariation.weatherIndex == _weatherVariation.weatherMax)
            positive = false;
        else if (_weatherVariation.weatherIndex != WeatherVariation.WeatherIndex.HEATWAVE)
            positive = Random.Range(0, 2) == 0;

        int delta = weatherDelta == 0u ? 0 : (Random.Range(0, (int)weatherDelta) + 1);

        if (positive)
            return (WeatherVariation.WeatherIndex)Mathf.Min((int)_weatherVariation.weatherIndex + delta, (int)_weatherVariation.weatherMax);
        else
            return (WeatherVariation.WeatherIndex)Mathf.Max((int)_weatherVariation.weatherIndex - delta, (int)WeatherVariation.WeatherIndex.HEATWAVE);
    }

    // Checks conditions for unlocking weather indexes
    void TryUnlock()
    {
        if (precision == Precision.RANDOM)
            return;

        if (Time.realtimeSinceStartup >= randomUnlockDuration)
        {
            precision = Precision.RANDOM;
            return;
        }

        if (precision != Precision.FLAWED && Time.realtimeSinceStartup >= flawedUnlockDuration)
            precision = Precision.FLAWED;
    }
}
