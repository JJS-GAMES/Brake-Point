using UnityEngine;

public class SelectCarButton : MonoBehaviour
{
    [SerializeField] private GameObject _carPrefab;
    public GameObject GetCarPrefab => _carPrefab;
}
