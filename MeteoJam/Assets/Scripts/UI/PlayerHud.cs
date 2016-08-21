using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    [SerializeField]
    private Text _clothCount;
    [SerializeField]
    private Image _gauge;

    // Update is called once per frame
    public void UpdateHud (int clothes, float lifePoints)
    {
        _clothCount.text = clothes + " / " +(int) WeatherVariation.instance.weatherIndex;
        float newScaleX = lifePoints / 100  ;
        _gauge.transform.localScale = new Vector3(newScaleX, 1, 1);
    }

    //lifebar alpha
    private static readonly byte alphaLifeBar = 255;
    private static readonly Color32 lifeUp = new Color32(139, 195, 74, alphaLifeBar);
    private static readonly Color32 lifeStill = new Color32(255, 255, 255, alphaLifeBar);
    private static readonly Color32 respite = new Color32(180, 180, 180, alphaLifeBar);
    private static readonly Color32 tooHot = new Color32(211, 47, 47, alphaLifeBar);
    private static readonly Color32 tooCold = new Color32(25, 118, 210, alphaLifeBar);
    // private static readonly byte alphaLifeBar = 200;

    public void changeColor(int clothes)
    {
        if (WeatherVariation.instance.state == WeatherVariation.State.RESPITE)
        {
            _gauge.GetComponent<Image>().color = respite;
            return;
        }

        WeatherVariation.WeatherIndex index = WeatherVariation.instance.weatherIndex;
        Color32 color;

        if (WeatherVariation.WeatherIndex.PERFECT == index)
        {
            if(clothes >= 1 && clothes <= 5)
                color = lifeUp;
            else
                color = lifeStill;
        }
        else
        {
            if (clothes < (int)index)
                color = tooCold;
            else if (clothes == (int)index)
                color = lifeStill;
            else
                color = tooHot;
        }
        
        _gauge.GetComponent<Image>().color = color;
    }

}
