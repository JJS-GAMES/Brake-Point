using UnityEngine;

public class MM_Manager : MonoBehaviour
{
    [SerializeField] private MM_UIManager _uiManager;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private GameData _gameData;

    private void Start()
    {
        if(_levelManager == null) _levelManager = GetComponent<LevelManager>();

        _uiManager.Init(_gameData, _levelManager);
    }
}
