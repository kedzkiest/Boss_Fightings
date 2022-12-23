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
    private bool canControll;

    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        canControll = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!canControll) return;

        Rotate();
        Move();
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

        if (Input.GetKey(KeyCode.Alpha0))
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }

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

    public void SetCanControllState(bool isControllable)
    {
        canControll = isControllable;
    }
}
