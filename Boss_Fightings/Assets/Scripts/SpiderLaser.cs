using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using System;
using UnityEngine;

public class SpiderLaser : MonoBehaviour
{
    public GameObject target;

    public GameObject FirePoint;
    public float MaxLength;
    public GameObject[] Prefabs;

    public bool fire;
    public bool stop;

    private Vector3 direction;
    private Quaternion rotation;

    private int Prefab;
    private GameObject Instance;
    private Hovl_Laser LaserScript;
    private Hovl_Laser2 LaserScript2;

    void Start ()
    {

    }

    void Update()
    {
        //Enable lazer
        if (fire)
        {
            fire = false;

            Destroy(Instance);
            Instance = Instantiate(Prefabs[Prefab], FirePoint.transform.position, FirePoint.transform.rotation);
            Instance.transform.parent = transform;
            LaserScript = Instance.GetComponent<Hovl_Laser>();
            LaserScript2 = Instance.GetComponent<Hovl_Laser2>();
        }

        //Disable lazer prefab
        if (stop)
        {
            stop = false;

            if (LaserScript) LaserScript.DisablePrepare();
            if (LaserScript2) LaserScript2.DisablePrepare();
            Destroy(Instance,1);
        }

        RaycastHit hit; //DELATE THIS IF YOU WANT TO USE LASERS IN 2D
        //ADD THIS IF YOU WANT TO USE LASERS IN 2D: RaycastHit2D hit = Physics2D.Raycast(RayMouse.origin, RayMouse.direction, MaxLength);
        if (Physics.Raycast(transform.position, target.transform.position - transform.position, out hit, MaxLength)) //CHANGE THIS IF YOU WANT TO USE LASERRS IN 2D: if (hit.collider != null)
        {
            RotateToMouseDirection(gameObject, hit.point);
            //LaserEndPoint = hit.point;
        }
        else
        {
            RotateToMouseDirection(gameObject, target.transform.position - transform.position);
            //LaserEndPoint = pos;
        }

    }
  
    //To rotate fire point
    void RotateToMouseDirection (GameObject obj, Vector3 destination)
    {
        direction = destination - obj.transform.position;
        rotation = Quaternion.LookRotation(direction);     
        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
    }
}
