using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{	
	void OnTriggerEnter(Collider other)
    {
        if(other.tag == "EndGoal")
        {
            other.GetComponentInParent<EndGoalController>().StartCoroutine("BulgeForward");
            Destroy(gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Water")
        {
            
            float distance = other.transform.parent.position.y - transform.position.y;
            float maxDistance = other.transform.localScale.y;
            GetComponent<Rigidbody>().AddForce(-Physics.gravity * (1 + (distance / maxDistance)));
            GetComponent<Rigidbody>().angularDrag = 10.0f;
            /*if(distance/maxDistance < 0.1f)

            {

            }*/
        }

    }
}
