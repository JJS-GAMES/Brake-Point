using System;
using UnityEngine;

public class ProgressTracker : MonoBehaviour
{
    private LevelManager _levelManager;
    private GameManager _gameManager;

    public event Action<float> OnProgressChanged;

    private float _lastProgress;
    public void Init(GameManager gameManager, LevelManager levelManager)
    {
        _gameManager = gameManager;
        _levelManager = levelManager;
    }
    private void Update()
    {
        if (_levelManager == null) return;

        float progress = _gameManager.CalculateLevelProgress();

        if (!Mathf.Approximately(progress, _lastProgress))
        {
            _lastProgress = progress;
            OnProgressChanged?.Invoke(progress);
        }
    }
}
