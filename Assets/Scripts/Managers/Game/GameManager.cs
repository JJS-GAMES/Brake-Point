using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private GameData _gameData;
    private Car _carScript;

    [Header("Zones / Зоны")]
    [Space, SerializeField, Tooltip("Position of the spawn on the stage / Позиция спавна на сцене")]
    private Transform _spawnPosition;
    [SerializeField, Tooltip("Prefab of the finish area on the stage / Префаб финишной зоны на сцене")] 
    private FinishZone _finishZone;
    [SerializeField, Tooltip("Prefab of the death area on the stage / Префаб смертельной зоны на сцене")]
    private DeathZone _deathZone;

    [Header("Development")]
    [SerializeField] private Camera _devCamera;

    private Transform _finishZoneTransform;

    private float _totalDistance;

    public event Action<float> OnProgressChanged;
    private float _lastProgress;

    private void Awake()
    {
        _uiManager.Init(this, _levelManager);
    }
    private void Start()
    {
        _finishZoneTransform = _finishZone.transform;

        _totalDistance = Vector2.Distance(_spawnPosition.position, _finishZoneTransform.position);

        CarInitialization();
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
    public void CarInitialization()
    {
        if (_carScript != null)
            _uiManager?.UnsubscribeFromCar(_carScript);

        GameObject car = Instantiate(_gameData.CarPrefab);
        _carScript = car.GetComponent<Car>();

        if (_spawnPosition != null) car.transform.position = _spawnPosition.position;
        else
        {
            Debug.LogWarning("Spawn Position - not inittialized! Using default position.");
            car.transform.position = new Vector2(0, 0);
        }

        _devCamera?.gameObject.SetActive(false);
        _carScript.Init();
        _uiManager.CarUIInitialization(_carScript);
        _deathZone?.Init(_carScript, _uiManager);
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
