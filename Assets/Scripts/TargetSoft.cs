using System;
using System.Collections;
using UnityEngine;

public class TargetSoft : MonoBehaviour
{
   [SerializeField] private int _targetValue = 1;
   [SerializeField] private float _shadowDuration = 3f;
   [SerializeField] private GameObject _particleEffect;
   private float _shadowTimer = 0f;
   private bool _isInShadows = false;
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.GetComponent<Player_Collect>() != null)
      {
         other.gameObject.GetComponent<Player_Collect>().UpdateScore(_targetValue);
         //Destroy(gameObject);
         //TODO : hide Target
         ToggleVisibility(false);
         //TODO : Start Timer
         //_isInShadows = true;
         //Instantiate(_particleEffect.transform.position Quaternion.identity);
         StartCoroutine(routine: ShadowTimerControl());
      }
   }

   private void ToggleVisibility(bool newVisibility)
   {
      GetComponent<MeshRenderer>().enabled = newVisibility;
      GetComponent<Collider>().enabled = newVisibility;
   }
   //TODO: Timer by deltatime
   /*private void Update()
   {
      if (_isInShadows)
      {
         _shadowTimer += Time.deltaTime;
         if (_shadowTimer >= _shadowDuration)
         {
            //TODO: Show Target
            ToggleVisibility(true);
            //TODO: Stop Timer
            _shadowTimer = 0f;
            _isInShadows = false;
        }
      
      }
            
    }*/
   
   //TODO : Timer by coroutine
   private IEnumerator ShadowTimerControl()
   {
      //yield return new WaitForEndOfFrame()
      yield return new WaitForSeconds(_shadowDuration);
      ToggleVisibility(true);
   }
       
}
