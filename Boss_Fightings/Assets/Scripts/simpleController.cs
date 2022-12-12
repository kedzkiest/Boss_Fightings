using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleController : MonoBehaviour
{
    [SerializeField] Rigidbody cntroller;

    [SerializeField] float speed = 6;
    private Vector3 velocity;

    private float turnSmoothVelocity;
    [SerializeField] float turnSmoothTime = 0.1f;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if(direction.magnitude >= 0.1f && cntroller.velocity.magnitude < 4)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            cntroller.AddForce(moveDir.normalized * speed * Time.deltaTime);
        }
    }
}
