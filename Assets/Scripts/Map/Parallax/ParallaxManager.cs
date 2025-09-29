using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    [SerializeField] private ParallaxLayer[] _layers;
    [SerializeField] private Transform _startTransform;

    private Camera _camera;

    private class LayerInstance
    {
        public GameObject Obj;
        public Vector3 InitialPosition;
        public float Width;
    }

    private List<List<LayerInstance>> _spawnedLayers = new();
    private List<ParallaxLayer> _spawnedLayerData = new();

    public void Init(Camera camera)
    {
        _camera = camera;
        Vector3 camPos = _camera.transform.position;
        camPos.z = 0;

        _spawnedLayers.Clear();
        _spawnedLayerData.Clear();

        foreach (var layer in _layers)
        {
            if (layer == null || layer.Prefab == null) continue;

            List<LayerInstance> instances = new();
            float spriteWidth = 1f;

            var sr = layer.Prefab.GetComponent<SpriteRenderer>();
            if (sr != null)
                spriteWidth = sr.bounds.size.x;

            for (int i = 0; i < layer.Copies; i++)
            {
                Vector3 pos = _startTransform.position + new Vector3(i * spriteWidth, 0, 0);
                var obj = Instantiate(layer.Prefab, pos, Quaternion.identity);
                obj.name = $"{layer.Prefab.name}_copy{i}";

                instances.Add(new LayerInstance
                {
                    Obj = obj,
                    InitialPosition = pos,
                    Width = spriteWidth
                });
            }

            _spawnedLayers.Add(instances);
            _spawnedLayerData.Add(layer);
        }
    }

    private void LateUpdate()
    {
        if (_camera == null) return;

        Vector3 camPos = _camera.transform.position;
        camPos.z = 0;

        for (int l = 0; l < _spawnedLayers.Count; l++)
        {
            var instances = _spawnedLayers[l];
            var layer = _spawnedLayerData[l];
            float factor = layer.ParallaxFactor;

            foreach (var instance in instances)
            {
                Vector3 pos = instance.InitialPosition + new Vector3(camPos.x * factor, camPos.y * factor, 0);
                instance.Obj.transform.position = pos;
            }

            for (int i = 0; i < instances.Count; i++)
            {
                var instance = instances[i];
                if (camPos.x * factor - instance.Obj.transform.position.x > instance.Width)
                {
                    float maxX = float.MinValue;
                    foreach (var inst in instances)
                        maxX = Mathf.Max(maxX, inst.Obj.transform.position.x);

                    instance.InitialPosition += new Vector3(maxX - instance.Obj.transform.position.x + instance.Width, 0, 0);
                }
            }
        }
    }
}
