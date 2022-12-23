using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HittedObject : MonoBehaviour {

    public float startHealth = 100;
    public Animator anim;
    private float health;
    public Image healthBar;
	// Use this for initialization
	void Start () {
        health = startHealth;
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(float amount)
    {
        health -= amount;
        if(healthBar != null) healthBar.fillAmount = health / startHealth;

        anim.SetTrigger("Take Damage");

        /*
        if(health <= 0)
        {
            Destroy(gameObject);
        }
        */
    }
}
