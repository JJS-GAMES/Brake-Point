using UnityEngine;

public class SelectLevelButton : MonoBehaviour
{
    [SerializeField] private int _sceneIndex;

    public int GetLevelIndex => _sceneIndex;
}
