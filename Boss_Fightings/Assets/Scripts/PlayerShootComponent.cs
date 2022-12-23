using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootComponent : MonoBehaviour
{
    [SerializeField] private float cooldownOnShoot = 1.0f;
    [SerializeField] private float bulletSpeed = 1.0f;
    [SerializeField] private GameObject nozzle;
    [Space]
    [SerializeField] private Camera cam;
    [SerializeField] private float rayLength;

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
            playerController.SetCanControllState(false);
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

        Vector3 bulletDest;
        if (cam != null)
        {
            RaycastHit hit; //DELATE THIS IF YOU WANT TO USE LASERS IN 2D
            var mousePos = Input.mousePosition;
            Ray RayMouse = cam.ScreenPointToRay(mousePos);
            //ADD THIS IF YOU WANT TO USE LASERS IN 2D: RaycastHit2D hit = Physics2D.Raycast(RayMouse.origin, RayMouse.direction, MaxLength);
            if (Physics.Raycast(RayMouse.origin, RayMouse.direction, out hit, rayLength)) //CHANGE THIS IF YOU WANT TO USE LASERRS IN 2D: if (hit.collider != null)
            {
                RotateToMouseDirection(gameObject, hit.point);
                bulletDest = hit.point;
                //LaserEndPoint = hit.point;
            }
            else
            {
                var pos = RayMouse.GetPoint(rayLength);
                RotateToMouseDirection(gameObject, pos);
                bulletDest = pos;
                //LaserEndPoint = pos;
            }
        }
        else
        {
            Debug.Log("No camera");
            bulletDest = transform.forward;
        }

        GameObject bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bullet.transform.position = nozzle.transform.position;
        bullet.transform.localScale *= 0.2f;
        bullet.tag = "Bullet";

        SphereCollider sc = bullet.GetComponent<SphereCollider>();
        sc.isTrigger = true;
        sc.radius = 3;
        //Debug.DrawRay(nozzle.transform.position, bulletDest - nozzle.transform.position, Color.red, 1);
        bullet.AddComponent<Rigidbody>();
        bullet.GetComponent<Rigidbody>().AddForce((bulletDest-nozzle.transform.position) * Time.deltaTime * bulletSpeed, ForceMode.Impulse);

        
        bullet.AddComponent<TrailRenderer>();
        TrailRenderer tr = bullet.GetComponent<TrailRenderer>();
        tr.time = 0.1f;
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
            playerController.SetCanControllState(true);
        }
    }

    //To rotate fire point
    void RotateToMouseDirection(GameObject obj, Vector3 destination)
    {
        Vector3 direction = destination - obj.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
        obj.transform.localRotation = Quaternion.Euler(0f, obj.transform.localRotation.eulerAngles.y, 0f);
    }
}
