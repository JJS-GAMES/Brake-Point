using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Button _respawnButton; // ��������� � UIManager

    private void Start()
    {
        _respawnButton.onClick.RemoveAllListeners();
        _respawnButton.onClick.AddListener(RestartButton);
    }
    private void RestartButton()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
