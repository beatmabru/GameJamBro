using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    [SerializeField]
    private Text _clothCount;

	// Update is called once per frame
	public void UpdateHud (int clothes)
    {
        _clothCount.text = clothes + "";
	}

}
