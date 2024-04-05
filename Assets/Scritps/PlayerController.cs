using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public Rigidbody2D theRB;
    public float moveSpeed = 5f;
    public float mouseSensitivity = 1f;
    public Camera viewCam;
    public GameObject bulletImpact;
    public int CurrentAmmo;

    public Animator gunAnim;
    public Animator fireAnim;
    public Animator anim;

    private Vector2 moveInput;
    private Vector2 mouseImput;

    public int currentHealth;
    public int maxHelth = 100;
    public GameObject deadScreen;
    public GameObject winScreen;
    public bool hasDied;

    public int scoore=0;

    public Text healthText, ammoText;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHelth;
        healthText.text = currentHealth.ToString() +"%";

        ammoText.text = CurrentAmmo.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(!hasDied)
        {
            //player movement
            moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            Vector3 moveHorizontal = transform.up * -moveInput.x;
            Vector3 moveVertical = transform.right * moveInput.y;

            theRB.velocity = (moveHorizontal + moveVertical) * moveSpeed;

            //player view control
            mouseImput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - mouseImput.x);

            viewCam.transform.localRotation = Quaternion.Euler(viewCam.transform.localRotation.eulerAngles + new Vector3(0f, mouseImput.y, 0f));

            //shooting
            if (Input.GetMouseButtonDown(0))
            {
                if (CurrentAmmo > 0)
                {
                    Ray ray = viewCam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        //Debug.Log("I'm Looking at" + hit.transform.name);

                        Instantiate(bulletImpact, hit.point, transform.rotation);

                        if(hit.transform.tag == "Enemy")
                        {
                            hit.transform.parent.GetComponent<EnemyController>().TakeDamage();
                        }
                    }
                    else
                    {
                        Debug.Log("I'm Looking nothing");
                    }
                    CurrentAmmo--;
                    gunAnim.SetTrigger("Shoot");
                    fireAnim.SetTrigger("fire");
                    UpdateAmmoUI();
                }
            }

            if(moveInput != Vector2.zero)
            {
                anim.SetBool("isMoving", true);
            }else{
                anim.SetBool("isMoving", false);
            }

            if(scoore >= 13)
            {
                winScreen.SetActive(true);
                hasDied = true;
            }
        }
    }

    public void TakeDamage(int damagAmount)
    {
        currentHealth -= damagAmount;

        if(currentHealth <= 0)
        {
            deadScreen.SetActive(true);
            hasDied = true;
            currentHealth = 0;
        }

        healthText.text = currentHealth.ToString() +"%";
    }

    public void AddHealth(int healAmount)
    {
        currentHealth += healAmount;
        if(currentHealth > maxHelth)
        {
            currentHealth = maxHelth;
        }
        healthText.text = currentHealth.ToString() +"%";
    }

    public void UpdateAmmoUI()
    {
        ammoText.text = CurrentAmmo.ToString();
    }
}
