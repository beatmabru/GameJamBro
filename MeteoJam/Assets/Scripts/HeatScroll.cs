using UnityEngine;
using System.Collections;

public class HeatScroll : MonoBehaviour
{
    private Material mat;

    public float scrollSpeed = 2.0f;
    public float force = 0.03f;

	// Use this for initialization
	void Start ()
    {
        mat = GetComponent<SpriteRenderer>().material;
	}
	
	// Update is called once per frame
	void Update ()
    {
        mat.SetFloat("_force", force);
        mat.SetTextureOffset("_texture", new Vector2((Time.time * scrollSpeed) * Time.deltaTime, 1));
    }
}
