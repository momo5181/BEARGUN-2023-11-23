using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
 private void OnCollisionEnter(Collision collision)
 {
 
   Transform hitTransform=collision.transform;
   
   if(hitTransform.CompareTag("Player"))
   {
    Debug.Log("hit player");
    hitTransform.GetComponent<PlayerHealth>().TakeDamage(5); //結合血條，當子彈擊中玩家扣血
   }
   Destroy(gameObject,2f);

 }
}
