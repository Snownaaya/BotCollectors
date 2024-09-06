using UnityEngine;
using TMPro;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private TextMeshProUGUI _resourceScore;

    private RectTransform _canvasTransform;
    private RectTransform _uiTransform;
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _uiTransform = GetComponent<RectTransform>();
    }

    private void OnEnable() =>
        _base.CountChanged += UpdateScore;

    private void OnDisable() =>
        _base.CountChanged -= UpdateScore;

    public void UpdateScore(int score) =>
        _resourceScore.text = $"resource: {score}";

    public void SetPosition(Vector3 worldPosition, RectTransform canvasTransform)
    {
        _canvasTransform = canvasTransform;

        Vector3 screenTransform = _mainCamera.WorldToScreenPoint(worldPosition);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasTransform, screenTransform, _mainCamera, out Vector2 localPoint);

        _uiTransform.anchoredPosition = localPoint;
    }
}
