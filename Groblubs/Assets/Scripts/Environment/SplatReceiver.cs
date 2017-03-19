using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatReceiver : MonoBehaviour
{
    public GameObject splatterObject;

    void OnParticleCollision(GameObject other)
    {
        Instantiate(splatterObject, other.transform.position + Vector3.up, Quaternion.identity);
    }
}
