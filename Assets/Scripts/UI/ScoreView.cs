using UnityEngine;
using TMPro;
using Zenject;

public class ScoreView : MonoBehaviour
{
    [Inject] ResourcePool _scoreCounter;

    [SerializeField] TMP_Text _resourceScore;

    private void OnEnable() =>
        _scoreCounter.CountChanged += UpdateScore;

    private void OnDisable() =>
        _scoreCounter.CountChanged -= UpdateScore;
    
    public void UpdateScore() =>
        _resourceScore.text = $"resource based on: {_scoreCounter.ResourceCount}";
}
