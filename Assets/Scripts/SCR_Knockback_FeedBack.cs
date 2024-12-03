using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SCR_Knockback_FeedBack : MonoBehaviour
{
   [SerializeField] private Rigidbody2D rb2d;
   
   [SerializeField] private float knockbackStrength = 16, delay = 0.5f;

   public UnityEvent OnBegin, OnDone;

   public void PlayFeedback(GameObject sender)
   {
      StopAllCoroutines();
      OnBegin?.Invoke();
      Vector2 direction = (transform.position - sender.transform.position).normalized;
      rb2d.AddForce(direction * knockbackStrength, ForceMode2D.Impulse);
      StartCoroutine(Reset());
   }
   
   private IEnumerator Reset()
   {
      yield return new WaitForSeconds(delay);
      rb2d.velocity = Vector3.zero;
      OnDone?.Invoke();
   }
}
