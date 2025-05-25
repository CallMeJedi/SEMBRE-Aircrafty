using System;
using UnityEngine;
using System.Collections;


public class Control : MonoBehaviour
{
    //SHOOTING
    [SerializeField] float movingSpeed;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float bulletSpeed;

    //GUN COOLDOWN
    [SerializeField] float countdownTime = 0.75f;
    private float timer;
    private bool hasShot = false;

    //HEALTH & DAMAGE
    [SerializeField] int playerHealth = 100;
    private bool isHit = false;
    private bool isDead = false;
        //Blink at Hit
    private Renderer rend;
    private Color originalColor;

    [SerializeField] Color blinkColor = Color.white;
    [SerializeField] float blinkDuration = 0.1f;
    [SerializeField] int blinkCount = 3;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    private void Awake()
    {
        isDead = false;
    }

    private void Update()
    {
        //ISDEAD
        if (playerHealth <= 0 && !isDead)
        {
            Debug.Log("PLAYER DIED");
            isDead = true;
        }
        
        // MOVEMENT
        float moveY = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(moveX, moveY, 0f).normalized;
        transform.position += movement * movingSpeed * Time.deltaTime;

        // SHOOT
        if (Input.GetButtonDown("Fire1") && !hasShot)
        {
            Shoot();
            hasShot = true;
            timer = countdownTime; // Start countdown
        }

        // COUNTDOWN
        if (hasShot)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                hasShot = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            StartCoroutine(Blink());
            playerHealth -= 15;
        }
        else if (other.CompareTag("Enemy"))
        {
            StartCoroutine(Blink());
            playerHealth -= 10;
        }
    }

    IEnumerator Blink()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            rend.material.color = blinkColor;
            yield return new WaitForSeconds(blinkDuration);
            rend.material.color = originalColor;
            yield return new WaitForSeconds(blinkDuration);
        }
    }
    
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.AddForce(firePoint.right * bulletSpeed);
        }
    }
}

