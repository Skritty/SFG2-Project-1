using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRen : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.DrawMesh(GetComponent<MeshFilter>().sharedMesh);
    }
}
