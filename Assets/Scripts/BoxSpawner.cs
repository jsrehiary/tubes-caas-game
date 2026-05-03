using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] daftarGatePrefab; 
    
    [Header("Pengaturan Waktu")]
    public float minInterval = 1f;  // min delay
    public float maxInterval = 15f; // max delay
    
    private float timer;
    private float nextSpawnTime;

    void Start()
    {
        // waktu spawn pas game dimulai
        SetRandomNextSpawn();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= nextSpawnTime)
        {
            SpawnRandomGate();
            
            // RNGnya
            timer = 0;
            SetRandomNextSpawn();
        }
    }

    void SetRandomNextSpawn()
    {
        // RNG dari minInterval dan maxInterval
        nextSpawnTime = Random.Range(minInterval, maxInterval);
    }

    void SpawnRandomGate()
    {
        if (daftarGatePrefab.Length > 0)
        {
            int indexAcak = Random.Range(0, daftarGatePrefab.Length);
            Instantiate(daftarGatePrefab[indexAcak], transform.position, Quaternion.identity);
        }
    }
}