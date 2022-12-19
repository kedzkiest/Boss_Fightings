using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootComponent : MonoBehaviour
{
    [SerializeField] private float cooldownOnShoot = 1.0f;
    [SerializeField] private float bulletSpeed = 1.0f;
    [SerializeField] private GameObject nozzle;

    private PlayerController playerController;
    private Animator anim;
    private float elapsedTime;
    private bool canShoot;
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

        PlayShootMotion();
        ShootProcess();

        StopShooting();
    }

    private void PlayShootMotion()
    {
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
    }

    private void ShootProcess()
    {
        string currentAnimName = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        if (currentAnimName != "Pistol_Attack_Fire")
        {
            canShoot = true;
            return;
        }

        if (!canShoot) return;
 
        GameObject bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bullet.transform.position = nozzle.transform.position;
        bullet.transform.localScale *= 0.2f;
        bullet.tag = "Bullet";

        bullet.AddComponent<Rigidbody>();
        bullet.AddComponent<SphereCollider>();
        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * Time.deltaTime * bulletSpeed, ForceMode.Impulse);

        bullet.AddComponent<TrailRenderer>();
        TrailRenderer tr = bullet.GetComponent<TrailRenderer>();
        tr.startWidth = 0.2f;
        tr.endWidth = 0.2f;        

        canShoot = false;
        Destroy(bullet, 1.0f);
    }

    private void StopShooting()
    {
        if (isShooting && elapsedTime >= cooldownOnShoot)
        {
            isShooting = false;
            playerController.SetCanMoveState(true);
        }
    }
}
