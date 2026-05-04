using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 20f;
    public Vector3 direction = Vector3.back;

    [Header("Combat")]
    public int damage = 1;

    [Header("Lifetime")]
    public float lifeTime = 3f;

    void OnEnable()
    {
        // penting kalau nanti pakai object pooling
        CancelInvoke();
        Invoke(nameof(DestroySelf), lifeTime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // kasih damage ke enemy
            // var enemy = other.GetComponent<Enemy>();
            // if (enemy != null)
            // {
            //     enemy.TakeDamage(damage);
            // }

            DestroySelf();
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}