using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StapleRemoverBottom : MonoBehaviour
{
    public float turnSpeed;
    public float jumpForce;
    public float jumpTime;


    private Transform top;
    private Rigidbody body;
    private Quaternion targetRotation;
    private float jumpTimer;
    private Vector3 curDir;
    private bool onGround;

	// Use this for initialization
	void Start ()
    {
        body = GetComponent<Rigidbody>();
        curDir = transform.forward;
        jumpTimer = jumpTime;
        top = transform.Find("Top");
        //body.centerOfMass = Vector3.forward;
	}
	
    void Update()
    {
        //if(onGround)
        {
            jumpTimer -= Time.deltaTime;
            if (jumpTimer <= 0)
            {

                if (Jump())
                {
                    jumpTimer = jumpTime;
                }
            }
        }
        
    }

	// Update is called once per frame
	void FixedUpdate ()
    {

        //targetRotation = Quaternion.LookRotation(curDir, Vector3.up);
        //transform.parent.rotation = targetRotation;
        if (body.velocity.y < 0)
        {
            targetRotation = Quaternion.LookRotation(curDir, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            if (Quaternion.Angle(transform.rotation, targetRotation) < turnSpeed * Time.deltaTime)
            {
                transform.rotation = targetRotation;
            }
        }
        if (Physics.Raycast(transform.position, -transform.up, 0.5f))
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, curDir, out hit, 2.1f, LayerMask.NameToLayer("Staple")))
        {
            print(hit.collider.tag);
            if (hit.collider.tag != "Minion")
            {
                curDir = new Vector3(hit.normal.x, 0.0f);
                transform.rotation = Quaternion.LookRotation(curDir);
            }
        }
        //if(Phys)
    }

    bool Jump()
    {
        
        if (onGround)
        {
            
            body.AddForce(curDir * jumpForce + Vector3.up * jumpForce);
            return true;
        }
        return false;

    }
}
