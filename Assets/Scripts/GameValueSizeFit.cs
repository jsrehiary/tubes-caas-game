using UnityEngine;

[ExecuteAlways]
public class GateValueTextAutoFit : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer obstacleRenderer;
    [SerializeField] private RectTransform valueCanvasRect;

    [Header("Fit Settings")]
    [SerializeField] private float pixelsPerUnit = 100f;
    [SerializeField] private float widthMultiplier = 0.85f;
    [SerializeField] private float heightMultiplier = 0.5f;
    [SerializeField] private Vector3 localOffset = new Vector3(0f, 0f, -0.1f);

    private void Reset()
    {
        obstacleRenderer = GetComponent<SpriteRenderer>();
        valueCanvasRect = GetComponentInChildren<Canvas>()?.GetComponent<RectTransform>();
    }

    private void Awake()
    {
        Fit();
    }

    private void Start()
    {
        Fit();
    }

    private void OnValidate()
    {
        Fit();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            Fit();
        }
#endif
    }

    public void Fit()
    {
        if (obstacleRenderer == null)
        {
            obstacleRenderer = GetComponent<SpriteRenderer>();
        }

        if (valueCanvasRect == null)
        {
            Canvas canvas = GetComponentInChildren<Canvas>();
            if (canvas != null)
            {
                valueCanvasRect = canvas.GetComponent<RectTransform>();
            }
        }

        if (obstacleRenderer == null || valueCanvasRect == null) return;

        Vector2 worldSize = obstacleRenderer.bounds.size;

        valueCanvasRect.sizeDelta = new Vector2(
            worldSize.x * pixelsPerUnit * widthMultiplier,
            worldSize.y * pixelsPerUnit * heightMultiplier
        );

        valueCanvasRect.localPosition = localOffset;
        valueCanvasRect.localRotation = Quaternion.identity;
    }
}