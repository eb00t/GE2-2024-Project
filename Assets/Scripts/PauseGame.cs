using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class PauseGame : MonoBehaviour
{
    private PlayerInputManager _playerInputManager;
    private bool _isPaused = false;
    private Volume _volume;
    public VolumeProfile unpause, pause;
    private PlayerController _playerController;
    private CinemachineInputProvider _fishCamInputProvider;
    private GameObject _menuCanvas;

    
   void Start()
   {
       _isPaused = false;
       _playerInputManager = PlayerInputManager.Instance;
       _volume = GameObject.FindWithTag("GlobalVolume").GetComponent<Volume>();
       _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
       _playerController.controlsEnabled = true;
       _menuCanvas = GameObject.FindWithTag("MenuCanvas");
       _fishCamInputProvider = GameObject.FindWithTag("FishCam").GetComponent<CinemachineInputProvider>();
   }

    private void Update()
    {
        if (_playerInputManager.Pause())
        {
            _isPaused = !_isPaused;
        }

        if (_isPaused)
        {
            _playerController.controlsEnabled = false;
            _menuCanvas.SetActive(true);
            _volume.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            _fishCamInputProvider.enabled = false;
            _playerController._cmPerlin.m_FrequencyGain = 0;
        }
        else if (_isPaused == false) 
        {
            _menuCanvas.SetActive(false);
            _playerController.controlsEnabled = true;
            _volume.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            _fishCamInputProvider.enabled = true;
        }
    }
}
