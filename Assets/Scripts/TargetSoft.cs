using System.Collections;
using UnityEngine;

// Cible "molle" (collision physique) qui se déplace horizontalement
// Retire des points au score quand elle est collectée par le joueur (pénalité)
public class TargetSoft : MonoBehaviour
{
    [Header("Score")]
    // Valeur en points ajoutée au score (négative pour pénalité)
    [SerializeField] private int _targetValue = -1;
    
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 2f;
    
    private MeshRenderer _meshRenderer;
    private Collider _collider;
    private Rigidbody _rigidbody;
    
    // Direction actuelle : -1 (gauche) ou 1 (droite)
    private float _currentDirection;
    
    // Flag pour éviter la double collecte si plusieurs collisions se produisent
    private bool _wasCollected = false;
    
    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
        
        // S'assure que le collider n'est PAS un trigger (collision physique solide)
        if (_collider != null)
        {
            _collider.isTrigger = false;
        }
        
        // Crée et configure un Rigidbody s'il n'existe pas déjà
        if (_rigidbody == null)
        {
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.freezeRotation = true;
            
            // Opérateur OR binaire (|) pour combiner plusieurs contraintes
            // Bloque : rotation complète + position Y (hauteur) + position Z (profondeur)
            // Permet seulement le mouvement horizontal (axe X)
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        }
        
        StartCoroutine(IgnoreOtherTargetsDelayed());
        ChooseRandomDirection();
    }
    
    // Coroutine qui désactive les collisions entre toutes les cibles du jeu
    // Le délai permet de s'assurer que tous les objets sont initialisés
    private IEnumerator IgnoreOtherTargetsDelayed()
    {
        // Attend que tous les objets de la scène soient initialisés
        yield return new WaitForSeconds(0.1f);
        
        // Trouve toutes les cibles molles dans la scène
        TargetSoft[] allTargetSofts = FindObjectsByType<TargetSoft>(FindObjectsSortMode.None);
        foreach (TargetSoft otherTarget in allTargetSofts)
        {
            // Ignore sa propre collision (otherTarget != this)
            if (otherTarget != this && otherTarget != null)
            {
                Collider otherCollider = otherTarget.GetComponent<Collider>();
                if (otherCollider != null && _collider != null)
                {
                    // Désactive la détection de collision physique entre ces deux objets
                    Physics.IgnoreCollision(_collider, otherCollider);
                }
            }
        }
        
        // Même processus pour ignorer les collisions avec les cibles dures
        TargetHard[] allTargetHards = FindObjectsByType<TargetHard>(FindObjectsSortMode.None);
        foreach (TargetHard otherTarget in allTargetHards)
        {
            if (otherTarget != null)
            {
                Collider otherCollider = otherTarget.GetComponent<Collider>();
                if (otherCollider != null && _collider != null)
                {
                    Physics.IgnoreCollision(_collider, otherCollider);
                }
            }
        }
    }
    
    // FixedUpdate pour le mouvement physique (intervalle fixe)
    private void FixedUpdate()
    {
        // Arrête le mouvement si la cible a déjà été collectée
        if (!_wasCollected && _rigidbody != null)
        {
            // Mouvement horizontal uniquement (Y et Z bloqués par les contraintes)
            Vector3 movement = new Vector3(_currentDirection * _moveSpeed, 0, 0);
            _rigidbody.linearVelocity = movement;
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        // Return précoce si déjà collectée (évite de traiter plusieurs collisions)
        if (_wasCollected) return;
        
        if (other.gameObject.CompareTag("Player"))
        {
            // Marque immédiatement comme collectée pour éviter les collectes multiples
            _wasCollected = true;
            
            Player_Collect playerCollect = other.gameObject.GetComponent<Player_Collect>();
            if (playerCollect != null)
            {
                // Met à jour le score avec une valeur NÉGATIVE (pénalité)
                playerCollect.UpdateScoreWithoutEvent(_targetValue);
                
                HideTarget();
                
                // Notifie le gestionnaire pour les statistiques globales
                if (TargetManager.Instance != null)
                {
                    TargetManager.Instance.OnTargetCollected(gameObject);
                }
            }
        }
        // Si collision avec un mur, inverse la direction
        else if (IsWall(other.gameObject))
        {
            // Multiplie par -1 pour inverser : 1 devient -1, -1 devient 1
            _currentDirection *= -1;
        }
    }
    
    // Désactive visuellement et physiquement la cible
    private void HideTarget()
    {
        if (_meshRenderer != null)
            _meshRenderer.enabled = false;
        if (_collider != null)
            _collider.enabled = false;
        if (_rigidbody != null)
            _rigidbody.linearVelocity = Vector3.zero;
    }
    
    private bool IsWall(GameObject obj)
    {
        return obj.name.Contains("Wall") || obj.name.Contains("Ground");
    }
    
    // Choisit aléatoirement une direction initiale (gauche ou droite)
    private void ChooseRandomDirection()
    {
        // Random.value retourne un float entre 0 et 1
        // < 0.5 donne 50% de chance pour chaque direction
        _currentDirection = UnityEngine.Random.value < 0.5f ? -1f : 1f;
    }
}
