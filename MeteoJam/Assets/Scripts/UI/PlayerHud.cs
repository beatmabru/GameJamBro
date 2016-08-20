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
        _clothCount.text = clothes + " - " +(int) WeatherVariation.instance.weatherIndex;
        float newScaleX = lifePoints / 100  ;
        _gauge.transform.localScale = new Vector3(newScaleX, 1, 1);
    }

    public void changeColor(int clothes)
    {
        WeatherVariation.WeatherIndex index = WeatherVariation.instance.weatherIndex;
        Color32 color;
        if (WeatherVariation.WeatherIndex.PERFECT == index)
        {
            if(clothes >= 1 && clothes <= 5)
                //life up
                color = new Color32(139, 195, 74, 180);
            else
                //stable
                color = new Color32(255, 255, 255, 180);
        }
        else
        {
            if (clothes < (int)index)
            {
                //trop froid
                color = new Color32(25, 118, 210, 180);
            }
            else if (clothes == (int)index)
            {
                //stable
                color = new Color32(255, 255, 255, 180);
            }
            else
            {
                //trop chaud
                color = new Color32(211, 47, 47, 180);
            }
        }
        
        _gauge.GetComponent<Image>().color = color;
    }

}
