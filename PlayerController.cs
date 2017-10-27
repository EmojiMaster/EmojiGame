using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject bullet;             // We use this for shooting
    [SerializeField] private float speed = 1.0f;            // How fast the player moves
    [SerializeField] private int startingHealth = 3;        // How much health the player starts with
    [SerializeField] private float timeBetweenShots = 0.2f; // The time it takes before the player can shoot again

    private float timeSinceLastShot = 0f;                   // How long it's been since the last shot

    private int health;         // The player's current health

    void Start()
    {
        timeSinceLastShot = timeBetweenShots;
        health = startingHealth;
    }

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        // Set movement direction to player input
        float velX = Input.GetAxis("Horizontal");
        float velY = Input.GetAxis("Vertical");
        Vector3 moveDir = new Vector3(velX, velY, 0f);
        Move(moveDir);

        // Shooting
        if (Input.GetButton("Fire1") && timeSinceLastShot >= timeBetweenShots)
        {
            Vector3 mouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            Vector3 shootingDir = new Vector3(mouseDir.x, mouseDir.y, 0f);
            Shoot(shootingDir);

            timeSinceLastShot = 0f;
        }
    }

    // Moves the player by moveDir
    void Move(Vector3 moveDir)
    {
        transform.position += moveDir * speed * Time.deltaTime;
    }

    // Shoots in the direction of shootDir
    void Shoot(Vector3 shootDir)
    {
        GameObject theBullet = GameObject.Instantiate(bullet, transform.position, Quaternion.identity);
        theBullet.GetComponent<Bullet>().moveDir = new Vector3(shootDir.x, shootDir.y, 0f);
        DestroyObject(theBullet, 1.5f);
    }

    // takes damage
    public void TakeDamage(int damage)
    {
        health -= damage;
        //Debug.Log("Player::TakeDamage " + damage);

        if (health <= 0)
        {
            OnDeath();
        }
    }

    void OnDeath()
    {
        UIController.instance.OnLose();
        Destroy(gameObject);
    }
}