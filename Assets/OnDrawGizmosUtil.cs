using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDrawGizmosUtil : MonoBehaviour
{
    [SerializeField] float sphereRadius = 1f;
    [SerializeField] Color gizmoColor = Color.yellow;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, sphereRadius);
    }
}
