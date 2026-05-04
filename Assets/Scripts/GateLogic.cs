using UnityEngine;

public class GateLogic : MonoBehaviour
{
    public enum GateType { Tambah, Kurang, Kali, Bagi }

    [Header("Gate Settings")]
    [SerializeField] private bool useRandomValue = true;
    [SerializeField] private GateType tipeGerbang;
    [SerializeField] private int nilaiOperasi;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float destroyZPosition = -20f;

    private bool sudahDiklaim = false;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Tetap paksa Material Unlit agar balok tidak hitam/gelap
        if (spriteRenderer != null)
        {
            spriteRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }

        if (useRandomValue)
        {
            SetupGateRNG();
        }
    }

    private void SetupGateRNG()
    {
        tipeGerbang = (GateType)Random.Range(0, 4);

        switch (tipeGerbang)
        {
            case GateType.Tambah: nilaiOperasi = Random.Range(1, 51); break;
            case GateType.Kurang: nilaiOperasi = Random.Range(1, 51); break;
            case GateType.Kali: nilaiOperasi = Random.Range(2, 5); break;
            case GateType.Bagi: nilaiOperasi = Random.Range(2, 4); break;
        }
    }

    private void Update()
    {
        // Pergerakan balok ke arah Z negatif
        transform.position += Vector3.back * moveSpeed * Time.deltaTime;

        if (transform.position.z < destroyZPosition)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (sudahDiklaim) return;
        if (!other.CompareTag("Player")) return;

        PlayerController playerScript = other.GetComponentInParent<PlayerController>();

        if (playerScript != null)
        {
            sudahDiklaim = true;
            playerScript.ExecuteGateLogic(tipeGerbang, nilaiOperasi);
            gameObject.SetActive(false);
        }
    }
}