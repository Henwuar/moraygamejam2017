using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : PressurePlateReceiver
{
    public float moveSpeed;
    public float maxDistance;

    private Rigidbody body;
    private bool open = false;

    private Vector3 closedPos;
    private Vector3 openPos;
    private Vector3 targetPos;

    // Use this for initialization
    void Start ()
    {
        closedPos = transform.position;
        openPos = transform.GetChild(0).position;
        body = GetComponent<Rigidbody>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(open)
        {
            targetPos = openPos;
        }
        else
        {
            targetPos = closedPos;
        }
	}

    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, targetPos) > maxDistance)
        {
            body.AddForce((targetPos - transform.position) * moveSpeed);
        }
        else
        {
            transform.position = targetPos;
            body.velocity = Vector3.zero;
        }
    }

    public override void Trigger()
    {
        open = !open;

    }
}
