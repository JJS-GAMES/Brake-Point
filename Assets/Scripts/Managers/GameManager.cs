using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager; // Temporary initialization in the inspector / Временно инициализация в инспекторе

    [SerializeField,Tooltip("Player car / Машина игрока")] 
    private Car _car;

    [Header("Zones / Зоны")]
    [Space, SerializeField, Tooltip("Prefab of the start area on the stage / Префаб стартовой зоны на сцене")]
    private StartZone _startZone;
    [SerializeField, Tooltip("Prefab of the finish area on the stage / Префаб финишной зоны на сцене")] 
    private FinishZone _finishZone;

    private float _totalDistance;

    private void Start()
    {
        // We calculate and save the total distance between start and finish
        // Считаем и сохраняем общее расстояние между стартом и финишем

        _totalDistance = Vector2.Distance(_startZone.transform.position,_finishZone.gameObject.transform.position);

        _startZone.Init(_car);
        _finishZone.Init(_car, _uiManager);
    }

    public void RestartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public float CalculateLevelProgress()
    {
        // We count how much the car has traveled
        // Считаем, сколько проехала машина

        float carDistance = Vector2.Distance(_startZone.gameObject.transform.position, _car.Target.transform.position);

        float progress = Mathf.Clamp01(carDistance / _totalDistance);

        return progress;
    }
}
