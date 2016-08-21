using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeatherVariation : MonoBehaviour, EventDispatcher.IEventListener
{
    private Forecaster _forecast;
    public Text weatherText;

    [HideInInspector]
    public WeatherIndex weatherIndex = WeatherIndex.PERFECT;
    public WeatherIndex weatherMax = WeatherIndex.VERY_COLD;

    public enum State
    {
        WEATHER,
        RESPITE
    }
    public float weatherDuration = 3f;
    public float respiteDuration = 7f;
    [HideInInspector]
    public State state = State.RESPITE;
    private float duration = 0f;

    public float freezingUnlockDuration = 30f;
    public float absoluteZeroUnlockDuration = 40f;

    public uint weatherDelta = 2u;
    private uint playerDeaths = 0u;

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

    void Awake()
    {
        _forecast = GetComponent<Forecaster>();
        EventDispatcher.instance.listeners.Add(this);
    }
        

    // Use this for initialization
    void Start()
    {
        playerDeaths = 0u;
        weatherIndex = WeatherIndex.PERFECT;
        _forecast.forecastText.gameObject.SetActive(true);
        weatherText.gameObject.SetActive(false);
        GoToRespite();
    }

    // Update is called once per frame
    void Update()
    {
        if (IngameManager.instance.gameOver) return;
        TryUnlock();

        duration -= Time.deltaTime;
        if(duration < 0f)
        {
            ChangeState();
        }
    }

    private void ChangeState()
    {
        _forecast.Animate((int)state);
        if (state == State.RESPITE)
        {
            _forecast.forecastText.gameObject.SetActive(false);
            weatherText.gameObject.SetActive(true);
            state = State.WEATHER;
            duration = weatherDuration;
        }
        else if (state == State.WEATHER)
        {
            _forecast.forecastText.gameObject.SetActive(true);
            weatherText.gameObject.SetActive(false);
            GoToRespite();
        }

        EventDispatcher.Event weatherChange = new EventDispatcher.Event(EventDispatcher.EventId.WEATHER_RESPITE_CHANGE, (object)state);
        EventDispatcher.instance.ThrowEvent(weatherChange);

        Debug.Assert(duration > 0f);
    }

    private void GoToRespite()
    {
        state = State.RESPITE;
        duration = respiteDuration;
        PickNextWeather();
    }

    // Checks conditions for unlocking weather indexes
    void TryUnlock()
    {
        if (weatherMax == WeatherIndex.ABSOLUTE_ZERO)
            return;

        if (IngameManager.instance.gameTime >= absoluteZeroUnlockDuration || playerDeaths >= 2u)
        {
            weatherMax = WeatherIndex.ABSOLUTE_ZERO;
            return;
        }

        if (weatherMax != WeatherIndex.FREEZING 
            && (IngameManager.instance.gameTime >= freezingUnlockDuration || playerDeaths >= 1u))
            weatherMax = WeatherIndex.FREEZING;
    }

    //Draw a WeatherIndex and apply WeatherPrecision on it
    void PickNextWeather()
    {
        //"real" pick before forecast variation
        WeatherIndex firstPick;

        firstPick = weatherIndex;
        bool positive = true;
        if (weatherIndex == weatherMax)
            positive = false;
        else if (weatherIndex != WeatherIndex.HEATWAVE)
            positive = Random.Range(0, 2) == 0;

        int delta = weatherDelta == 0u ? 0 : (Random.Range(0, (int)weatherDelta) + 1);

        if (positive)
            firstPick = (WeatherIndex)Mathf.Min((int)weatherIndex + delta, (int)weatherMax);
        else
            firstPick = (WeatherIndex)Mathf.Max((int)weatherIndex - delta, (int)WeatherIndex.HEATWAVE);

        weatherIndex = firstPick;
        weatherText.text = "The current weather is : " + weatherIndex;

        _forecast.ComputeForecast();
    }

    public bool HandleEvent(EventDispatcher.Event e)
    {
        if(e.id == EventDispatcher.EventId.PLAYER_DEATH)
        {
            ++playerDeaths;
            TryUnlock();
        }
        return false;
    }
}
