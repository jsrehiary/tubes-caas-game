using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private PlayerController playerController;

    [Header("Shooting")]
    [SerializeField] private float fireRate = 0.8f;
    [SerializeField] private Vector3 shootDirection = Vector3.forward;

    [Header("Limit")]
    [SerializeField] private int maxShooters = 30;

    private float timer;

    private void Awake()
    {
        if (bulletPrefab == null)
        {
            bulletPrefab = Resources.Load<GameObject>("Bulletkuy");

            if (bulletPrefab == null)
            {
                Debug.LogWarning("Bulletkuy prefab tidak ada di folder Resources!", this);
            }
        }

        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();

            if (playerController == null)
            {
                Debug.LogWarning("PlayerController tidak ditemukan di scene!", this);
            }
        }
    }

    private void Update()
    {
        if (bulletPrefab == null || playerController == null) return;

        timer += Time.deltaTime;

        if (timer >= fireRate)
        {
            ShootFromSquad();
            timer = 0f;
        }
    }

    private void ShootFromSquad()
    {
        List<Transform> firePoints = playerController.GetActiveFirePoints();

        if (firePoints == null || firePoints.Count <= 0) return;

        int shooterCount = Mathf.Min(firePoints.Count, maxShooters);

        for (int i = 0; i < shooterCount; i++)
        {
            Transform firePoint = firePoints[i];

            if (firePoint == null) continue;

            GameObject bulletObject = Instantiate(
                bulletPrefab,
                firePoint.position,
                Quaternion.identity
            );

            BulletBehaviour bullet = bulletObject.GetComponent<BulletBehaviour>();

            if (bullet != null)
            {
                bullet.direction = shootDirection.normalized;
            }
        }
    }
}