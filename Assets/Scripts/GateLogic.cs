using UnityEngine;
using TMPro;

public class GateLogic : MonoBehaviour
{
    public enum GateType { Tambah, Kurang, Kali, Bagi }

    [Header("Gate Settings")]
    [SerializeField] private bool useRandomValue = true;
    [SerializeField] private GateType tipeGerbang;
    [SerializeField] private int nilaiOperasi;

    [Header("Text")]
    [SerializeField] private TMP_Text valueText;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float destroyZPosition = -20f;

    private bool sudahDiklaim = false;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }

        if (useRandomValue)
        {
            SetupGateRNG();
        }

        UpdateValueText();
    }

    private void SetupGateRNG()
    {
        tipeGerbang = (GateType)Random.Range(0, 4);

        switch (tipeGerbang)
        {
            case GateType.Tambah:
                nilaiOperasi = Random.Range(1, 51);
                break;

            case GateType.Kurang:
                nilaiOperasi = Random.Range(1, 51);
                break;

            case GateType.Kali:
                nilaiOperasi = Random.Range(2, 5);
                break;

            case GateType.Bagi:
                nilaiOperasi = Random.Range(2, 4);
                break;
        }
    }

    private void UpdateValueText()
    {
        if (valueText == null)
        {
            valueText = GetComponentInChildren<TextMeshPro>();
        }

        if (valueText == null)
        {
            Debug.LogWarning("ValueText belum dipasang pada obstacle: " + gameObject.name, this);
            return;
        }

        switch (tipeGerbang)
        {
            case GateType.Tambah:
                valueText.text = "+" + nilaiOperasi;
                break;

            case GateType.Kurang:
                valueText.text = "-" + nilaiOperasi;
                break;

            case GateType.Kali:
                valueText.text = "x" + nilaiOperasi;
                break;

            case GateType.Bagi:
                valueText.text = "/" + nilaiOperasi;
                break;
        }
    }

    private void Update()
    {
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