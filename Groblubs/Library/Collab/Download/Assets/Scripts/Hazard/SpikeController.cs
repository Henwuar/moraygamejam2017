using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour
{
    public float changeTime;
    public float moveSpeed;
    public float maxDistance;

    private float timer;
    private bool state = false;
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private Rigidbody body;

    private Vector3 position1;
    private Vector3 position2;

	// Use this for initialization
	void Start ()
    {
        timer = changeTime;
        originalPosition = transform.position;
        body = GetComponent<Rigidbody>();

        position1 = transform.Find("Start").position;// + transform.position;
        position2 = transform.Find("End").position;// + transform.position;
        targetPosition = position1;
    }
	
	// Update is called once per frame
	void Update ()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = changeTime;
            state = !state;
            print(state);
            if (state)
            {
                targetPosition = position1;
            }
            else
            {
                targetPosition = position2;
            }
        }

       
	}

    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, targetPosition) > maxDistance)
        {
            body.AddForce((targetPosition - transform.position) * moveSpeed);
        }
        else
        {
            transform.position = targetPosition;
            body.velocity = Vector3.zero;
        }
    }

    public void Attach(GameObject attachedObject)
    {
        Rigidbody otherBody = attachedObject.GetComponent<Rigidbody>();
        if(otherBody)
        {
            SpringJoint newJoint = gameObject.AddComponent<SpringJoint>();
            newJoint.connectedBody = otherBody;
            newJoint.breakForce = moveSpeed;
            newJoint.spring = 1000.0f;  
        }
    }
}
