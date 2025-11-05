using UnityEngine;
using System.Collections;

// Gère le comportement d'une porte verrouillée qui nécessite une carte d'accès pour s'ouvrir
// La porte change de couleur selon son état (verrouillée/déverrouillée) et peut consommer la carte
public class AccessDoor : MonoBehaviour
{
    // La carte d'accès requise pour déverrouiller cette porte
    [SerializeField] private AccessCardData _requiredCard;
    
    // Objet visuel de la porte (peut être différent du GameObject principal)
    [SerializeField] private GameObject _doorVisual;
    
    // Si vrai, la carte sera retirée de l'inventaire après utilisation
    [SerializeField] private bool _consumeCard = false;
    
    // Délai en secondes avant que la porte ne s'ouvre après déverrouillage
    [SerializeField] private float _openDelay = 2f;
    
    // Couleur de la porte quand elle est verrouillée
    [SerializeField] private Color _lockedColor = Color.red;
    
    // Couleur de la porte quand elle est déverrouillée
    [SerializeField] private Color _unlockedColor = Color.green;
    
    [Header("Audio")]
    // Son joué quand la porte est déverrouillée avec succès
    [SerializeField] private AudioClip _unlockSound;
    
    // Son joué quand l'accès est refusé (pas la bonne carte)
    [SerializeField] private AudioClip _deniedSound;
    
    // Volume des sons (0 à 3)
    [SerializeField] [Range(0f, 3f)] private float _soundVolume = 1.5f;
    
    // Référence au Renderer pour changer la couleur de la porte
    private Renderer _renderer;
    
    // État de la porte : est-elle déverrouillée ?
    private bool _isUnlocked = false;
    
    // État du processus d'ouverture : la porte est-elle en train de s'ouvrir ?
    private bool _isOpening = false;
    
    // Initialisation au démarrage
    private void Start()
    {
        // Récupère le Renderer pour pouvoir changer la couleur
        _renderer = GetComponent<Renderer>();
        
        // Applique la couleur initiale (verrouillée)
        UpdateVisual();
    }
    
    // Détecte quand un objet entre dans le trigger de la porte
    private void OnTriggerEnter(Collider other)
    {
        // Si c'est le joueur et que la porte n'est ni déverrouillée ni en cours d'ouverture
        if (other.CompareTag("Player") && !_isUnlocked && !_isOpening)
        {
            // Tente de déverrouiller la porte
            TryUnlock();
        }
    }
    
    // Vérifie si le joueur possède la bonne carte et déverrouille la porte si c'est le cas
    private void TryUnlock()
    {
        // Vérifie que l'InventoryManager existe et que le joueur possède la carte requise
        if (InventoryManager.Instance != null && InventoryManager.Instance.HasCard(_requiredCard))
        {
            // Déverrouille la porte
            _isUnlocked = true;
            
            // Si configuré, retire la carte de l'inventaire après utilisation
            if (_consumeCard)
            {
                InventoryManager.Instance.RemoveCard(_requiredCard);
            }
            
            // Joue le son de déverrouillage si assigné
            if (_unlockSound != null)
            {
                AudioSource.PlayClipAtPoint(_unlockSound, transform.position, _soundVolume);
                Debug.Log("AccessDoor: Door unlocked!");
            }
            
            // Lance le processus d'ouverture de la porte
            StartCoroutine(OpenDoorWithDelay());
        }
        else
        {
            // Accès refusé : le joueur n'a pas la bonne carte
            if (_deniedSound != null)
            {
                AudioSource.PlayClipAtPoint(_deniedSound, transform.position, _soundVolume);
                Debug.Log("AccessDoor: Access denied!");
            }
            
            // Affiche quelle carte est nécessaire
            Debug.Log($"Carte d'accès requise: {_requiredCard?.cardName ?? "Non définie"}");
        }
    }
    
    // Ouvre la porte après un délai configuré
    private IEnumerator OpenDoorWithDelay()
    {
        // Indique que la porte est en train de s'ouvrir
        _isOpening = true;
        
        // Met à jour l'apparence visuelle (change la couleur en vert)
        UpdateVisual();
        
        // Attend le délai configuré
        yield return new WaitForSeconds(_openDelay);
        
        // Désactive le visuel de la porte (si assigné) ou l'objet entier
        if (_doorVisual != null)
        {
            _doorVisual.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    
    // Met à jour la couleur de la porte selon son état
    private void UpdateVisual()
    {
        if (_renderer != null)
        {
            // Couleur verte si déverrouillée, rouge sinon
            _renderer.material.color = _isUnlocked ? _unlockedColor : _lockedColor;
        }
    }
}
