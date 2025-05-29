using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


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
    /*private bool isHit = false;*/
    private bool isDead = false;
        //Blink at Hit
    private Renderer rend;
    private Color originalColor;

    [SerializeField] Color blinkColor = Color.white;
    [SerializeField] float blinkDuration = 0.1f;
    [SerializeField] int blinkCount = 3;

    //Temporary System
    private bool isPaused = false;
    
    /*//Model Tilting
    [SerializeField] float tiltAmount = 30f;
    [SerializeField] float tiltSpeed = 5f;
    [SerializeField] Transform playerModel;*/
    /*Quaternion defaultRotation;*/
    
    
    
    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
        
        /*defaultRotation = playerModel.localRotation;*/
    }

    void Awake()
    {
        isDead = false;
    }

    void Update()
    {
        // MOVEMENT
        /*float moveY = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(moveX, moveY, 0f).normalized;
        transform.position += movement * movingSpeed * Time.deltaTime;*/
        float moveY = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveX, moveY, 0f).normalized;
        transform.position += movement * movingSpeed * Time.deltaTime;

        
        // //Movement Tilt
        // Quaternion targetRotation;
        // if (Mathf.Abs(moveY) > 0.01f)
        // {
        //     // Calculate target tilt when moving
        //     float targetX = -moveY * tiltAmount;
        //     targetRotation = Quaternion.Euler(targetX, 0f, 0f);
        // }
        // else
        // {
        //     // Return to default upright rotation
        //     targetRotation = defaultRotation;
        // }
        // playerModel.localRotation = Quaternion.Slerp(playerModel.localRotation, targetRotation, Time.deltaTime * tiltSpeed);
        
        //Health & Death
        HandleHealth();
        
        //Shooting
        HandleShooting();
        
        //PAUSE AND RESTART
        HandleRestart();
        HandlePause();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            StartCoroutine(Blink());
            playerHealth -= 15;
        }
        else if (other.CompareTag("EnemyBullet"))
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

    void HandlePause()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!isPaused)
            {
                Time.timeScale = 0f;
                isPaused = true;
                Debug.Log("Game Paused");
            }
            else
            {
                Time.timeScale = 1f;
                isPaused = false;
                Debug.Log("Game Resumed");
            }
        }
    }
    
    void HandleRestart()
    {
        //Temporary Restart
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    void HandleShooting()
    {
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

    void HandleHealth()
    {
        //ISDEAD
        if (playerHealth <= 0 && !isDead)
        {
            Debug.Log("PLAYER DIED");
            isDead = true;
            Time.timeScale = 0f;
            SceneManager.LoadScene("SampleScene");
            Time.timeScale = 1f;
        }
    }
    
}

    


        