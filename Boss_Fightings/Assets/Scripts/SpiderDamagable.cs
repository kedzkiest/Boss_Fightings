using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderDamagable : MonoBehaviour
{
    [SerializeField] int HP = 30;
    [SerializeField] ParticleSystem ps;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bullet")) return;

        other.enabled = false;
        Destroy(other.gameObject);
        HP--;

        ps.transform.position = transform.position;
        ps.Play();
        Invoke(nameof(StopParticle), 0.3f);
    }

    private void StopParticle()
    {
        ps.Stop();
    }
}
