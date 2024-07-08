using UnityEngine;
using TMPro;

public class ScoreView : MonoBehaviour
{
    [SerializeField] ResourcePool _scoreCounter;
    [SerializeField] TMP_Text _resourceScore;

    private void OnEnable() =>
        _scoreCounter.CountChaged += UpdateScore;

    private void OnDisable() =>
        _scoreCounter.CountChaged -= UpdateScore;

    public void UpdateScore() =>
        _resourceScore.text = $"resource based on: {_scoreCounter.ResourceCount}";
}
