using UnityEngine;
using System.Collections;

public class ParticleAnimator : MonoBehaviour
{
    private ParticleSystem particle;

    public float maxParticle;
    public Vector2 speed;
    public Vector2 size;

    // Use this for initialization
    void Start () {
        particle = GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        particle.maxParticles = Mathf.FloorToInt(maxParticle);
        particle.startSpeed = Random.Range(speed.x, speed.y);
        particle.startSize = Random.Range(size.x, size.y);
    }
}
