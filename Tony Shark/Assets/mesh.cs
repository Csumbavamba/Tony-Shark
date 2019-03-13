using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mesh : MonoBehaviour
{
    // Start is called before the first frame update

    MeshCollider c;
    SkinnedMeshRenderer m;
    
    void Start()
    {
        c = GetComponent<MeshCollider>();
        m = GetComponent<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        c.enabled = false;
        c.sharedMesh = m.sharedMesh;
        c.enabled = true;
    }
}
