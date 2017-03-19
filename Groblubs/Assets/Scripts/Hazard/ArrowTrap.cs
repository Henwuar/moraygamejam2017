using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ArrowTrap : PressurePlateReceiver
{
    public GameObject arrow;
    public Vector3 direction;

    private GameObject curArrow = null;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + direction);
    }

    public override void Trigger()
    {
        if (!curArrow)
        {
            curArrow = Instantiate(arrow, transform.position, Quaternion.LookRotation(direction));
            curArrow.GetComponent<Rigidbody>().AddForce(direction * 2000.0f);
        }
    }


}
