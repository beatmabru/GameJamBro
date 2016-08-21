using UnityEngine;
using System.Collections;

public class MoveHudToPlayer : MonoBehaviour
{
    private RectTransform _rectTransform;
    private RectTransform _canvas;
    public Camera mainCamera;
    public Transform playerTarget;
    public float damp = 5f;

    // Use this for initialization
    void Start ()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 viewPos = mainCamera.WorldToViewportPoint(playerTarget.position);
        Vector2 newPos = new Vector2(viewPos.x * _canvas.rect.width, viewPos.y * _canvas.rect.height + 30f);
        _rectTransform.anchoredPosition = Vector2.Lerp(_rectTransform.anchoredPosition, newPos, damp * Time.deltaTime);
    }
}
