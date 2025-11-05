using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    // Tableau de murs qui seront affectés par cette plaque de pression
    // Permet de contrôler plusieurs murs simultanément avec une seule plaque
    [SerializeField] private DisappearingWall[] _wallsToControl;
    
    // Temps d'attente avant que la plaque puisse être réactivée
    [SerializeField] private float _cooldown = 1f;
    
    [SerializeField] private Material _activatedMaterial;
    [SerializeField] private Material _defaultMaterial;
    
    private MeshRenderer _meshRenderer;
    
    // Stocke le temps (Time.time) de la dernière activation
    // Permet de calculer si le cooldown est écoulé
    private float _lastActivationTime;
    
    // Flag pour empêcher les activations multiples pendant le cooldown
    private bool _isActivated;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        
        // S'assure automatiquement que le Collider est configuré en trigger
        Collider collider = GetComponent<Collider>();
        if (collider != null && !collider.isTrigger)
        {
            collider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Triple condition de sécurité pour l'activation
        if (other.CompareTag("Player") && !_isActivated)
        {
            // Vérifie que le temps écoulé depuis la dernière activation >= cooldown
            // Time.time retourne les secondes depuis le démarrage du jeu
            if (Time.time - _lastActivationTime >= _cooldown)
            {
                ActivatePlate();
            }
        }
    }

    private void ActivatePlate()
    {
        // Enregistre le moment exact de l'activation pour le calcul du cooldown
        _lastActivationTime = Time.time;
        _isActivated = true;
        
        // Feedback visuel : change le matériau pour indiquer l'activation
        if (_meshRenderer != null && _activatedMaterial != null)
        {
            _meshRenderer.material = _activatedMaterial;
        }
        
        // Boucle à travers tous les murs contrôlés et les fait disparaître
        // foreach est optimal pour parcourir des tableaux sans se soucier de l'index
        foreach (DisappearingWall wall in _wallsToControl)
        {
            if (wall != null)
            {
                wall.Disappear();
            }
        }
        
        Debug.Log($"{gameObject.name} activated!");
        
        // Invoke() appelle une méthode après un délai (en secondes)
        // nameof() évite les erreurs de frappe en utilisant le nom de la méthode directement
        Invoke(nameof(ResetPlate), _cooldown);
    }

    // Méthode appelée automatiquement après le cooldown via Invoke()
    private void ResetPlate()
    {
        // Réinitialise l'état pour permettre une nouvelle activation
        _isActivated = false;
        
        // Feedback visuel : restaure le matériau par défaut
        if (_meshRenderer != null && _defaultMaterial != null)
        {
            _meshRenderer.material = _defaultMaterial;
        }
    }
}
