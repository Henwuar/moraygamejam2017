using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterShader : MonoBehaviour
{
    public float rippleSpeed;
    private Material material;

	// Use this for initialization
	void Start ()
    {
        material = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update ()
    {
        material.SetFloat("_Timer", Time.time*rippleSpeed);	
	}
}
