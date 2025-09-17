using System;
using UnityEngine;

public class ProgressTracker : MonoBehaviour
{
    private GameManager _gameManager;

    public event Action<float> OnProgressChanged;

    private float _lastProgress;
    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    private void Update()
    {
        if (_gameManager == null) return;

        float progress = _gameManager.CalculateLevelProgress();

        if (!Mathf.Approximately(progress, _lastProgress))
        {
            _lastProgress = progress;
            OnProgressChanged?.Invoke(progress);
        }
    }
}
