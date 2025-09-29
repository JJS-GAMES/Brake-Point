using System.Collections.Generic;
using UnityEngine;
public class ParallaxManager : MonoBehaviour
{
    [SerializeField] private ParallaxLayer[] _layers;

    private Camera _camera;
    private List<GameObject> _spawnedLayers = new();
    private Vector3[] _initialPositions;

    public void Init(Camera camera)
    {
        _camera = camera;
        Vector3 camPos = _camera.transform.position;
        camPos.z = 0;

        _spawnedLayers.Clear();
        _initialPositions = new Vector3[_layers.Length];

        for (int i = 0; i < _layers.Length; i++)
        {
            var layer = _layers[i];
            if (layer == null || layer.ParallaxPrefab == null) continue;

            var newLayer = Instantiate(layer.ParallaxPrefab, camPos, Quaternion.identity);
            newLayer.name = $"{layer.ParallaxPrefab.name}_instance";

            _spawnedLayers.Add(newLayer);
            _initialPositions[i] = newLayer.transform.position;
        }
    }

    private void LateUpdate()
    {
        if (_camera == null) return;

        Vector3 camPos = _camera.transform.position;
        camPos.z = 0;

        for (int i = 0; i < _spawnedLayers.Count; i++)
        {
            var layer = _spawnedLayers[i];
            float factor = _layers[i].ParallaxFactor;

            layer.transform.position = _initialPositions[i] + new Vector3(camPos.x * factor, camPos.y * factor, 0);
        }
    }
}
