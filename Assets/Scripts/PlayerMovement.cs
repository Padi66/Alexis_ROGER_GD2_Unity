using UnityEngine;
using UnityEngine.InputSystem;

// Attribut qui garantit qu'un Rigidbody est toujours présent sur le GameObject
// Unity ajoutera automatiquement un Rigidbody si absent lors de l'ajout de ce script
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    private float _horizontalMovement;
    private float _verticalMovement;
    private Vector3 _grappinDirection;
    private Vector3 _grappinHit; 
    
    [Header("Movement Settings")]
    [SerializeField] private float _speed = 2.0f;
    
    [Header("Jump Settings")]
    [SerializeField] private float _jumpForce = 5.0f;
    [SerializeField] private float _groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask _groundLayer;
    
    // Multiplicateurs pour une physique de saut plus réaliste et réactive
    [SerializeField] private float _fallMultiplier = 2.5f;
    [SerializeField] private float _lowJumpMultiplier = 2f;
    
    private Vector3 _movement;
    private bool _isGrounded;
    
    // Flag pour synchroniser l'input (Update) avec la physique (FixedUpdate)
    // Évite de perdre l'input de saut entre deux frames physiques
    private bool _jumpRequested;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    // Update() pour les inputs car il s'exécute à chaque frame de rendu
    // Garantit qu'aucun input n'est manqué même avec des framerates variables
    void Update()
    {
        _horizontalMovement = Input.GetAxis("Horizontal");
        _verticalMovement = Input.GetAxis("Vertical");
        
        // GetButtonDown ne dure qu'une frame, donc on sauvegarde l'intention de sauter
        if (Input.GetButtonDown("Jump"))
        {
            _jumpRequested = true;
        }
        
        CheckGroundStatus();
    }
    
    // FixedUpdate() pour la physique car il s'exécute à intervalles fixes (0.02s par défaut)
    // Garantit une physique stable et prévisible
    void FixedUpdate()
    {
        // Construit le vecteur de mouvement horizontal (pas de Y car géré par la physique)
        _movement = new Vector3(_horizontalMovement, 0f, _verticalMovement);
        
        // Normalize() empêche le mouvement diagonal d'être plus rapide
        // Sans normalisation: diagonal = sqrt(1² + 1²) ≈ 1.41 fois plus rapide
        _movement.Normalize();
        _movement *= _speed;
        
        // Préserve la vélocité verticale (gravité et saut) en la réappliquant
        // Sinon le mouvement horizontal écraserait la vélocité Y
        _movement.y = _rb.linearVelocity.y;
    
        if (_rb != null)
        {
            _rb.linearVelocity = _movement;
        
            // Consomme le flag de saut seulement si le joueur est au sol
            if (_jumpRequested && _isGrounded)
            {
                Jump();
            }
        
            ApplyFallGravity();
        
            // Réinitialise le flag après traitement pour éviter les sauts répétés
            _jumpRequested = false;
        }
        else
        {
            Debug.LogError("No RigidBody found !");
        }
    }

    
    private void Jump()
    {
        // Annule d'abord la vélocité verticale actuelle pour un saut constant
        // Sans cela, sauter en descendant une pente donnerait un saut plus faible
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        
        // ForceMode.Impulse applique une force instantanée (prend en compte la masse)
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
    
    private void CheckGroundStatus()
    {
        // Raycast vers le bas pour détecter le sol
        // Utilise un LayerMask pour ignorer les objets non-sol (joueur, ennemis, etc.)
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _groundCheckDistance + 0.5f, _groundLayer);
    }
    
    // Améliore la sensation de saut en modifiant la gravité selon la situation
    private void ApplyFallGravity()
    {
        // Si le joueur tombe (vélocité Y négative)
        if (_rb.linearVelocity.y < 0)
        {
            // Applique une gravité supplémentaire pour une chute plus rapide et réactive
            // (_fallMultiplier - 1) car la gravité de base est déjà appliquée par Unity
            _rb.linearVelocity += Vector3.up * Physics.gravity.y * (_fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        // Si le joueur monte (vélocité Y positive) mais ne maintient plus le bouton de saut
        else if (_rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            // Applique une gravité supplémentaire pour arrêter la montée plus vite
            // Permet des sauts de hauteur variable selon la durée de pression du bouton
            _rb.linearVelocity += Vector3.up * Physics.gravity.y * (_lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
    
    public float GetSpeed()
    {
        return _speed;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public float GetJumpForce()
    {
        return _jumpForce;
    }

    public void SetJumpForce(float jumpForce)
    {
        _jumpForce = jumpForce;
    }
}
