using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 12f; // speed musuh

    [Header("RNG Spawn Settings")]
    public float xRange = 25f; // batas x

    [Header("Combat Settings")]
    public int health = 1; // berapa hit musuh mati

    void Start()
    {
        // 1. RNG buat awal spawn
        float randomX = Random.Range(-xRange, xRange);
        
        // posisi xnya bakal acak, y sama z dari unity
        transform.position = new Vector3(randomX, transform.position.y, transform.position.z);
    }

    void Update()
    {
        // ke arah tag "player"
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

        // batas musuh hidup
        if (transform.position.z < -20f)
        {
            Destroy(gameObject);
        }
    }

    // hit dari bullet
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // partikel (opsional)
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // fungsi nabrak kalo kena musuh biar ilang
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponentInParent<PlayerController>();
            
            if (player != null)
            {
                // pasukan (player) -1 kalo kena musuh
                player.ExecuteGateLogic(GateLogic.GateType.Kurang, 1);
                
                Die();
            }
        }
    }
}