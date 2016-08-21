using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeatherManager2 : MonoBehaviour
{
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

    public AudioSource narratorAudioSource;
    public WeatherIndex currentWeather = WeatherIndex.CHILLY;
    public Animator weatherAnimator;
    public Animator franceAnimator;
    public Animator narratorAnimator;
    public Animator bubbleAnimator;
    public Text bubbleText;
    public Text ecnarfText;

    public float timeStep = 10f;
    private float currentTimer = 10f;

    void OnGUI()
    {
        GUILayout.Label("currentWeather " + currentWeather);
    }

    // Use this for initialization
    void Start ()
    {
        currentWeather = WeatherIndex.CHILLY;
    }
	
	// Update is called once per frame
	void Update ()
    {
        currentTimer -= Time.deltaTime;
        if(currentTimer<=0)
        {
            currentTimer = timeStep;
            ChangeWeather();
        }
	}

    void ChangeWeather()
    {
        int rnd = Random.Range(0, (int)WeatherIndex._COUNT -1);

        currentWeather = (WeatherIndex)rnd;
        ChangeAllEffects();
    }

    private void ChangeAllEffects()
    {
        bool narratorPerfect = false;

        int temp = 19;

        string narratorSpeech = "";

        switch (currentWeather)
        {
            case WeatherIndex.HEATWAVE:
                temp = 45;
                narratorSpeech = "Actually, I'm not wearing any pants right now...";
                break;
            case WeatherIndex.VERY_HOT:
                temp = 5;
                narratorSpeech = "Time to go topless!";
                break;
            case WeatherIndex.HOT:
                temp = 25;
                narratorSpeech = "Our advice? A baseball cap and a cool sweater";
                break;
            case WeatherIndex.PERFECT:
                temp = 19;
                narratorPerfect = true;
                narratorSpeech = "Perfect weather for a healthy stroll in the park!";
                break;
            case WeatherIndex.CHILLY:
                temp = 12;
                narratorSpeech = "Take your jacket before going out!";
                break;
            case WeatherIndex.COLD:
                temp = 6;
                narratorSpeech = "Don't forget the hood yo!";
                break;
            case WeatherIndex.VERY_COLD:
                temp = 0;
                narratorSpeech = "Atcha! Where are my scarf and gloves?";
                break;
            case WeatherIndex.FREEZING:
                temp = -15;
                narratorSpeech = "Put on all your things or freeze to death!";
                break;
            case WeatherIndex.ABSOLUTE_ZERO:
                temp = -273;
                narratorSpeech = "Who turned the sun off?!";
                break;
            default:
                break;
        }

        weatherAnimator.SetInteger("temperature", temp);


        StartCoroutine(DelayCoroutine(temp, narratorPerfect, narratorSpeech));
    }

    IEnumerator DelayCoroutine(int temp,bool narratorPerfect, string narratorSpeech)
    {
        float delay = 1f;
        while(delay>0)
        {
            delay -= Time.deltaTime;
            yield return 0;
        }

        //sounds
        AudioClip clip = AudioClipManager.instance.GetForcasterWeatherById((int)currentWeather);
        playSound(clip);

        //animate METEO
        ecnarfText.text = temp + "C";

        bubbleText.text = narratorSpeech;
        bubbleAnimator.SetTrigger("Spawn");
        franceAnimator.SetInteger("temperature", temp);

        //Narrator 
        if (narratorPerfect)
        {
            narratorAnimator.SetTrigger("ThumbsUp");
        }
        else
        {
            int rnd = Random.Range(0, 5);

            switch (rnd)
            {
                case 0:
                    narratorAnimator.SetTrigger("Show");
                    break;
                case 1:
                    narratorAnimator.SetTrigger("Advice");
                    break;
                case 2:
                    narratorAnimator.SetTrigger("Surprise");
                    break;
                case 3:
                    narratorAnimator.SetTrigger("Laugh");
                    break;
                case 4:
                    narratorAnimator.SetTrigger("Sorry");
                    break;
                default:
                    narratorAnimator.SetTrigger("Show");
                    break;
            }
        }
    }

    IEnumerator correctSoundVolumeAfterPlaying()
    {
        while (narratorAudioSource.isPlaying) { yield return new WaitForEndOfFrame(); }
        changePlayerVolume(1.0f);
        yield return null;

    }

    void playSound(AudioClip clip)
    {
        narratorAudioSource.clip = clip;
        changePlayerVolume(0.2f);

        narratorAudioSource.Play();
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
}
