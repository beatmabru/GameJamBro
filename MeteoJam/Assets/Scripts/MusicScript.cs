﻿using UnityEngine;
using System.Collections;

public class MusicScript : MonoBehaviour {

    public AudioSource musicNeutre;
    public AudioSource musicHot;
    public AudioSource musicCold;
    public AudioSource winSong;
    public AudioSource looseSong;


    float maxVolume = 0.3f;


    private WeatherManager2 _weather;

    void Awake()
    {
        GameObject variationHolder = GameObject.Find("WeatherManager2");
        Debug.Assert(variationHolder != null);
        _weather = variationHolder.GetComponent<WeatherManager2>();
    }


	// Use this for initialization
	void Start () {
        musicNeutre.Play();
        musicHot.Play();
        musicCold.Play();
        musicNeutre.loop = true;
        musicHot.loop = true;
        musicCold.loop = true;
        musicCold.volume = 0;
        musicHot.volume = 0;
        musicNeutre.volume = maxVolume;
    }
	
	// Update is called once per frame
	void Update () {
        if((int)_weather.currentWeather  < 3)
        {
            StartCoroutine(fadeInTransition(musicHot));
            StartCoroutine(fadeOutTransition(musicCold));
        }
        else if((int)_weather.currentWeather == 3)
        {
            StartCoroutine(fadeOutTransition(musicHot));
            StartCoroutine(fadeOutTransition(musicCold));
        }
        else
        {
            StartCoroutine(fadeInTransition(musicCold)); 
            StartCoroutine(fadeOutTransition(musicHot));
        }
	}

    public void NotifyEndGame(bool isWin)
    {
        musicNeutre.Stop();
        musicCold.Stop();
        musicHot.Stop();
        if (isWin)
            winSong.Play();
        else
            looseSong.Play();
    }

    IEnumerator fadeInTransition(AudioSource audioSource )
    {
        while(audioSource.volume < maxVolume)
        {
            audioSource.volume += 0.1f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    IEnumerator fadeOutTransition(AudioSource audioSource )
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= 3*Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
