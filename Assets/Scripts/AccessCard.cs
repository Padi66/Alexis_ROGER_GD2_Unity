using System.Collections;
using UnityEngine;

// Gère le comportement d'une carte d'accès collectible dans le jeu
// La carte tourne sur elle-même et peut être ramassée par le joueur
public class AccessCard : MonoBehaviour
{
    // Référence aux données de la carte (type, couleur, etc.)
    [SerializeField] private AccessCardData _cardData;
    
    // Vitesse de rotation de la carte en degrés par seconde
    [SerializeField] private float _rotationSpeed = 50f;
    
    // Si vrai, la carte sera détruite après collecte, sinon elle sera simplement désactivée
    [SerializeField] private bool _destroyOnCollect = true;
    
    [Header("Audio")]
    // Son joué lorsque la carte est collectée
    [SerializeField] private AudioClip _collectSound;
    
    // Volume du son de collecte (0 à 3)
    [SerializeField] [Range(0f, 3f)] private float _soundVolume = 1.5f;
    
    // Initialisation au démarrage
    private void Start()
    {
        // Applique la couleur de la carte aux matériaux du renderer si les données existent
        if (_cardData != null)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = _cardData.cardColor;
            }
        }
        
        // Lance la coroutine pour ignorer les collisions avec les cibles
        StartCoroutine(IgnoreTargetsDelayed());
    }
    
    // Désactive les collisions entre la carte et toutes les cibles du jeu
    // Le délai permet de s'assurer que tous les objets sont bien initialisés
    private IEnumerator IgnoreTargetsDelayed()
    {
        // Attend 0.1 seconde pour laisser le temps aux objets de s'initialiser
        yield return new WaitForSeconds(0.1f);
        
        // Récupère le collider de la carte
        Collider cardCollider = GetComponent<Collider>();
        if (cardCollider == null) yield break;
        
        // Ignore les collisions avec toutes les cibles dures (TargetHard)
        TargetHard[] allTargetHards = FindObjectsByType<TargetHard>(FindObjectsSortMode.None);
        foreach (TargetHard target in allTargetHards)
        {
            if (target != null)
            {
                Collider targetCollider = target.GetComponent<Collider>();
                if (targetCollider != null)
                {
                    Physics.IgnoreCollision(cardCollider, targetCollider);
                }
            }
        }
        
        // Ignore les collisions avec toutes les cibles molles (TargetSoft)
        TargetSoft[] allTargetSofts = FindObjectsByType<TargetSoft>(FindObjectsSortMode.None);
        foreach (TargetSoft target in allTargetSofts)
        {
            if (target != null)
            {
                Collider targetCollider = target.GetComponent<Collider>();
                if (targetCollider != null)
                {
                    Physics.IgnoreCollision(cardCollider, targetCollider);
                }
            }
        }
    }
    
    // Fait tourner la carte continuellement autour de l'axe vertical (Y)
    private void Update()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }
    
    // Détecte quand un objet entre en collision avec le trigger de la carte
    private void OnTriggerEnter(Collider other)
    {
        // Si c'est le joueur qui touche la carte, on la collecte
        if (other.CompareTag("Player"))
        {
            CollectCard(other.gameObject);
        }
    }
    
    // Gère la collecte de la carte par le joueur
    private void CollectCard(GameObject player)
    {
        // Vérifie que l'InventoryManager existe et que les données de la carte sont valides
        if (InventoryManager.Instance != null && _cardData != null)
        {
            // Ajoute la carte à l'inventaire du joueur
            InventoryManager.Instance.AddCard(_cardData);
            
            // Joue le son de collecte si assigné
            if (_collectSound != null)
            {
                AudioSource.PlayClipAtPoint(_collectSound, transform.position, _soundVolume);
                Debug.Log($"AccessCard: Playing sound at volume {_soundVolume}");
            }
            else
            {
                Debug.LogWarning("AccessCard: Collect Sound not assigned!");
            }
            
            // Détruit ou désactive la carte selon le paramètre _destroyOnCollect
            if (_destroyOnCollect)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
