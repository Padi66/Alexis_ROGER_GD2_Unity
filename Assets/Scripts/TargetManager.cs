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
    private bool _isDisabled = false;

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
        if (_isRespawning || _isDisabled)
        {
            return;
        }
        
        Debug.Log($"TargetManager: Target collected, starting respawn sequence");
        StartCoroutine(DestroyAllAndRespawn());
    }
    
    public void DisableAllTargets()
    {
        _isDisabled = true;
        
        StopAllCoroutines();
        
        Debug.Log($"TargetManager: Disabling all targets permanently");
        
        foreach (GameObject target in _activeTargets)
        {
            if (target != null)
            {
                Destroy(target);
            }
        }
        
        _activeTargets.Clear();
    }

    private IEnumerator DestroyAllAndRespawn()
    {
        _isRespawning = true;
        
        Debug.Log($"TargetManager: Destroying {_activeTargets.Count} targets");
        
        foreach (GameObject target in _activeTargets)
        {
            if (target != null)
            {
                Destroy(target);
            }
        }
        
        _activeTargets.Clear();
        
        Debug.Log($"TargetManager: Waiting {_respawnDelay} seconds before respawn");
        yield return new WaitForSeconds(_respawnDelay);
        
        if (_isDisabled)
        {
            Debug.Log("TargetManager: Disabled, not respawning");
            yield break;
        }
        
        Debug.Log("TargetManager: Starting respawn");
        int targetsToSpawn = Mathf.Min(_numberOfTargets, _spawnPoints.Length);
        for (int i = 0; i < targetsToSpawn; i++)
        {
            SpawnRandomTargetAt(i);
        }
        
        Debug.Log($"TargetManager: Respawned {_activeTargets.Count} targets");
        _isRespawning = false;
    }

    private void SpawnRandomTargetAt(int spawnIndex)
    {
        if (spawnIndex >= _spawnPoints.Length)
        {
            Debug.LogWarning($"TargetManager: Spawn index {spawnIndex} out of range");
            return;
        }
        
        bool spawnHard = UnityEngine.Random.value < 0.5f;
        GameObject prefabToSpawn = spawnHard ? _targetHardPrefab : _targetSoftPrefab;
        
        if (prefabToSpawn == null)
        {
            Debug.LogWarning($"TargetManager: Prefab not assigned! (Hard: {_targetHardPrefab != null}, Soft: {_targetSoftPrefab != null})");
            return;
        }
        
        Transform spawnPoint = _spawnPoints[spawnIndex];
        GameObject newTarget = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
        _activeTargets.Add(newTarget);
        
        string targetType = spawnHard ? "TargetHard" : "TargetSoft";
        Debug.Log($"TargetManager: Spawned {targetType} at {spawnPoint.name} (Total: {_activeTargets.Count})");
    }
}
