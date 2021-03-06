﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioClipManager : MonoBehaviour {

    public static AudioClipManager instance;

    public AudioClip[] narratorAudioClip;
    public AudioClip[] playerAudioClip;
    public AudioClip[] sfxAudioClip;

    List<AudioClip> listPlayerDeathByCold = new List<AudioClip>();
    List<AudioClip> listPlayerDeathByHot = new List<AudioClip>();
    List<AudioClip> listPlayerPush = new List<AudioClip>();
    List<AudioClip> listPlayerPushed = new List<AudioClip>();
    List<AudioClip> listPlayerWin = new List<AudioClip>();
    List<AudioClip> listPlayerItemGet = new List<AudioClip>();

    List<AudioClip> listForcasterWrongprediction = new List<AudioClip>();
    List<AudioClip> listForcasterGoodprediction = new List<AudioClip>();
   //AudioClip listForcasterprediction;
    //AudioClip forcasterHot;
    //AudioClip forcasterCold;
    List<AudioClip> listForcasterWeathers = new List<AudioClip>();

    public AudioClip attackSFX;
    public AudioClip attackMissedSFX;
    public AudioClip equipSFX;
    public AudioClip throwCloth;

    public PlayerManager player1;
    public PlayerManager player2;
    public PlayerManager player3;
    public PlayerManager player4;

    private List<AudioSource> audioSources;
 

    // Use this for initialization

    void Awake()
    {
        instance = this;
        audioSources = new List<AudioSource>();
        audioSources.Add(null);
        audioSources.Add(null);
        audioSources.Add(null);
        narratorAudioClip = Resources.LoadAll<AudioClip>("Sound/narrator");
        playerAudioClip = Resources.LoadAll<AudioClip>("Sound/player");

        foreach (AudioClip clip in playerAudioClip)
        {
            if (clip.name.Contains("etrepousser"))
            {
                listPlayerPushed.Add(clip);
            }
            else if (clip.name.Contains("itemGet"))
            {
                listPlayerItemGet.Add(clip);
            }
            else if (clip.name.Contains("mortchaud"))
            {
                listPlayerDeathByHot.Add(clip);
            }
            else if (clip.name.Contains("mortfroid"))
            {
                listPlayerDeathByCold.Add(clip);
            }
            else if (clip.name.Contains("win"))
            {
                listPlayerWin.Add(clip);
            }
            else if (clip.name.Contains("pousser"))
            {
                listPlayerPush.Add(clip);
            }
        }
        foreach (AudioClip clip in narratorAudioClip)
        {
            if (clip.name.Contains("wrongprediction"))
            {
                listForcasterWrongprediction.Add(clip);
            }
            else if (clip.name.Contains("goodprediction"))
            {
                listForcasterGoodprediction.Add(clip);
            }
            else if(clip.name == "predictions-andtheweatheris")
            {
                //listForcasterprediction = clip;
            }
            else if (clip.name == "tooHot")
            {
               // forcasterHot = clip;
            }
            else if (clip.name == "coldd")
            {
                //forcasterCold = clip;
            }
            else if(clip.name.Contains("forcast"))
            {
                listForcasterWeathers.Add(clip);
            }
        }

     attackSFX = Resources.Load<AudioClip>("Sound/sfx/attack");
     attackMissedSFX = Resources.Load<AudioClip>("Sound/sfx/attackmissed");
     equipSFX = Resources.Load<AudioClip>("Sound/sfx/equipement");
     throwCloth = Resources.Load<AudioClip>("Sound/sfx/retombejsaispas");
        
    }

    void Start () {
        
    }

    public AudioClip GetForcasterWeatherById(int id)
    {
        return listForcasterWeathers[id];
    }

    public AudioClip GetForcasterResult(bool wasTrue)
    {
        AudioClip returnVal;
        if (wasTrue)
        {
            int randomIndex = Mathf.CeilToInt(Random.Range(0.01f, listForcasterGoodprediction.Count)) - 1;
            returnVal =  listForcasterGoodprediction[randomIndex];
        }
        else
        {
            int randomIndex = Mathf.CeilToInt(Random.Range(0.01f, listForcasterWrongprediction.Count)) - 1;
            returnVal= listForcasterWrongprediction[randomIndex];
        }
        return returnVal;
       
    }

    public int numberPlayerPlayingVoice()
    {
        int result = 0;
        if (player1.VoiceSource.isPlaying) result++;
        if (player2.VoiceSource.isPlaying) result++;
        if (player3.VoiceSource.isPlaying) result++;
        if (player4.VoiceSource.isPlaying) result++;
        return result;

    }

    private void addAudioSource(AudioSource source)
    {
        if (audioSources.Count >= 3)
        {

            if (audioSources[2] != null) { audioSources[2].Stop();
            }
        }
        audioSources[2] = audioSources[1];
        audioSources[1] = audioSources[0];
        audioSources[0] = source;
    }

    public void playSound(AudioSource audiosource)
    {
        audiosource.Play();
        addAudioSource(audiosource);
        StartCoroutine(deleteAudioSource(audiosource));
    }

    IEnumerator deleteAudioSource(AudioSource audioSource)
    {
        while (audioSource.isPlaying) { yield return new WaitForEndOfFrame(); }
        for(int i=0; i<3; i++)
        {
            if(audioSources[i] == audioSource)
            {
                switch (i)
                {
                    case 2:
                        audioSources[2] = null;
                        break;
                    case 1:
                        audioSources[1] = audioSources[2];
                        break;
                    case 0:
                        audioSources[0] = audioSources[1];
                        audioSources[1] = audioSources[2];
                        break;
                }
            }
        }
        yield return null;
    }

    public AudioClip GetPlayerDeathByCold()
    {
        int randomIndex = Mathf.CeilToInt(Random.Range(0.01f, listPlayerDeathByCold.Count))-1;
        return listPlayerDeathByCold[randomIndex];
    }
    public AudioClip GetPlayerDeathByHot()
    {
        int randomIndex = Mathf.CeilToInt(Random.Range(0.01f, listPlayerDeathByHot.Count)) - 1;
        return listPlayerDeathByHot[randomIndex];
    }
    public AudioClip GetPlayerWin()
    {
        int randomIndex = Mathf.CeilToInt(Random.Range(0.01f, listPlayerWin.Count)) - 1;
        return listPlayerWin[randomIndex];
    }
    public AudioClip GetPlayerPushed()
    {
        int randomIndex = Mathf.CeilToInt(Random.Range(0.01f, listPlayerPushed.Count)) - 1;
        return listPlayerPushed[randomIndex];
    }
    public AudioClip GetPlayerPush()
    {
        int randomIndex = Mathf.CeilToInt(Random.Range(0.01f, listPlayerPush.Count)) - 1;
        return listPlayerPush[randomIndex];
    }
    public AudioClip GetPlayerItemGet()
    {
        int randomIndex = Mathf.CeilToInt(Random.Range(0.01f, listPlayerItemGet.Count)) - 1;
        return listPlayerItemGet[randomIndex];
    }
    // Update is called once per frame
    void Update () {
	
	}
}
