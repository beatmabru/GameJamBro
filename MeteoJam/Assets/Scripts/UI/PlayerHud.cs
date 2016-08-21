﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    [SerializeField]
    private Text _clothCount;
    [SerializeField]
    private Image _gauge;

    private WeatherVariation _weather;
    private Forecaster _forecast;

    void Awake()
    {
        GameObject variationHolder = GameObject.Find("WeatherManager");
        Debug.Assert(variationHolder != null);
        _weather = variationHolder.GetComponent<WeatherVariation>();
        _forecast = variationHolder.GetComponent<Forecaster>();
    }

    // Update is called once per frame
    public void UpdateHud (int clothes, float lifePoints)
    {
        if(_weather.state==WeatherVariation.State.WEATHER)
        {
            _clothCount.text = clothes + " / " + (int)_weather.weatherIndex;
        }
        else
        {
            _clothCount.text = clothes + " / " + +(int)_forecast.forecast + "?";
        }

        float newScaleX = lifePoints / 100  ;
        _gauge.transform.localScale = new Vector3(newScaleX, 1, 1);
    }

    //lifebar alpha
    private static readonly byte alphaLifeBar = 255;
    private static readonly Color32 lifeUp = new Color32(151, 253, 35, alphaLifeBar);
    private static readonly Color32 respite = new Color32(180, 220, 180, alphaLifeBar);
    private static readonly Color32 tooHot = new Color32(211, 47, 47, alphaLifeBar);
    private static readonly Color32 tooCold = new Color32(25, 118, 210, alphaLifeBar);
    // private static readonly byte alphaLifeBar = 200;

    public void changeColor(int clothes)
    {
        if (_weather.state == WeatherVariation.State.RESPITE)
        {
            _gauge.GetComponent<Image>().color = respite;
            return;
        }

        WeatherVariation.WeatherIndex index = _weather.weatherIndex;
        Color32 color;

        if (WeatherVariation.WeatherIndex.PERFECT == index)
        {
            if(clothes >= 1 && clothes <= 5)
                color = lifeUp;
            else
                color = respite;
        }
        else
        {
            if (clothes < (int)index)
                color = tooCold;
            else if (clothes == (int)index)
                color = respite;
            else
                color = tooHot;
        }
        
        _gauge.GetComponent<Image>().color = color;
    }

}
