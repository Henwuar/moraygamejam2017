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
}
