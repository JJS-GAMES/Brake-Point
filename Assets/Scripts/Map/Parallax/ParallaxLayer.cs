using System;
using UnityEngine;

[Serializable]
public class ParallaxLayer
{
    public GameObject ParallaxPrefab => _parallaxPrefab;
    public float ParallaxFactor => _parallaxFactor;

    [SerializeField] private GameObject _parallaxPrefab;
    [Range(0f, 1f), SerializeField] private float _parallaxFactor = 1f;
}
