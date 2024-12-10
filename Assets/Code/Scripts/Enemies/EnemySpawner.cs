using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField] private GameObject[] enemyPrefabs; - —тарое добавление мобов дл€ спавна

    [Header("References")]
    [SerializeField] private GameObject[] emptyStars; // ссылки на пустые звезды
    [SerializeField] private GameObject[] filledStars; // ссылки на заполненные звезды


    [Header("Mob Waves")]
    [SerializeField] private GameObject[] wave1Enemies;
    [SerializeField] private GameObject[] wave2Enemies;
    [SerializeField] private GameObject[] wave3Enemies;


    [Header("Attribute")]
    [SerializeField] private int totalWaves = 3;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWafes = 5f;

    [Header("UI")]
    [SerializeField] private GameObject gameOverHover;
    [SerializeField] private GameObject victoryHover;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private float eps; // Enemies Per Second - ƒл€ усложнени€ игры. (ћонстры выход€т чаще)
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
        enemiesLeftToSpawn = GetEnemiesForCurrentWave().Length;
        eps = enemiesPerSecond;
    }


    private void EndWave()
    {
        if (totalWaves <= 0)
        {
            currentWave = 1;
            StartCoroutine(StartWave());
            return;
        }          

        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;

        if (currentWave > totalWaves)
        {
            Victory();
        }
        else
        {
            StartCoroutine(StartWave());
        }

        // ќбновить состо€ние звезд
        UpdateStars();
    }

    private void UpdateStars()
    {
        // —крыть пустые звезды и показать заполненные звезды на основании пройденных волн
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

    private void SpawnEnemy()
    {
        GameObject[] enemiesForWave = GetEnemiesForCurrentWave();
        int index = enemiesForWave.Length - enemiesLeftToSpawn;
        GameObject prefabToSpawn = enemiesForWave[index];
        Vector3 spawnPosition = LevelManager.main.startPoint.position;
        spawnPosition.z = 0f;

        GameObject enemy = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        spawnedEnemies.Add(enemy);
    }

    private GameObject[] GetEnemiesForCurrentWave()
    {
        return currentWave switch
        {
            1 => wave1Enemies,
            2 => wave2Enemies,
            3 => wave3Enemies,
            _ => new GameObject[0],
        };
    }


    private void Victory()
    {
        if (totalWaves <= 0) return;
        OpenVictoryHover();

        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
    }

    public void GameOver()
    {
        if (totalWaves <= 0) return;
        OpenGameOverHover();

        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;

        // ”далить всех мобов с карты
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        spawnedEnemies.Clear(); // ќчистка списка
    }
}
