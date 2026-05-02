using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab of the bullet to spawn
    public Transform firePoint; // Point where the bullet will be spawned

    public float fireRate = 0.2f;
    private float timer;

    public int bulletCount = 1;
    public float spreadAngle = 30f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= fireRate)
        {
            Shoot();
            timer = 0f;
        }
    }

    void Shoot()
    {
        float angleStep = spreadAngle / (bulletCount - 1);
        float startAngle = -spreadAngle / 2;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle  + (angleStep * i);
            Quaternion rotation = Quaternion.Euler(0, 0, angle) * firePoint.rotation;
            Instantiate(bulletPrefab, firePoint.position, rotation);
        }
    }
}
