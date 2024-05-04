using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    private Animator _animator;
    private static readonly int TurnOff = Animator.StringToHash("TurnOff");
    private static readonly int SwitchCam = Animator.StringToHash("SwitchCam");

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlaySwitchAnim()
    {
        _animator.SetBool(SwitchCam, true);
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

   public void ResetBools()
   {
       _animator.SetBool(TurnOff,false);
       _animator.SetBool(SwitchCam,false);
   }
}
