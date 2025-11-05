using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gestionnaire centralisé pour toutes les cibles du jeu
// Gère le spawn initial, la destruction et le respawn automatique après collecte
public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance { get; private set; }
    
    [Header("Prefabs")]
    // Prefabs des deux types de cibles à instancier
    [SerializeField] private GameObject _targetHardPrefab;
    [SerializeField] private GameObject _targetSoftPrefab;
    
    [Header("Settings")]
    // Points de spawn où les cibles peuvent apparaître
    [SerializeField] private Transform[] _spawnPoints;
    
    // Nombre de cibles à spawner simultanément
    [SerializeField] private int _numberOfTargets = 5;
    
    // Délai d'attente avant de respawner les cibles après qu'une soit collectée
    [SerializeField] private float _respawnDelay = 3f;
    
    // Liste dynamique de toutes les cibles actuellement en jeu
    // Utilisé pour les détruire toutes lors du respawn
    private List<GameObject> _activeTargets = new List<GameObject>();
    
    // Flag pour éviter de lancer plusieurs séquences de respawn simultanément
    private bool _isRespawning = false;
    
    // Flag pour désactiver définitivement le système de respawn
    private bool _isDisabled = false;

    // Awake s'exécute avant Start, idéal pour initialiser le Singleton
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // Si une instance existe déjà, détruit ce doublon
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SpawnInitialTargets();
    }

    // Spawn le nombre configuré de cibles au démarrage
    private void SpawnInitialTargets()
    {
        // Mathf.Min garantit qu'on ne spawne jamais plus que de points disponibles
        // Évite les erreurs si _numberOfTargets > _spawnPoints.Length
        int targetsToSpawn = Mathf.Min(_numberOfTargets, _spawnPoints.Length);
        
        for (int i = 0; i < targetsToSpawn; i++)
        {
            SpawnRandomTargetAt(i);
        }
    }

    // Méthode publique appelée par les cibles quand elles sont collectées
    public void OnTargetCollected(GameObject collectedTarget)
    {
        // Sort immédiatement si un respawn est déjà en cours ou désactivé
        if (_isRespawning || _isDisabled)
        {
            return;
        }
        
        Debug.Log($"TargetManager: Target collected, starting respawn sequence");
        
        // Lance la séquence : détruit TOUTES les cibles puis les respawn toutes
        StartCoroutine(DestroyAllAndRespawn());
    }
    
    // Désactive définitivement le système (utile en fin de niveau par exemple)
    public void DisableAllTargets()
    {
        _isDisabled = true;
        
        // Arrête toutes les coroutines en cours (notamment le respawn)
        StopAllCoroutines();
        
        Debug.Log($"TargetManager: Disabling all targets permanently");
        
        // Détruit toutes les cibles actives
        foreach (GameObject target in _activeTargets)
        {
            if (target != null)
            {
                Destroy(target);
            }
        }
        
        // Vide la liste pour libérer la mémoire
        _activeTargets.Clear();
    }

    // Coroutine qui gère la séquence complète : destruction → attente → respawn
    private IEnumerator DestroyAllAndRespawn()
    {
        // Active le flag pour empêcher les respawns multiples simultanés
        _isRespawning = true;
        
        Debug.Log($"TargetManager: Destroying {_activeTargets.Count} targets");
        
        // Détruit TOUTES les cibles actives, pas seulement celle collectée
        // Crée un "reset" complet pour renouveler le gameplay
        foreach (GameObject target in _activeTargets)
        {
            if (target != null)
            {
                Destroy(target);
            }
        }
        
        // Clear() vide la liste pour pouvoir la remplir avec les nouvelles cibles
        _activeTargets.Clear();
        
        Debug.Log($"TargetManager: Waiting {_respawnDelay} seconds before respawn");
        
        // Attend le délai configuré avant de respawner
        yield return new WaitForSeconds(_respawnDelay);
        
        // Vérifie si le système a été désactivé pendant l'attente
        if (_isDisabled)
        {
            Debug.Log("TargetManager: Disabled, not respawning");
            yield break; // Termine la coroutine sans respawner
        }
        
        Debug.Log("TargetManager: Starting respawn");
        
        // Respawn le même nombre de cibles qu'au début
        int targetsToSpawn = Mathf.Min(_numberOfTargets, _spawnPoints.Length);
        for (int i = 0; i < targetsToSpawn; i++)
        {
            SpawnRandomTargetAt(i);
        }
        
        Debug.Log($"TargetManager: Respawned {_activeTargets.Count} targets");
        
        // Désactive le flag pour permettre un nouveau respawn si une cible est collectée
        _isRespawning = false;
    }

    // Spawne aléatoirement une cible dure ou molle à un point de spawn spécifique
    private void SpawnRandomTargetAt(int spawnIndex)
    {
        // Validation : vérifie que l'index est dans les limites du tableau
        if (spawnIndex >= _spawnPoints.Length)
        {
            Debug.LogWarning($"TargetManager: Spawn index {spawnIndex} out of range");
            return;
        }
        
        // Random.value retourne un float entre 0 et 1
        // < 0.5 donne 50% de chance pour chaque type de cible
        bool spawnHard = UnityEngine.Random.value < 0.5f;
        
        // Opérateur ternaire : condition ? valeurSiVrai : valeurSiFaux
        GameObject prefabToSpawn = spawnHard ? _targetHardPrefab : _targetSoftPrefab;
        
        // Sécurité : vérifie que le prefab est assigné dans l'Inspector
        if (prefabToSpawn == null)
        {
            Debug.LogWarning($"TargetManager: Prefab not assigned! (Hard: {_targetHardPrefab != null}, Soft: {_targetSoftPrefab != null})");
            return;
        }
        
        Transform spawnPoint = _spawnPoints[spawnIndex];
        
        // Instantiate crée une copie du prefab dans la scène
        // Copie la position et rotation du point de spawn
        GameObject newTarget = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
        
        // Ajoute la nouvelle cible à la liste pour tracking
        _activeTargets.Add(newTarget);
        
        string targetType = spawnHard ? "TargetHard" : "TargetSoft";
        Debug.Log($"TargetManager: Spawned {targetType} at {spawnPoint.name} (Total: {_activeTargets.Count})");
    }
}
