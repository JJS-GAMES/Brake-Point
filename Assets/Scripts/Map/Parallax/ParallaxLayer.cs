using UnityEngine;

[System.Serializable]
public class ParallaxLayer
{
    public GameObject Prefab => _prefab;
    public float ParallaxFactor => _parallaxFactor;
    public int Copies => _copies;

    [SerializeField] private GameObject _prefab;
    [Range(0f, 1f), SerializeField] private float _parallaxFactor = 0.5f;
    [SerializeField] private int _copies = 2;
}