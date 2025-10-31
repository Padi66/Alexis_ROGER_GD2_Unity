using UnityEngine;
using System.Collections.Generic;
public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _targetPrefabs;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private int _maxTargetsOnMap = 5;
    [SerializeField] private bool _spawnOnTargetCollected = true;
    
    private List<GameObject> _activeTargets = new List<GameObject>();

    private void Start()
    {
        SpawnInitialTargets();
    }

    private void OnEnable()
    {
        if (_spawnOnTargetCollected)
        {
            Player_Collect.OnTargetColleted += OnTargetCollected;
        }
    }

    private void OnDisable()
    {
        if (_spawnOnTargetCollected)
        {
            Player_Collect.OnTargetColleted -= OnTargetCollected;
        }
    }

    private void SpawnInitialTargets()
    {
        int targetsToSpawn = Mathf.Min(_maxTargetsOnMap, _spawnPoints.Length);
        
        for (int i = 0; i < targetsToSpawn; i++)
        {
            SpawnRandomTarget();
        }
    }

    private void OnTargetCollected(int score)
    {
        CleanupDestroyedTargets();
        
        if (_activeTargets.Count < _maxTargetsOnMap)
        {
            SpawnRandomTarget();
        }
    }

    private void SpawnRandomTarget()
    {
        if (_targetPrefabs == null || _targetPrefabs.Length == 0)
        {
            Debug.LogWarning("TargetSpawner: No target prefabs assigned!");
            return;
        }

        if (_spawnPoints == null || _spawnPoints.Length == 0)
        {
            Debug.LogWarning("TargetSpawner: No spawn points assigned!");
            return;
        }

        Transform spawnPoint = GetRandomAvailableSpawnPoint();
        if (spawnPoint == null)
        {
            Debug.LogWarning("TargetSpawner: No available spawn points!");
            return;
        }

        GameObject randomPrefab = _targetPrefabs[Random.Range(0, _targetPrefabs.Length)];
        GameObject spawnedTarget = Instantiate(randomPrefab, spawnPoint.position, spawnPoint.rotation);
        _activeTargets.Add(spawnedTarget);
        
        Debug.Log($"Spawned {randomPrefab.name} at {spawnPoint.name}");
    }

    private Transform GetRandomAvailableSpawnPoint()
    {
        List<Transform> availablePoints = new List<Transform>();

        foreach (Transform spawnPoint in _spawnPoints)
        {
            if (!IsSpawnPointOccupied(spawnPoint))
            {
                availablePoints.Add(spawnPoint);
            }
        }

        if (availablePoints.Count == 0)
            return null;

        return availablePoints[Random.Range(0, availablePoints.Count)];
    }

    private bool IsSpawnPointOccupied(Transform spawnPoint)
    {
        float checkRadius = 1f;
        Collider[] colliders = Physics.OverlapSphere(spawnPoint.position, checkRadius);
        
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Untagged") && (col.GetComponent<TargetHard>() != null || col.GetComponent<TargetSoft>() != null))
            {
                return true;
            }
        }
        
        return false;
    }

    private void CleanupDestroyedTargets()
    {
        _activeTargets.RemoveAll(target => target == null);
    }
}
