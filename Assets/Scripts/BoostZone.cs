using UnityEngine;

// Gère une zone de boost qui améliore temporairement les capacités du joueur
// Augmente la vitesse et la hauteur de saut lorsque le joueur entre dans la zone
public class BoostZone : MonoBehaviour
{
    [Header("Boost Settings")]
    // Multiplicateur appliqué à la vitesse du joueur (ex: 2.0 = deux fois plus rapide)
    [SerializeField] private float _speedMultiplier = 2.0f;
    
    // Multiplicateur appliqué à la hauteur de saut (ex: 1.5 = saute 50% plus haut)
    [SerializeField] private float _jumpMultiplier = 1.5f;
    
    // Durée du boost en secondes
    [SerializeField] private float _boostDuration = 5.0f;
    
    [Header("Visual Feedback")]
    // Couleur de la zone de boost (par défaut jaune pour la rendre visible)
    [SerializeField] private Color _boostColor = Color.yellow;
    
    // Son joué lorsque le joueur entre dans la zone
    [SerializeField] private AudioClip _boostSound;
    
    // Référence à l'AudioSource pour jouer les sons
    private AudioSource _audioSource;
    
    // Initialisation au démarrage
    private void Start()
    {
        // Récupère l'AudioSource pour jouer les sons de boost
        _audioSource = GetComponent<AudioSource>();
        
        // S'assure que le Collider est configuré en tant que trigger
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
        
        // Applique la couleur configurée au Renderer pour identifier visuellement la zone
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = _boostColor;
        }
    }
    
    // Détecte quand un objet entre dans la zone de boost
    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si c'est le joueur qui entre dans la zone
        if (other.CompareTag("Player"))
        {
            // Récupère le composant PlayerBoost du joueur
            PlayerBoost playerBoost = other.GetComponent<PlayerBoost>();
            if (playerBoost != null)
            {
                // Applique le boost avec les multiplicateurs et la durée configurés
                playerBoost.ApplyBoost(_speedMultiplier, _jumpMultiplier, _boostDuration);
                
                // Joue le son de boost si assigné
                if (_boostSound != null && _audioSource != null)
                {
                    _audioSource.PlayOneShot(_boostSound);
                }
            }
        }
    }
}
