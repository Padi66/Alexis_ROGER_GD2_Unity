using System;
using UnityEngine;

public class TargetSoft : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
      Destroy(gameObject);
   }
}
