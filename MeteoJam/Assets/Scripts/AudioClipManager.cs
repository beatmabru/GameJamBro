using UnityEngine;
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

    // Use this for initialization

    void Awake()
    {
        instance = this;
        narratorAudioClip = Resources.LoadAll<AudioClip>("Sound/narrator");
        playerAudioClip = Resources.LoadAll<AudioClip>("Sound/player");
        sfxAudioClip = Resources.LoadAll<AudioClip>("Sound/sfx");

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
    }

    void Start () {
        
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
