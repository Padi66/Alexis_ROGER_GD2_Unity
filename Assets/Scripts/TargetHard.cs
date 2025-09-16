using System;
using UnityEngine;

public class TargetHard : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Player_Collect>() != null)
        {
            Destroy(gameObject);
            
        }
    }
}
