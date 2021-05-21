using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoTest : MonoBehaviour
{
    private float distance = 1;
    public bool drawSphere = true;
    public bool drawWireSphere = true;
    public bool drawWireCube = true;
    public bool drawRay = true;

    private void OnDrawGizmosSelected()
    {
        if (drawSphere)
            Gizmos.DrawSphere(transform.position, distance);
        if (drawWireSphere)
            Gizmos.DrawWireSphere(transform.position, distance);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        if (drawWireCube)
            //Vector3.one = new Vector3(1, 1, 1)
            Gizmos.DrawWireCube(transform.position, Vector3.one * distance);
        if (drawRay)
            Gizmos.DrawRay(transform.position, Vector3.one * distance);
    }
}
