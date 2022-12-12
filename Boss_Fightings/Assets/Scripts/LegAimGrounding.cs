using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegAimGrounding : MonoBehaviour
{
    private GameObject raycastOrigin;
    int layerMask;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Ground");
        raycastOrigin = transform.parent.gameObject;   
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(raycastOrigin.transform.position, -transform.up, out hit, Mathf.Infinity, layerMask))
        {
            transform.position = hit.point;
        }
    }
}
