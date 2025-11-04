using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TargetManager : MonoBehaviour
{
 public static TargetManager Instance { get; private set; }
    
    [Header("Prefabs")]
    [SerializeField] private GameObject _targetHardPrefab;
    [SerializeField] private GameObject _targetSoftPrefab;
    
    [Header("Settings")]
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private int _numberOfTargets = 5;
    [SerializeField] private float _respawnDelay = 3f;
    
    private List<GameObject> _activeTargets = new List<GameObject>();
    private bool _isRespawning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SpawnInitialTargets();
    }

    private void SpawnInitialTargets()
    {
        int targetsToSpawn = Mathf.Min(_numberOfTargets, _spawnPoints.Length);
        
        for (int i = 0; i < targetsToSpawn; i++)
        {
            SpawnRandomTargetAt(i);
        }
    }

    public void OnTargetCollected(GameObject collectedTarget)
    {
        if (_isRespawning) return;
        
        StartCoroutine(DestroyAllAndRespawn());
    }

    private IEnumerator DestroyAllAndRespawn()
    {
        _isRespawning = true;
        
        foreach (GameObject target in _activeTargets)
        {
            if (target != null)
            {
                Destroy(target);
            }
        }
        
        _activeTargets.Clear();
        
        yield return new WaitForSeconds(_respawnDelay);
        
        int targetsToSpawn = Mathf.Min(_numberOfTargets, _spawnPoints.Length);
        for (int i = 0; i < targetsToSpawn; i++)
        {
            SpawnRandomTargetAt(i);
        }
        
        _isRespawning = false;
    }

    private void SpawnRandomTargetAt(int spawnIndex)
    {
        if (spawnIndex >= _spawnPoints.Length) return;
        
        GameObject prefabToSpawn = Random.value < 0.5f ? _targetHardPrefab : _targetSoftPrefab;
        
        if (prefabToSpawn == null)
        {
            Debug.LogWarning($"TargetManager: Prefab not assigned!");
            return;
        }
        
        Transform spawnPoint = _spawnPoints[spawnIndex];
        GameObject newTarget = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
        _activeTargets.Add(newTarget);
        
        Debug.Log($"Spawned {prefabToSpawn.name} at {spawnPoint.name}");
    }
}
