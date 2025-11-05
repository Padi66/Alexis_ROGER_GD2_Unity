using UnityEngine;

// Gère le suivi fluide du joueur par la caméra avec possibilité de zoom
// La caméra suit le joueur avec un décalage configurable et peut zoomer/dézoomer avec la molette
public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    // Référence au GameObject du joueur que la caméra doit suivre
    [SerializeField] private GameObject _player;
    
    [Header("Follow Settings")]
    // Décalage de position entre la caméra et le joueur (ex: (0, 5, -10) = 5 unités au-dessus, 10 unités derrière)
    [SerializeField] private Vector3 _offset = new Vector3(0f, 5f, -10f);
    
    // Vitesse de lissage du mouvement (0 = instantané, 1 = très lent)
    [SerializeField] private float _smoothSpeed = 0.125f;
    
    // Si vrai, la caméra regardera toujours vers le joueur
    [SerializeField] private bool _lookAtPlayer = true;
    
    [Header("Zoom Settings")]
    // Vitesse à laquelle la caméra zoom/dézoom avec la molette
    [SerializeField] private float _zoomSpeed = 2f;
    
    // Distance minimale entre la caméra et le joueur (zoom maximum)
    [SerializeField] private float _minZoom = 3f;
    
    // Distance maximale entre la caméra et le joueur (dézoom maximum)
    [SerializeField] private float _maxZoom = 15f;
    
    // Décalage actuel qui varie avec le zoom
    private Vector3 _currentOffset;
    
    // Initialisation au démarrage
    private void Start()
    {
        // Initialise le décalage actuel avec la distance minimale de zoom
        _currentOffset = _offset.normalized * _minZoom;
    }
    
    // Appelé après tous les Update() pour éviter les saccades de caméra
    private void LateUpdate()
    {
        // Vérifie que la référence au joueur existe
        if (_player == null)
        {
            Debug.LogError("Player reference is missing in CameraFollow!");
            return;
        }
        
        // Gère le zoom avec la molette de la souris
        HandleZoom();
        
        // Calcule la position souhaitée de la caméra (position du joueur + décalage)
        Vector3 desiredPosition = _player.transform.position + _currentOffset;
        
        // Lisse le mouvement de la caméra pour un déplacement fluide
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
        transform.position = smoothedPosition;
        
        // Oriente la caméra vers le joueur si activé
        if (_lookAtPlayer)
        {
            transform.LookAt(_player.transform);
        }
    }
    
    // Gère le zoom/dézoom avec la molette de la souris
    private void HandleZoom()
    {
        // Récupère l'entrée de la molette (positif = zoom avant, négatif = zoom arrière)
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        
        // Si la molette a bougé
        if (scrollInput != 0f)
        {
            // Calcule la quantité de zoom à appliquer
            float zoomAmount = scrollInput * _zoomSpeed;
            
            // Direction du zoom (vers ou depuis le joueur)
            Vector3 zoomDirection = _currentOffset.normalized;
            
            // Calcule le nouveau décalage après zoom
            Vector3 newOffset = _currentOffset + zoomDirection * zoomAmount;
            
            // Calcule la nouvelle distance entre la caméra et le joueur
            float newDistance = newOffset.magnitude;
            
            // Applique le zoom seulement si la nouvelle distance est dans les limites
            if (newDistance >= _minZoom && newDistance <= _maxZoom)
            {
                _currentOffset = newOffset;
            }
            else
            {
                // Limite le zoom aux valeurs min/max configurées
                _currentOffset = zoomDirection * Mathf.Clamp(newDistance, _minZoom, _maxZoom);
            }
        }
    }
}
