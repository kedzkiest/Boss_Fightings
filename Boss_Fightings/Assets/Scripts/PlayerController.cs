using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed_move;
    [SerializeField] private float speed_rot;

    private Animator anim;
    private CharacterController characterController;
    private bool grounded;

    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        JudgeGrounded();
        Rotate();
        Move();
    }

    private void JudgeGrounded()
    {

    }

    private void Rotate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal == 0 && vertical == 0) return;

        Quaternion rot = Quaternion.LookRotation(new Vector3(horizontal, 0, vertical));
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, speed_rot * Time.deltaTime);
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        characterController.Move(new Vector3(horizontal, Physics.gravity.y, vertical) * speed_move * Time.deltaTime);

        if(horizontal != 0 || vertical != 0)
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }
    }
}
