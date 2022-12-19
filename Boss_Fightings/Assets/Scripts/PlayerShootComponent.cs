using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootComponent : MonoBehaviour
{
    [SerializeField] private float cooldownOnShoot = 1.0f;

    private PlayerController playerController;
    private Animator anim;
    private float elapsedTime;
    private bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        elapsedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            anim.SetBool("Shoot", true);
            isShooting = true;
            playerController.SetCanMoveState(false);
            elapsedTime = 0;
        }
        else
        {
            anim.SetBool("Shoot", false);
        }
        
        
        if (isShooting && elapsedTime >= cooldownOnShoot)
        {
            isShooting = false;
            playerController.SetCanMoveState(true);
        }
        
    }
}
