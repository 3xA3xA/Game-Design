using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float bps = 1f; // Bullets Per Second
    [SerializeField] private float slowmoRange = 2.5f;
    [SerializeField] private float aps = 4f; // Attacks Per Second
    [SerializeField] private float freezeTime = 1f;

    private Transform target;
    private float timeUntilFire;
    private float timeUntilSlowmo;
    private BuildManager buildManager;

    private void Awake()
    {
        buildManager = BuildManager.main;
    }

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / bps)
            {
                Shoot();
                timeUntilFire = 0f;
            }

            //»з-за об€зательной инициализации башни через магазин костыль.
            if (buildManager == null)
                return;

            // ѕровер€ем индекс башни, чтобы замедл€ть врагов только если индекс равен 2
            if (buildManager.selectedTower == 2)
            {
                timeUntilSlowmo += Time.deltaTime;

                if (timeUntilSlowmo >= 1f / aps)
                {
                    FreezeEnemies();
                    timeUntilSlowmo = 0f;
                }
            }
        }
    }

    private void Shoot()
    {
        Vector3 firingPosition = firingPoint.position;
        firingPosition.z = 0f;

        GameObject bulletObj = Instantiate(bulletPrefab, firingPosition, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.SetTarget(target);
        }
    }

    private void FreezeEnemies()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, slowmoRange, Vector2.zero, 0f, enemyMask);

        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                EnemyMovement enemyMovement = hit.transform.GetComponent<EnemyMovement>();

                if (enemyMovement != null)
                {
                    enemyMovement.UpdateSpeed(0.5f); // «амедлить врага
                    StartCoroutine(ResetEnemySpeed(enemyMovement));
                }
            }
        }
    }

    private IEnumerator ResetEnemySpeed(EnemyMovement enemyMovement)
    {
        yield return new WaitForSeconds(freezeTime);
        enemyMovement.ResetSpeed();
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
        Handles.DrawWireDisc(transform.position, transform.forward, slowmoRange);
    }
#endif
}
