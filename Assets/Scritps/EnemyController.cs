using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health=3;
    public GameObject explosion;
    public float playerRange = 10f;
    public Rigidbody2D theRB;
    public float moveSpeed;
    public Animator demonAnim;

    public bool shouldshoot;
    public float fireRate = .7f;
    private float shotCounter;
    public GameObject bullet;
    public Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, PlayerController.instance.transform.position) < playerRange)
        {
            Vector3 playerDirection = PlayerController.instance.transform.position - transform.position;

            theRB.velocity = playerDirection * moveSpeed; 

            demonAnim.SetBool("demon_run", true);

            if(shouldshoot)
            {
                shotCounter -= Time.deltaTime;
                if(shotCounter <= 0)
                {
                    Instantiate(bullet, firePoint.position, firePoint.rotation);
                    shotCounter = fireRate;
                }
            }
        }else
        {
            theRB.velocity = Vector2.zero;
            demonAnim.SetBool("demon_run", false);
        }
    }

    public void TakeDamage()
    {
        health--;
        if(health <= 0)
        {
            Destroy(gameObject);
            Instantiate(explosion, transform.position, transform.rotation);
            PlayerController.instance.scoore += 1;
        }
    }
}
