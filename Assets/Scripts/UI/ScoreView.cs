using UnityEngine;
using TMPro;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private TextMeshProUGUI _resourceScore;

    private void Awake() =>
        transform.SetParent(_base.transform);

    private void OnEnable() =>
        _base.CountChanged += UpdateScore;

    private void OnDisable() =>
        _base.CountChanged -= UpdateScore;

    public void UpdateScore() =>
        _resourceScore.text = $"resource: {_base.CurrentResource}";
}
