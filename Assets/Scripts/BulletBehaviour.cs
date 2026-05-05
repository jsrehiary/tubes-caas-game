using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 20f;
    public Vector3 direction = Vector3.forward;

    [Header("Combat")]
    [SerializeField] private int damage = 1;

    [Header("Lifetime")]
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private float destroyZLimit = 1000f;

    private bool hasHit;

    private void Start()
    {
        Renderer rend = GetComponentInChildren<Renderer>();

        if (rend != null)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Unlit");

            if (shader == null)
            {
                shader = Shader.Find("Unlit/Color");
            }

            Material mat = new Material(shader);
            mat.color = Color.yellow;
            mat.SetColor("_BaseColor", Color.yellow);
            rend.material = mat;
        }

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if (Mathf.Abs(transform.position.z) > destroyZLimit)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        // Abaikan player/soldier sendiri.
        if (other.GetComponentInParent<PlayerController>() != null)
        {
            return;
        }

        // Ambil enemy dari object yang kena atau parent-nya.
        EnemyBehavior enemy = other.GetComponentInParent<EnemyBehavior>();

        // Kalau bukan enemy, abaikan. Contoh: obstacle, ground, gate, dll.
        if (enemy == null)
        {
            return;
        }

        hasHit = true;

        enemy.TakeDamage(damage);

        Destroy(gameObject);
    }
}