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

}
