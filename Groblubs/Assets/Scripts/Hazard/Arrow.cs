using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float aliveTime;
    public int numHits;

    private Rigidbody body;


	// Use this for initialization
	void Start ()
    {
        body = GetComponent<Rigidbody>();	
	}
	
    void Update()
    {
        if(numHits <= 0)
        {
            aliveTime -= Time.deltaTime;
            if (aliveTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

	// Update is called once per frame
	void FixedUpdate ()
    {
        if (numHits > 0)
        {
            transform.LookAt(transform.position + body.velocity);
        }
	}

    void OnCollisionEnter()
    {
        numHits--;
    }

}
