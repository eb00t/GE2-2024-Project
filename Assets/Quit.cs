using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
  public void StopPlaying()
   {
       Debug.Log("Quit button pressed!");
      Application.Quit();
   }
}
