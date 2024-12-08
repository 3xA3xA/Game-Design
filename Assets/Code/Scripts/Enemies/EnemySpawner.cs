using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject[] emptyStars; // ссылки на пустые звезды
    [SerializeField] private GameObject[] filledStars; // ссылки на заполненные звезды




    [Header("Attribute")]
    [SerializeField] private int totalWaves = 1;
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWafes = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;
    [SerializeField] private float enemiesPerSecondCap = 15f;
    [SerializeField] private GameObject gameOverHover;
    [SerializeField] private GameObject victoryHover;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private float eps; // Enemies Per Second - Для усложнения игры. (Монстры выходят чаще)
    private bool isSpawning = false;


    public static EnemySpawner main;
    public int currentWave = 1;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
        main = this;
    }

    private void Start()
    {
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (1f / eps) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();

            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;

        }


        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0 )
        {
            EndWave();
        }
    }

    public void OpenGameOverHover()
    {
        gameOverHover.SetActive(true);
    }

    public void OpenVictoryHover()
    {
        victoryHover.SetActive(true);
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWafes);

        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
        eps = EnemiesPerSecond();
    }

    //private void EndWave()
    //{
    //    isSpawning = false;
    //    timeSinceLastSpawn = 0f;
    //    currentWave++;
    //    //StartCoroutine(StartWave());

    //    if ((totalWaves <= 0)) StartCoroutine(StartWave());

    //    if (currentWave > totalWaves) { Victory(); }
    //    else
    //    {
    //        StartCoroutine(StartWave());
    //    }
    //}

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;

        if (totalWaves <= 0) StartCoroutine(StartWave());

        if (currentWave > totalWaves)
        {
            Victory();
        }
        else
        {
            StartCoroutine(StartWave());
        }

        // Обновить состояние звезд
        UpdateStars();
    }

    private void UpdateStars()
    {
        // Скрыть пустые звезды и показать заполненные звезды на основании пройденных волн
        for (int i = 0; i < currentWave - 1 && i < emptyStars.Length && i < filledStars.Length; i++)
        {
            if (emptyStars[i] != null)
            {
                emptyStars[i].SetActive(false); // скрыть пустую звезду
            }

            if (filledStars[i] != null)
            {
                filledStars[i].SetActive(true); // показать заполненную звезду
            }
        }
    }



    private int EnemiesPerWave()
    {
        //С каждой новой волной увеличивается кол-во врагов на Уровень волны * Модификатор сложности.
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }

    private float EnemiesPerSecond()
    {
        //С каждой новой волной увеличивается кол-во врагов на Уровень волны * Модификатор сложности.
        return Mathf.Clamp(enemiesPerSecond * Mathf.Pow(currentWave, difficultyScalingFactor), 0f, enemiesPerSecondCap);
    }

    private void SpawnEnemy()
    {
        int index = Random.Range(0, enemyPrefabs.Length);

        GameObject prefabToSpawn = enemyPrefabs[index]; //Так себе идея. Пример - 2 слота(обычный моб) 1 слот(большой моб) шансы - 2 к 1. 
        Vector3 spawnPosition = LevelManager.main.startPoint.position;
        spawnPosition.z = 0f;
        //Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        GameObject enemy = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity); 
        spawnedEnemies.Add(enemy); // Add the spawned enemy to the list
    }

    private void Victory()
    {
        if (totalWaves <= 0) return;
        OpenVictoryHover();

        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        Debug.Log("WIn!");
    }

    public void GameOver()
    {
        if (totalWaves <= 0) return;
        OpenGameOverHover();

        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        Debug.Log("Lose!");

        // Destroy all enemies on the map
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        spawnedEnemies.Clear(); // Clear the list after destroying all enemies
    }
}
