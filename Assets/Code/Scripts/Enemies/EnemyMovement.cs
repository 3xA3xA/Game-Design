using System.Collections;
using System.Collections.Generic;
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

    private float baseSpeed;

    private void Start()
    {
        baseSpeed = moveSpeed;
        target = LevelManager.main.path[pathIndex];

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

            if (pathIndex == LevelManager.main.path.Length) 
            {
                EnemySpawner.onEnemyDestroy.Invoke();           
                Destroy(gameObject);

                en.GameOver();
                return;
            }
            else
            {
                target = LevelManager.main.path[pathIndex];
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

            // Flip the character sprite based on the direction
            if (rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1); // Flip to the left
            }
            else if (rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1); // Flip to the right
            }

            animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        }
    }

}
