using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public int pointsToWin = 30;

    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject player;
    [SerializeField] private int enemiesToSpawn = 3;        // How many enemies we want to spawn per wave
    [SerializeField] private float enemyMinDistanceFromPlayer = 20;
    [SerializeField] private float enemyMaxDistanceFromPlayer = 50;
    [SerializeField] private float timeToInitialSpawn = 15; // How long before the first wave      
    [SerializeField] private float timeToSpawnEnemies = 2f; // How long between each wave
    [SerializeField] private int lastWave = 10;             // How many waves do we want to spawn?

    private float enemySpawnTimer;  // How long since last enemy wave
    private int waves = 0;          // The wave we are currently on

    public delegate void OnRestart();
    public OnRestart onRestart;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("GameController -- More than one instance");
            Destroy(gameObject);
        }

        enemySpawnTimer = timeToSpawnEnemies - timeToInitialSpawn;
    }

    void Update()
    {
        enemySpawnTimer += Time.deltaTime;

        if (enemySpawnTimer >= timeToSpawnEnemies)
        {
            SpawnEnemies();
            enemySpawnTimer = 0;
        }

        // For debugging purposes
        if (Input.GetButtonDown("Reset"))
        {
            RestartGame();
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void SpawnEnemies()
    {
        if (player != null && waves < lastWave)
        {
            // Spawn our enemies
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                float randX = 0f;
                float randY = 0f;

                // Get the absolue values for x and y

                // We only want one of x or y to have a minimum distance
                // Otherwise, the enemies will only spawn in the corners
                // which is lame af tbh fam
                if (Random.Range(0, 2) == 0)
                {
                    randX = Random.Range(enemyMinDistanceFromPlayer, enemyMaxDistanceFromPlayer);
                    randY = Random.Range(0, enemyMaxDistanceFromPlayer);
                }
                else
                {
                    randX = Random.Range(0, enemyMaxDistanceFromPlayer);
                    randY = Random.Range(enemyMinDistanceFromPlayer, enemyMaxDistanceFromPlayer);
                }

                // Make it so they're not always positive
                if (Random.Range(0, 2) == 1)
                {
                    randX = -randX;
                }

                if (Random.Range(0, 2) == 1)
                {
                    randY = -randY;
                }

                // Spawn our enemies
                Vector3 pos = player.transform.position + new Vector3(randX, randY, 0f);
                GameObject.Instantiate(enemy, pos, Quaternion.identity);
            }

            waves++;
        }
    }

    public void OnVictory()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject go in enemies)
        {
            Destroy(go);
        }

        waves = lastWave;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("_main");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}