using UnityEngine;
using System.Collections;

public class MusicScript : MonoBehaviour {

    public AudioSource musicNeutre;
    public AudioSource musicHot;
    public AudioSource musicCold;

    float maxVolume = 0.5f;
	// Use this for initialization
	void Start () {
        musicNeutre.Play();
        musicHot.Play();
        musicCold.Play();
        musicCold.volume = 0;
        musicHot.volume = 0;
        musicNeutre.volume = maxVolume;
        
    }
	
	// Update is called once per frame
	void Update () {
        if((int)WeatherVariation.instance.weatherIndex  < 3)
        {

            StartCoroutine(fadeInTransition(musicHot));
            StartCoroutine(fadeOutTransition(musicCold));
        }
        else if((int)WeatherVariation.instance.weatherIndex == 3)
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
