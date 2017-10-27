using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int startingHealth = 1;
    [SerializeField] private float speed = 3f;
    [SerializeField] private int pointsThePlayerGetsWhenThisEnemyDies = 1;
    [SerializeField] private float speedIncreaseOverTime = 0.2f;   // How quickly the enemies spawned later in the game increase in speed

    [SerializeField] private float minRotation = 0f;                // How fast the emojis rotate
    [SerializeField] private float maxRotation = 180f;

    private Vector3 rotation;

    private int health;

    void Start()
    {
        health = startingHealth;

        speed += Random.Range(-1f, 1f);
        speed += speedIncreaseOverTime * Time.timeSinceLevelLoad;

        rotation.x = Random.Range(minRotation, maxRotation);
        rotation.y = Random.Range(minRotation, maxRotation);
        rotation.z = Random.Range(minRotation, maxRotation);
    }

    void Update()
    {
        MoveTowardsPlayer();

        transform.Rotate(rotation * Time.deltaTime);
    }

    void MoveTowardsPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Vector3 dir = player.transform.position - transform.position;
            transform.position += dir.normalized * speed * Time.deltaTime;
        }
    }

    void OnDeath()
    {
        GameObject.Find("Canvas").GetComponent<UIController>().IncreaseScore(pointsThePlayerGetsWhenThisEnemyDies);
        Destroy(gameObject);
    }

    // takes damage
    public void TakeDamage(int damage)
    {
        health -= damage;
        //Debug.Log("Enemy.TakeDamage " + damage);

        if (health <= 0)
        {
            OnDeath();
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            coll.gameObject.GetComponent<PlayerController>().TakeDamage(1);
            Destroy(gameObject);
        }
    }
}