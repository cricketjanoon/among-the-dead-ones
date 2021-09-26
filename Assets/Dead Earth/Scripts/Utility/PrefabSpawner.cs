using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab = null;

    [SerializeField]
    private List<Transform> _spawnPoints = new List<Transform>();

    // Use this for initialization
    private void Awake()
    {
        if (_spawnPoints.Count == 0 || _prefab == null)
            return;

        Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
        Instantiate(_prefab, spawnPoint.position, spawnPoint.rotation);
    }
}