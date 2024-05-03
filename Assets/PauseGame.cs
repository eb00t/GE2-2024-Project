using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PauseGame : MonoBehaviour
{
    private PlayerInputManager _playerInputManager;
    private bool _isPaused = false;
    private Volume _volume;
    public VolumeProfile unpause, pause;
    private PlayerController _playerController;
    private GameObject _menuCanvas;

    
   void Start()
   {
       _isPaused = false;
        _playerInputManager = PlayerInputManager.Instance;
        _volume = GameObject.FindWithTag("GlobalVolume").GetComponent<Volume>();
        _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        _playerController.controlsEnabled = true;
        _menuCanvas = GameObject.FindWithTag("MenuCanvas");
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
        }
        else if (_isPaused == false) 
        {
            _menuCanvas.SetActive(false);
            _playerController.controlsEnabled = true;
            _volume.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
