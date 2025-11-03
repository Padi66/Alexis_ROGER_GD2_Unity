using UnityEngine;

public class DeathBox : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            RespawnPlayer(other.gameObject);
        }
    }
    
    private void RespawnPlayer(GameObject player)
    {
        RespawnBox respawnBox = FindFirstObjectByType<RespawnBox>();
        
        if (respawnBox != null)
        {
            player.transform.position = respawnBox.transform.position;
            
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
        else
        {
            Debug.LogWarning("Aucune RespawnBox trouvée dans la scène!");
        }
    }
}

