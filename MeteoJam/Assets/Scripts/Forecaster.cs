using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Forecaster : MonoBehaviour
{
    private WeatherVariation _weatherVariation;
    public Precision precision = Precision.PRECISE;
    public float flawedUnlockDuration = 60f;
    public float randomUnlockDuration = 90f;
    public AudioSource audioSource;
    public Text forecastText;



    public enum Precision
    {
        PRECISE,
        FLAWED,
        RANDOM
    }

    public Animator animatorBubble;
    public Animator animatorNarrator;
    public Animator animatorEcnarf;
    public Animator animatorCamera;

    public Text ecnarfText;

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
        "Some heavy rain or a happy sky? ... Maybe?",
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
        audioSource = GetComponent<AudioSource>();

    }

    public WeatherVariation.WeatherIndex forecast;
    private int indexSentenceExcuse;
    private int indexSentenceWeather;
    private int indexSentencePrecise;
    private int indexSentenceFlawed;
    private int indexSentenceRandom;
    private string temp;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
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
                    forecast = VariationWeather(1, 1);
                else
                    forecast = _weatherVariation.weatherIndex;
                break;
            case Precision.FLAWED:
                if (Random.Range(0, 10) > 4)
                    forecast = VariationWeather(1, 3);
                else
                    forecast = _weatherVariation.weatherIndex;
                break;
            case Precision.RANDOM:
                int percent = Random.Range(0, 10);
                if (percent <= 2)
                    forecast = _weatherVariation.weatherIndex;
                else if (percent < 7)
                    forecast = VariationWeather(1, 3);
                else
                    forecast = VariationWeather(2, 4);

                break;
            default:
                break;
        }

        AudioClip clip = AudioClipManager.instance.GetForcasterWeatherById((int)forecast);
        playSound(clip);
        StartCoroutine(playnarratorResult(forecast == _weatherVariation.weatherIndex));

    }

    IEnumerator playnarratorResult(bool wasTrue)
    {
        yield return new WaitForSeconds(3);
        AudioClip clip = AudioClipManager.instance.GetForcasterResult(wasTrue);
        playSound(clip);

    }

    IEnumerator correctSoundVolumeAfterPlaying()
    {
        while (audioSource.isPlaying) { yield return new WaitForEndOfFrame(); }
        changePlayerVolume(1.0f);
        yield return null;

    }

    void playSound(AudioClip clip)
    {
        audioSource.clip = clip;
        changePlayerVolume(0.2f);

        audioSource.Play();
        StartCoroutine(correctSoundVolumeAfterPlaying());
    }

    void changePlayerVolume(float volume)
    {
        AudioClipManager.instance.player1.VoiceSource.volume = volume;
        AudioClipManager.instance.player2.VoiceSource.volume = volume;
        AudioClipManager.instance.player3.VoiceSource.volume = volume;
        AudioClipManager.instance.player4.VoiceSource.volume = volume;

        AudioClipManager.instance.player1.SFXSource.volume = volume;
        AudioClipManager.instance.player2.SFXSource.volume = volume;
        AudioClipManager.instance.player3.SFXSource.volume = volume;
        AudioClipManager.instance.player4.SFXSource.volume = volume;
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

    public void Animate(int state)
    {
        //weather
        if (state == 1)
        {
            switch (_weatherVariation.weatherIndex)
            {
                case WeatherVariation.WeatherIndex.HEATWAVE:
                    temp = "45";
                    break;
                case WeatherVariation.WeatherIndex.VERY_HOT:
                    temp = "35";
                    break;
                case WeatherVariation.WeatherIndex.HOT:
                    temp = "25";
                    break;
                case WeatherVariation.WeatherIndex.PERFECT:
                    temp = "19";
                    break;
                case WeatherVariation.WeatherIndex.CHILLY:
                    temp = "12";
                    break;
                case WeatherVariation.WeatherIndex.COLD:
                    temp = "6";
                    break;
                case WeatherVariation.WeatherIndex.VERY_COLD:
                    temp = "0";
                    break;
                case WeatherVariation.WeatherIndex.FREEZING:
                    temp = "-15";
                    break;
                case WeatherVariation.WeatherIndex.ABSOLUTE_ZERO:
                    temp = "-273";
                    break;
                default:
                    break;
            }
            ecnarfText.text = temp+"C";
            animatorEcnarf.SetInteger("temperature",int.Parse(temp));
            //weather prédiction wrong
            if (_weatherVariation.weatherIndex != forecast)
            {
                //anim wrong
                animatorNarrator.SetTrigger("Sorry");

                //sentence sorry
                indexSentenceExcuse = Mathf.CeilToInt(Random.Range(0.1f, listExcuseText.Count)) -1 ;
                animatorBubble.gameObject.GetComponentInChildren<Text>().text = listExcuseText[indexSentenceExcuse];
                animatorBubble.SetTrigger("Spawn");
            }

            if ((int)precision == 3)
            {
                //anim thumb up + smile
                animatorNarrator.SetTrigger("ThumbsUp");
            }
            else
            {
                //anim finger up
                animatorNarrator.SetTrigger("Advice");
            }

            //change text with a the current sentence from listWeatherText
            indexSentenceWeather = Mathf.CeilToInt(Random.Range(0.1f, listWeatherText.Count)) - 1;
            animatorBubble.gameObject.GetComponentInChildren<Text>().text = listWeatherText[indexSentenceWeather];
            animatorBubble.SetTrigger("Spawn");

        }
        else
        { //respite
            switch (precision)
            {
                case Precision.PRECISE:
                    //change text with a random sentence from listPreciseText
                    indexSentencePrecise = Mathf.CeilToInt(Random.Range(0.1f, listPreciseText.Count)) - 1;
                    animatorBubble.gameObject.GetComponentInChildren<Text>().text = listPreciseText[indexSentencePrecise];
                    animatorBubble.SetTrigger("Spawn");
                    break;
                case Precision.FLAWED:
                    //change text with a random sentence from listFlawedText
                    indexSentenceFlawed = Mathf.CeilToInt(Random.Range(0.1f, listFlawedText.Count)) - 1;
                    animatorBubble.gameObject.GetComponentInChildren<Text>().text = listFlawedText[indexSentenceFlawed];
                    animatorBubble.SetTrigger("Spawn");
                    break;
                case Precision.RANDOM:
                    //change text with a random sentence from listRandomText
                    indexSentenceRandom = Mathf.CeilToInt(Random.Range(0.1f, listRandomText.Count)) - 1;
                    animatorBubble.gameObject.GetComponentInChildren<Text>().text = listRandomText[indexSentenceRandom];
                    animatorBubble.SetTrigger("Spawn");
                    break;
                default:
                    break;
            }
            //anim show weather
            animatorNarrator.SetTrigger("Show");
        }
    }
}
