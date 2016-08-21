using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    [SerializeField]
    private Text _clothCount;
    [SerializeField]
    private Image _gauge;

    private WeatherManager2 _weather;
    private Forecaster _forecast;

    void Awake()
    {
        GameObject variationHolder = GameObject.Find("WeatherManager2");
        Debug.Assert(variationHolder != null);
        _weather = variationHolder.GetComponent<WeatherManager2>();
        _forecast = variationHolder.GetComponent<Forecaster>();
    }

    // Update is called once per frame
    public void UpdateHud (int clothes, float lifePoints)
    {
        //if(_weather.currentWeather == WeatherVariation.State.WEATHER)
        //{
        _clothCount.text = clothes + " / " + (int)_weather.currentWeather;
        /*}
        else
        {
            _clothCount.text = clothes + " / " + +(int)_forecast.forecast + "?";
        }*/

        float newScaleX = lifePoints / 100  ;
        _gauge.transform.localScale = new Vector3(newScaleX, 1, 1);
    }

    public void changeColor(int clothes)
    {
        WeatherManager2.WeatherIndex index = _weather.currentWeather;
        Color32 color;

        if (index == WeatherManager2.WeatherIndex.PERFECT)
        {
            if(clothes >= 1 && clothes <= 5)
                color = GameManager.instance.lifeUp;
            else
                color = GameManager.instance.lifeNeutral;
        }
        else
        {
            if (clothes < (int)index)
                color = GameManager.instance.lifeTooCold;
            else if (clothes == (int)index)
                color = GameManager.instance.lifeNeutral;
            else
                color = GameManager.instance.lifeTooHot;
        }
        
        _gauge.GetComponent<Image>().color = Color.Lerp(_gauge.GetComponent<Image>().color, color,5*Time.deltaTime);
    }
}
