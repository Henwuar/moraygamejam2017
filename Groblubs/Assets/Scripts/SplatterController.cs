using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatterController : MonoBehaviour
{
    public float lifetime;
    public float fadeSpeed;

    private Vector3 originalPos;

	// Use this for initialization
	void Start ()
    {
        originalPos = transform.position;	
	}
	
	// Update is called once per frame
	void Update ()
    {
        lifetime -= Time.deltaTime;
        if(lifetime <= 0)
        {
            transform.position -= transform.forward * fadeSpeed * Time.deltaTime;
            if(Vector3.Distance(transform.position, originalPos) > 1)
            {
                Destroy(gameObject);
            }
        }
	}
}
