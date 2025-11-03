using UnityEngine;

public class RespawnBox : MonoBehaviour
{
    [Header("Visualisation")]
    public Color gizmoColor = Color.green;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 2f);
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}