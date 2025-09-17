using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager; // Temporary initialization in the inspector / Временно инициализация в инспекторе

    [SerializeField] private GameObject _carPrefab;
    private Car _carScript;

    [Header("Zones / Зоны")]
    [Space, SerializeField, Tooltip("Position of the spawn on the stage / Позиция спавна на сцене")]
    private Transform _spawnPosition;
    [SerializeField, Tooltip("Prefab of the finish area on the stage / Префаб финишной зоны на сцене")] 
    private FinishZone _finishZone;

    [Header("Development")]
    [SerializeField] private Camera _devCamera;

    private Transform _finishZoneTransform;

    private float _totalDistance;

    public event Action<float> OnProgressChanged;
    private float _lastProgress;

    private void Start()
    {
        _finishZoneTransform = _finishZone.transform;

        _totalDistance = Vector2.Distance(_spawnPosition.position, _finishZoneTransform.position);

        _uiManager.Init(this);
    }

    public void RestartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    private void FixedUpdate()
    {
        if (_carScript == null) return;

        float progress = CalculateLevelProgress();
        if (!Mathf.Approximately(progress, _lastProgress))
        {
            _lastProgress = progress;
            OnProgressChanged?.Invoke(progress);
        }
    }
    public void CarInitialization(GameObject carPrefab)
    {
        if (_carScript != null)
            _uiManager?.UnsubscribeFromCar(_carScript);

        _carPrefab = carPrefab;

        GameObject car = Instantiate(_carPrefab);
        _carScript = car.GetComponent<Car>();

        if (_spawnPosition != null) car.transform.position = _spawnPosition.position;
        else
        {
            Debug.LogWarning("Spawn Position - not inittialized! Using default position.");
            car.transform.position = new Vector2(0, 0);
        }

        _devCamera?.gameObject.SetActive(false);
        _uiManager.CarUIInitialization(_carScript);
        _finishZone.Init(_carScript, _uiManager);
    }

    public float CalculateLevelProgress()
    {
        // We count how much the car has traveled
        // Считаем, сколько проехала машина

        float carDistance = Vector2.Distance(_spawnPosition.position, _carScript.Target.transform.position);

        float progress = Mathf.Clamp01(carDistance / _totalDistance);

        return progress;
    }
}
