using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    Animator animator;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private EnemySpawner en;

    private Transform target;
    private int pathIndex = 0;

    private Transform[] path1;
    private Transform[] path2;

    private float baseSpeed;

    private void Start()
    {
        baseSpeed = moveSpeed;

        path1 = LevelManager.main.path1;
        path2 = LevelManager.main.path2;

        // Выбор первой цели
        target = Random.Range(0, 2) == 0 ? path1[pathIndex] : path2[pathIndex];

        animator = GetComponent<Animator>();

        if (en == null) // :)
        {
            en = FindObjectOfType<EnemySpawner>();
        }
    }

    private void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if (pathIndex == path1.Length || pathIndex == path2.Length)
            {
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);

                en.GameOver();
                return;
            }
            else
            {
                // 2/3 вероятности остаться на текущем пути и 1/3 вероятности сменить путь
                bool switchPath = Random.Range(0, 3) == 0;
                if (switchPath)
                {
                    target = target == path1[pathIndex - 1] ? path2[pathIndex] : path1[pathIndex];
                }
                else
                {
                    target = target == path1[pathIndex - 1] ? path1[pathIndex] : path2[pathIndex];
                }
            }
        }
    }

    public void UpdateSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    public void ResetSpeed()
    {
        moveSpeed = baseSpeed;
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position);
        float distanceToTarget = direction.magnitude;

        if (distanceToTarget < 0.1f)
        {
            transform.position = target.position;
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.velocity = direction.normalized * moveSpeed;

            // Поворот персонажа в сторону ходьбы
            if (rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                //transform.localScale = new Vector3(-1, 1, 1); // Поворот направо
            }
            else if (rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                //transform.localScale = new Vector3(1, 1, 1); // Поворот налево
            }

            animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        }
    }
}