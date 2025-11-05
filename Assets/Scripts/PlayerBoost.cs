using UnityEngine;

public class PlayerBoost : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    
    // Stocke les valeurs originales pour pouvoir les restaurer après le boost
    // Important : ces valeurs doivent être sauvegardées AVANT le premier boost
    private float _originalSpeed;
    private float _originalJumpForce;
    
    // Temps (en secondes depuis le démarrage) auquel le boost se termine
    // Utilise Time.time pour une comparaison facile dans Update()
    private float _boostEndTime;
    
    // Flag pour savoir si un boost est actuellement actif
    // Évite de sauvegarder les valeurs boostées comme valeurs originales
    private bool _isBoostActive;
    
    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }
    
    private void Update()
    {
        // Vérifie à chaque frame si le boost doit se terminer
        // Time.time retourne le temps écoulé depuis le démarrage du jeu
        if (_isBoostActive && Time.time >= _boostEndTime)
        {
            RemoveBoost();
        }
    }
    
    public void ApplyBoost(float speedMultiplier, float jumpMultiplier, float duration)
    {
        // Si c'est le premier boost, sauvegarde les valeurs d'origine
        if (!_isBoostActive)
        {
            StoreOriginalValues();
        }
        else
        {
            // Si un boost est déjà actif, le retire d'abord
            // Cela garantit qu'on repart des valeurs originales avant d'appliquer le nouveau boost
            RemoveBoost();
        }
        
        // Applique les multiplicateurs aux valeurs originales (pas aux valeurs actuelles)
        _playerMovement.SetSpeed(_originalSpeed * speedMultiplier);
        _playerMovement.SetJumpForce(_originalJumpForce * jumpMultiplier);
        
        // Calcule le moment où le boost se terminera
        // Time.time (maintenant) + duration (durée du boost)
        _boostEndTime = Time.time + duration;
        _isBoostActive = true;
    }
    
    // Sauvegarde les statistiques normales du joueur avant application du boost
    // Appelé uniquement lors du premier boost pour éviter de sauvegarder des valeurs boostées
    private void StoreOriginalValues()
    {
        _originalSpeed = _playerMovement.GetSpeed();
        _originalJumpForce = _playerMovement.GetJumpForce();
    }
    
    // Restaure les statistiques originales du joueur
    private void RemoveBoost()
    {
        _playerMovement.SetSpeed(_originalSpeed);
        _playerMovement.SetJumpForce(_originalJumpForce);
        _isBoostActive = false;
    }
}
