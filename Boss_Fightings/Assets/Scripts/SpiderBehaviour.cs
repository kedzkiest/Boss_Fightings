using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class SpiderBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private List<GameObject> waypoints = new List<GameObject>();
    [SerializeField] private GameObject player;
    [Space]
    [SerializeField] private bool chasePlayer = false;
    [SerializeField] private float moveSpeed = 1000f;
    [SerializeField] private float rotateSpeed = 3f;
    [SerializeField] private float howCloseCanReachTarget = 0.3f;

    private int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Move();
    }

    private void Rotate()
    {
        Vector3 direction = waypoints.ElementAt(currentIndex).transform.position - transform.position;
        Quaternion rot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotateSpeed * Time.deltaTime);
    }

    private void Move()
    {
        Vector3 direction;
        if (chasePlayer)
        {
            direction = player.transform.position - transform.position;
        }
        else
        {
            direction = waypoints.ElementAt(currentIndex).transform.position - transform.position;
        }

        if(!chasePlayer && direction.magnitude < howCloseCanReachTarget)
        {
            rb.velocity = Vector3.zero;
            currentIndex = currentIndex+1 <  waypoints.Count ? currentIndex+1 : 0;
            return;
        }

        rb.AddForce(direction.normalized * Time.deltaTime * moveSpeed);
    }
}
