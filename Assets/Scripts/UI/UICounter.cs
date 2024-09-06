using UnityEngine;

public class UICounter : MonoBehaviour
{
    [SerializeField] private ScoreView _�urrentScoreView;
    [SerializeField] private RectTransform _canvas;

    public ScoreView Generate(Vector3 position, int initialized)
    {
        _�urrentScoreView.SetPosition(position, _canvas);
        _�urrentScoreView.UpdateScore(initialized);

        return _�urrentScoreView;
    }
}