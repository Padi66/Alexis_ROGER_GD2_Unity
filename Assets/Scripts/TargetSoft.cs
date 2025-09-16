using System;
using UnityEngine;

public class TargetSoft : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.GetComponent<Player_Collect>() != null)
      {
         Destroy(gameObject);
      }
   }
}
