using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager; // Temporary initialization in the inspector / �������� ������������� � ����������

    [SerializeField,Tooltip("Player car / ������ ������")] 
    private Car _car;

    [Header("Zones / ����")]
    [Space, SerializeField, Tooltip("Prefab of the start area on the stage / ������ ��������� ���� �� �����")]
    private GameObject _startZone;
    [SerializeField, Tooltip("Prefab of the finish area on the stage / ������ �������� ���� �� �����")] 
    private FinishZone _finishZone;

    private Transform _startZoneTransform;
    private Transform _finishZoneTransform;

    private float _totalDistance;

    private void Start()
    {
        _uiManager.Init(this, _car);

        _startZoneTransform = _startZone.transform;
        _finishZoneTransform = _finishZone.transform;

        // We calculate and save the total distance between start and finish
        // ������� � ��������� ����� ���������� ����� ������� � �������

        _totalDistance = Vector2.Distance(_startZoneTransform.position, _finishZoneTransform.position);

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
        // �������, ������� �������� ������

        float carDistance = Vector2.Distance(_startZoneTransform.position, _car.Target.transform.position);

        float progress = Mathf.Clamp01(carDistance / _totalDistance);

        return progress;
    }
}
