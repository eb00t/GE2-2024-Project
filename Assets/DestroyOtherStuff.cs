using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOtherStuff : MonoBehaviour
{
  private void OnCollisionEnter(Collision other)
  {
    if (other.gameObject.CompareTag("Pickup"))
    {
      other.transform.position = other.gameObject.GetComponent<PickUp>().spawnPos;
      other.transform.rotation = other.gameObject.GetComponent<PickUp>().spawnRot;
      other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
  }
}
