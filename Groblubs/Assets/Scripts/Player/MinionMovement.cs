using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MinionMovement : MonoBehaviour
{
    public int player;
    public float movementForce;
    public float jumpForce;
    public float maxSpeed;
    public float dieSpeed;
    public float turnSpeed;
    public float grabDistance;
    public float spawnTimer;

    private Rigidbody body;
    private Vector3 forceDirection;
    private string inputAxis;
    private bool onGround = false;
    private bool controlled = true;
    private SpringJoint grabJoint = null;
    private Quaternion targetRotation;
    private bool spawned = false;
    private Animator animator;
    private Transform projector;

	// Use this for initialization
	void Start ()
    {
        body = GetComponent<Rigidbody>();
        body.constraints = RigidbodyConstraints.FreezePositionZ;
        body.freezeRotation = true;
        targetRotation = transform.rotation;
        projector = transform.FindChild("Shadow");


        if (player == 1)
        {
            inputAxis = "LeftStick";          
        }
        else
        {
            inputAxis = "RightStick";
        }

        animator = GetComponentInChildren<Animator>();
	}

    void OnAwake()
    {
        Start();
        body.AddForce(Vector3.right * movementForce * 2);
    }
	
	// Update is called once per frame
	void Update ()
    {
        projector.position = transform.position + Vector3.up * 2;
        projector.LookAt(transform.position-Vector3.up);
        if(!spawned)
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                spawned = true;
            }
        }
        if (controlled && spawned)
        {
            GetInput();
            if(Quaternion.Angle(transform.rotation, targetRotation) > turnSpeed * Time.deltaTime)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime); 
            }
            else
            {
                transform.rotation = targetRotation;
            }

            animator.SetFloat("MoveSpeed", (body.velocity.x));
            animator.SetBool("OnGround", onGround);

            if(forceDirection.x == 0)
            {
                animator.SetBool("Still", true);
            }
            else
            {
                animator.SetBool("Still", false);
            }

            if (grabJoint)
            {
                animator.SetBool("Grabbing", true);
            }
            else
            {
                animator.SetBool("Grabbing", false);
            }
            
        }

    }

    void FixedUpdate()
    {
        if(spawned)
        {
            body.AddForce(forceDirection);

            float xSpeed = Mathf.Clamp(body.velocity.x, -maxSpeed, maxSpeed);
            body.velocity = new Vector3(xSpeed, body.velocity.y, body.velocity.z);
            

            RaycastHit hit;
            if (Physics.SphereCast(transform.position, GetComponent<CapsuleCollider>().radius, -Vector3.up, out hit, 0.51f))
            {
                if (hit.rigidbody && grabJoint)
                {
                    if (hit.rigidbody == grabJoint.connectedBody)
                    {
                        onGround = false;
                    }
                    else
                    {
                        onGround = true;
                    }
                }
                else
                {
                    onGround = true;
                }
            }
            else
            {
                onGround = false;
            }

            Vector3 dir = new Vector3(forceDirection.x, 0.0f);
            if(Physics.CapsuleCast(transform.position + Vector3.up*0.5f, transform.position - Vector3.up*0.5f, GetComponent<CapsuleCollider>().radius, forceDirection, out hit, 0.1f))
            {
                if(hit.collider.tag == "Environment")
                {
                    body.AddForce(-dir*2);
                }
            }
        }

        if(body.velocity.y < -5.0f && !grabJoint)
        {
            targetRotation = Quaternion.Euler(90*Mathf.Sign(forceDirection.x), 90, 0.0f);
        }
    }

    void GetInput()
    {
        float xVal = 0.0f;
        xVal = Input.GetAxis(inputAxis + "X") * movementForce;
        if(xVal == 0)
        {
            body.velocity = new Vector3(body.velocity.x * 0.5f, body.velocity.y);
        }
        else if(grabJoint == null)
        {
            //transform.LookAt(transform.position + new Vector3(xVal, 0));
            targetRotation = Quaternion.LookRotation(new Vector3(xVal, 0.0f, -0.1f), Vector3.up);
        }


        forceDirection = new Vector3(xVal, 0.0f);

        if (Input.GetButton("Jump" + player.ToString()) && onGround && body.velocity.y <= 0)
        {
            forceDirection.y = jumpForce;
            onGround = false;
        }
        
        if(Input.GetAxis("Grab" + player.ToString()) != 0)
        {
            if (grabJoint == null)
            {
                RaycastHit hit;
                if (Physics.SphereCast(transform.position, 0.25f, transform.forward, out hit, grabDistance))
                {
                    if(hit.rigidbody)
                    {        
                        grabJoint = gameObject.AddComponent<SpringJoint>();
                        grabJoint.connectedBody = hit.rigidbody;
                        grabJoint.maxDistance = 0.1f;
                        grabJoint.spring = 1000.0f;
                        grabJoint.enableCollision = true;
                    }
                }
            }
        }
        else
        {
            if(grabJoint)
            {
                Destroy(grabJoint);
            }
        }
    }

    public void Die(GameObject killer = null, bool bloody = true)
    {
        //Instantiate(gameObject, GameObject.Find("Spawn").transform.position, Quaternion.identity);
        controlled = false;
        GetComponent<Renderer>().material.color = Color.red;
        body.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
        forceDirection = Vector3.zero;
        //body.AddTorque(new Vector3(0.0f, 0.0f, Random.Range(-10.0f, 10.0f)));
        //GetComponent<CapsuleCollider>().material = deadMinionPhysics;
        //GameObject.Find("Spawn").GetComponent<MinionSpawner>().SpawnMinion(player);
        gameObject.layer = LayerMask.NameToLayer("Body");
        body.mass = 1.0f;
        animator.SetBool("Dead", true);
        projector.gameObject.SetActive(false);

        Transform blood = transform.FindChild("Blood");
        if (blood)
        {
            if (killer)
            {
                blood.LookAt(killer.transform.position);
            }
            else
            {
                blood.LookAt(transform.position + Vector3.up);
            }
            if (bloody)
            {
                blood.GetComponent<ParticleSystem>().Play();
            }
            blood.SetParent(null);
        }
        if (grabJoint)
        {
            Destroy(grabJoint);
        }

    }

    void OnCollisionEnter(Collision col)
    {
        if (controlled)
        {
            if (col.relativeVelocity.magnitude > dieSpeed)
            {
                Die();
            }
            if(col.gameObject.tag == "Hazard")
            {
                Die(col.gameObject);
                if(col.gameObject.layer == LayerMask.NameToLayer("Spike"))
                { 
                    col.gameObject.GetComponentInParent<SpikeController>().Attach(gameObject);
                }
            }
            if(col.gameObject.tag == "Killzone")
            {
                
                Destroy(gameObject);
            }
        }
        else
        {
            if (col.gameObject.tag == "Killzone")
            {
                Destroy(gameObject);
            }
            if(col.gameObject.tag == "Hazard")
            {
                if(col.gameObject.layer == LayerMask.NameToLayer("Spike"))
                {
                    col.gameObject.GetComponentInParent<SpikeController>().Attach(gameObject);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (controlled)
        {
            if (other.gameObject.tag == "Hazard")
            {
                Die(other.gameObject);
            }
        }
        else
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Staple"))
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Water")
        {
            if(controlled)
            {
                Die(null, false);
            }
            float distance = other.transform.parent.position.y - transform.position.y;
            float maxDistance = other.transform.localScale.y;
            body.AddForce(-Physics.gravity*(1 + (distance / maxDistance)));
            body.angularDrag = 10.0f;
            /*if(distance/maxDistance < 0.1f)

            {

            }*/
        }

    }

    public bool Controlled()
    {
        return controlled;
    }
}
