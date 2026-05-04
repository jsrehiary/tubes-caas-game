using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;
    public int damage = 1;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // transform Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // if (other.CompareTag("Enemy"))
        // {
        //     other.GetComponent<Enemy>()?.TakeDamage(damage);

        //     Destroy(gameObject);
        // }
    }
}
