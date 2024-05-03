using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    private Animator _animator;
    private static readonly int TurnOff = Animator.StringToHash("TurnOff");

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayOffAnim()
    {
        _animator.SetBool(TurnOff, true);
    }
  public void StopPlaying()
   {
       Debug.Log("Quit button pressed!");
      Application.Quit();
   }
}
