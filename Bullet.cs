using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 100f;
    [SerializeField] private float minEmojiSize = 0.1f;
    [SerializeField] private float maxEmojiSize = 2f;

    private static Sprite[] emojis;

    public Vector3 moveDir;

    void Start()
    {
        
        float randScale = Random.Range(minEmojiSize, maxEmojiSize);
        transform.localScale = new Vector3(randScale, randScale, 0f);

        /*
        float randRed = Random.Range(0f, 1f);
        float randGreen = Random.Range(0f, 1f);
        float randBlue = Random.Range(0f, 1f);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(randRed, randGreen, randBlue);
        */

        // Set random emoji as sprite

        emojis = Resources.LoadAll<Sprite>("Emojis");

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        int randIndex = Random.Range(0, emojis.Length);
        sr.sprite = emojis[randIndex];
    }

    void Update()
    {
        transform.position += moveDir.normalized * speed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        GameObject go = coll.gameObject;
        if (go.tag == "Enemy")
        {
            go.GetComponent<Enemy>().TakeDamage(1);
            Destroy(gameObject);
        }
    }
}