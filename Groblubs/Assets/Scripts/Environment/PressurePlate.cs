using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public PressurePlateReceiver connectedObject;

	void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Minion")
        {
            connectedObject.Trigger();
        }
    }
}
