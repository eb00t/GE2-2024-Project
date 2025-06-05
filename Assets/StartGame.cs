using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class StartGame : MonoBehaviour
{
   public PauseGame _pauseGame;

   void Start()
   {
      _pauseGame = GameObject.Find("GameManager").GetComponent<PauseGame>();
   }

   public void StartOff()
   {
      _pauseGame._volume.gameObject.SetActive(false);
      Cursor.lockState = CursorLockMode.Locked;
      _pauseGame.isStarted = true;
      _pauseGame._pauseSounds.stop(STOP_MODE.ALLOWFADEOUT);
   }
}
