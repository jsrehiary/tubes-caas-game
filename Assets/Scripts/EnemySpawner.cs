using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public float spawnInterval = 2f; 

    void Start()
    {
        //manggil fungsi spawn dr interval
        InvokeRepeating("SpawnEnemy", 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        // posisi dr objek spawn, x acak
        Vector3 spawnPos = transform.position; 
        
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}