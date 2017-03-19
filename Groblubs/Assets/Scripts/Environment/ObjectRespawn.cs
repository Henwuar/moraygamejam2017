using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRespawn : MonoBehaviour
{
    private Vector3 respawnPos;
    private Quaternion respawnRot;

	// Use this for initialization
	void Start ()
    {
        respawnPos = transform.position;
        respawnRot = transform.rotation;
	}
	
    public void Respawn()
    {
        transform.position = respawnPos;
        transform.rotation = respawnRot;
    }

	void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Killzone")
        {
            Respawn();
        }
    }
}
