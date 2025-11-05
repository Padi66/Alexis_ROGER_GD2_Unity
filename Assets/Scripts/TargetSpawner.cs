using UnityEngine;
using System.Collections.Generic;

// Spawner alternatif qui gère le spawn continu de cibles
// Contrairement au TargetManager qui respawn tout d'un coup, celui-ci spawne une cible à la fois
public class TargetSpawner : MonoBehaviour
{
    // Tableau de prefabs de cibles possibles (peut contenir TargetHard, TargetSoft, etc.)
    [SerializeField] private GameObject[] _targetPrefabs;
    
    // Points où les cibles peuvent apparaître
    [SerializeField] private Transform[] _spawnPoints;
    
    // Nombre maximum de cibles autorisées simultanément sur la carte
    [SerializeField] private int _maxTargetsOnMap = 5;
    
    // Si vrai, spawne automatiquement une nouvelle cible quand une est collectée
    [SerializeField] private bool _spawnOnTargetCollected = true;
    
    // Liste dynamique de toutes les cibles actuellement actives
    // Permet de suivre combien sont en jeu et de nettoyer celles détruites
    private List<GameObject> _activeTargets = new List<GameObject>();

    private void Start()
    {
        SpawnInitialTargets();
    }

    // S'abonne à l'événement de collecte quand le composant est activé
    private void OnEnable()
    {
        if (_spawnOnTargetCollected)
        {
            // S'abonne à l'événement statique du Player_Collect
            Player_Collect.OnTargetColleted += OnTargetCollected;
        }
    }

    // Se désabonne de l'événement quand le composant est désactivé
    // CRITIQUE pour éviter les erreurs si l'objet est détruit
    private void OnDisable()
    {
        if (_spawnOnTargetCollected)
        {
            // Se désabonne pour éviter d'appeler une méthode sur un objet détruit
            Player_Collect.OnTargetColleted -= OnTargetCollected;
        }
    }

    // Spawne le nombre initial de cibles au démarrage
    private void SpawnInitialTargets()
    {
        // Mathf.Min garantit qu'on ne spawne jamais plus que de points disponibles
        int targetsToSpawn = Mathf.Min(_maxTargetsOnMap, _spawnPoints.Length);
        
        for (int i = 0; i < targetsToSpawn; i++)
        {
            SpawnRandomTarget();
        }
    }

    // Callback appelé automatiquement via l'événement quand une cible est collectée
    private void OnTargetCollected(int score)
    {
        // Nettoie la liste en retirant les références nulles (cibles détruites)
        CleanupDestroyedTargets();
        
        // Spawne une nouvelle cible uniquement si on est en dessous de la limite
        // Maintient un nombre constant de cibles sur la carte
        if (_activeTargets.Count < _maxTargetsOnMap)
        {
            SpawnRandomTarget();
        }
    }

    // Spawne une cible aléatoire à un point de spawn disponible
    private void SpawnRandomTarget()
    {
        // Validation : vérifie que les prefabs sont assignés
        if (_targetPrefabs == null || _targetPrefabs.Length == 0)
        {
            Debug.LogWarning("TargetSpawner: No target prefabs assigned!");
            return;
        }

        // Validation : vérifie que les points de spawn sont assignés
        if (_spawnPoints == null || _spawnPoints.Length == 0)
        {
            Debug.LogWarning("TargetSpawner: No spawn points assigned!");
            return;
        }

        // Trouve un point de spawn libre (pas déjà occupé par une cible)
        Transform spawnPoint = GetRandomAvailableSpawnPoint();
        if (spawnPoint == null)
        {
            Debug.LogWarning("TargetSpawner: No available spawn points!");
            return;
        }

        // Random.Range avec des entiers : maximum est EXCLUSIF
        // Random.Range(0, 5) retourne 0, 1, 2, 3, ou 4 (jamais 5)
        GameObject randomPrefab = _targetPrefabs[Random.Range(0, _targetPrefabs.Length)];
        
        // Instancie le prefab choisi au point de spawn
        GameObject spawnedTarget = Instantiate(randomPrefab, spawnPoint.position, spawnPoint.rotation);
        
        // Ajoute à la liste pour le tracking
        _activeTargets.Add(spawnedTarget);
        
        Debug.Log($"Spawned {randomPrefab.name} at {spawnPoint.name}");
    }

    // Filtre les points de spawn pour ne retourner qu'un point libre (non occupé)
    private Transform GetRandomAvailableSpawnPoint()
    {
        // Crée une liste temporaire pour stocker uniquement les points disponibles
        List<Transform> availablePoints = new List<Transform>();

        // Parcourt tous les points et garde uniquement ceux qui sont libres
        foreach (Transform spawnPoint in _spawnPoints)
        {
            if (!IsSpawnPointOccupied(spawnPoint))
            {
                availablePoints.Add(spawnPoint);
            }
        }

        // Si aucun point n'est disponible, retourne null
        if (availablePoints.Count == 0)
            return null;

        // Retourne un point aléatoire parmi ceux disponibles
        return availablePoints[Random.Range(0, availablePoints.Count)];
    }

    // Vérifie si un point de spawn est déjà occupé par une cible
    private bool IsSpawnPointOccupied(Transform spawnPoint)
    {
        float checkRadius = 1f;
        
        // Physics.OverlapSphere crée une sphère invisible et retourne tous les colliders dedans
        // Très utile pour détecter la présence d'objets dans une zone
        Collider[] colliders = Physics.OverlapSphere(spawnPoint.position, checkRadius);
        
        // Parcourt tous les colliders trouvés dans la sphère
        foreach (Collider col in colliders)
        {
            // Vérifie si l'objet est une cible (Hard ou Soft)
            // Double condition : doit avoir le tag "Untagged" ET être une cible
            if (col.CompareTag("Untagged") && (col.GetComponent<TargetHard>() != null || col.GetComponent<TargetSoft>() != null))
            {
                return true; // Point occupé
            }
        }
        
        return false; // Point libre
    }

    // Nettoie la liste en retirant toutes les références nulles
    // Les cibles détruites deviennent null dans la liste
    private void CleanupDestroyedTargets()
    {
        // RemoveAll utilise un lambda (expression anonyme) pour filtrer
        // Retire tous les éléments où la condition est vraie (target == null)
        _activeTargets.RemoveAll(target => target == null);
    }
}
