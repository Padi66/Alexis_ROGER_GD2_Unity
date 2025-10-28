using UnityEngine;

public class PlayerBoost : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private float _originalSpeed;
    private float _originalJumpForce;
    private float _boostEndTime;
    private bool _isBoostActive;
    
    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }
    
    private void Update()
    {
        if (_isBoostActive && Time.time >= _boostEndTime)
        {
            RemoveBoost();
        }
    }
    
    public void ApplyBoost(float speedMultiplier, float jumpMultiplier, float duration)
    {
        if (!_isBoostActive)
        {
            StoreOriginalValues();
        }
        else
        {
            RemoveBoost();
        }
        
        _playerMovement.SetSpeed(_originalSpeed * speedMultiplier);
        _playerMovement.SetJumpForce(_originalJumpForce * jumpMultiplier);
        
        _boostEndTime = Time.time + duration;
        _isBoostActive = true;
    }
    
    private void StoreOriginalValues()
    {
        _originalSpeed = _playerMovement.GetSpeed();
        _originalJumpForce = _playerMovement.GetJumpForce();
    }
    
    private void RemoveBoost()
    {
        _playerMovement.SetSpeed(_originalSpeed);
        _playerMovement.SetJumpForce(_originalJumpForce);
        _isBoostActive = false;
    }
}
