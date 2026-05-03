using TMPro;
using UnityEngine;

public class GateLogic : MonoBehaviour
{
    public enum GateType
    {
        Tambah,
        Kurang,
        Kali,
        Bagi
    }

    [Header("Gate Settings")]
    [SerializeField] private bool useRandomValue = true;
    [SerializeField] private GateType tipeGerbang;
    [SerializeField] private int nilaiOperasi;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float destroyZPosition = -20f;

    [Header("UI")]
    [SerializeField] private TextMeshPro valueText;

    [Header("Visual Colors")]
    [SerializeField] private Color tambahColor = new Color(0.1f, 0.9f, 0.35f);
    [SerializeField] private Color kurangColor = new Color(1f, 0.2f, 0.2f);
    [SerializeField] private Color kaliColor = new Color(0.2f, 0.6f, 1f);
    [SerializeField] private Color bagiColor = new Color(1f, 0.75f, 0.1f);

    private bool sudahDiklaim = false;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (useRandomValue)
        {
            SetupGateRNG();
        }

        UpdateGateUI();
        UpdateGateVisual();
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

    private void Update()
    {
        transform.position += Vector3.back * moveSpeed * Time.deltaTime;

        if (transform.position.z < destroyZPosition)
        {
            Destroy(gameObject);
        }
    }

    private void UpdateGateUI()
    {
        if (valueText == null) return;

        valueText.text = GetDisplayText();
        valueText.color = GetGateColor();
    }

    private void UpdateGateVisual()
    {
        if (spriteRenderer == null) return;

        spriteRenderer.color = GetGateColor();
    }

    private string GetDisplayText()
    {
        switch (tipeGerbang)
        {
            case GateType.Tambah:
                return "+" + nilaiOperasi;

            case GateType.Kurang:
                return "-" + nilaiOperasi;

            case GateType.Kali:
                return "x" + nilaiOperasi;

            case GateType.Bagi:
                return "÷" + nilaiOperasi;

            default:
                return nilaiOperasi.ToString();
        }
    }

    private Color GetGateColor()
    {
        switch (tipeGerbang)
        {
            case GateType.Tambah:
                return tambahColor;

            case GateType.Kurang:
                return kurangColor;

            case GateType.Kali:
                return kaliColor;

            case GateType.Bagi:
                return bagiColor;

            default:
                return Color.white;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (sudahDiklaim) return;
        if (!other.CompareTag("Player")) return;

        PlayerController playerScript = other.GetComponentInParent<PlayerController>();

        if (playerScript == null)
        {
            Debug.LogWarning("PlayerController tidak ditemukan di Player atau parent-nya.");
            return;
        }

        sudahDiklaim = true;

        playerScript.ExecuteGateLogic(tipeGerbang, nilaiOperasi);

        // Optional: panggil floating text HUD jika nanti sudah ada
        // HUDController.Instance?.SpawnFloatingText(GetDisplayText(), transform.position, FeedbackType.Positive);

        gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            UpdateGateUI();
            UpdateGateVisual();
        }
    }
#endif
}