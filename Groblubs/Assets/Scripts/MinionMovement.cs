using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MinionMovement : MonoBehaviour
{
    public int player;
    public float movementForce;

    private Rigidbody body;
    private Vector3 forceDirection;
    private string inputAxis;

	// Use this for initialization
	void Start ()
    {
        body = GetComponent<Rigidbody>();
        body.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY;
        //body.constraints = RigidbodyConstraints

        if (player == 0)
        {
            inputAxis = "LeftStick";          
        }
        else
        {
            inputAxis = "RightStick";
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        GetInput();
	}

    void FixedUpdate()
    {
        body.AddForce(forceDirection * movementForce);
    }

    void GetInput()
    {
        float xVal = Input.GetAxis(inputAxis + "X");

        forceDirection.x = xVal;
    }
}
