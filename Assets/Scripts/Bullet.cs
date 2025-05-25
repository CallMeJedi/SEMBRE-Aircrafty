using System;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.CompareTag("Enemy"))
      {
         Debug.Log("Bullet hit enemy");
         Destroy(other.gameObject);
         Destroy(gameObject);
      }
      if (other.gameObject.CompareTag("InvisWall"))
      {
         Debug.Log("Bullet Hit wall");
         Destroy(gameObject);
      }
   }
}
