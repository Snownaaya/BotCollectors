using UnityEngine;

public class UICounter : MonoBehaviour
{
    [SerializeField] private ScoreView _ñurrentScoreView;
    [SerializeField] private RectTransform _canvas;

    public ScoreView Generate(Vector3 position, int initialized)
    {
        _ñurrentScoreView.SetPosition(position, _canvas);
        _ñurrentScoreView.UpdateScore(initialized);

        return _ñurrentScoreView;
    }
}