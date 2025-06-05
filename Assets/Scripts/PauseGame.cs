using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class PauseGame : MonoBehaviour
{
    [SerializeField]private PlayerInputManager _playerInputManager;
    public bool isStarted;
    [SerializeField] private bool _isPaused = false;
    [SerializeField] public Volume _volume;
    [SerializeField]private CameraManager _cameraManager;
    public VolumeProfile unpause, pause;
    [SerializeField]private PlayerController _playerController;
    [SerializeField]private CinemachineInputProvider _fishCamInputProvider;
    [SerializeField]private GameObject _menuCanvas, _startCanvas;
    public GamePaused paused;
    [SerializeField] public EventInstance _pauseSounds;

    
   void Start()
   {
       _isPaused = false;
       isStarted = false;
       _pauseSounds = AudioManager.Instance.CreateEventInstance(FMODEvents.Instance.Pause);
       _pauseSounds.start();
       _playerInputManager = PlayerInputManager.Instance;
       _volume = GameObject.FindWithTag("GlobalVolume").GetComponent<Volume>();
       _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
       _playerController.controlsEnabled = true;
       _startCanvas = GameObject.FindWithTag("StartCanvas");
       _menuCanvas = GameObject.FindWithTag("MenuCanvas");
       _fishCamInputProvider = GameObject.FindWithTag("FishCam").GetComponent<CinemachineInputProvider>();
       _cameraManager = GameObject.FindWithTag("CameraManager").GetComponent<CameraManager>();
   }

    private void Update()
    {
        if (_playerInputManager.Pause())
        {
            _isPaused = !_isPaused;
            switch (_isPaused)
            {
                case false:
                    paused = GamePaused.No;
                    _pauseSounds.stop(STOP_MODE.ALLOWFADEOUT);
                    break;
                default:
                    paused = GamePaused.Yes;
                    _pauseSounds.start();
                    break;
            }
           
        }

        if (_isPaused)
        {
            _playerController.controlsEnabled = false;
            _playerController.isMoving = false;
            _menuCanvas.SetActive(true);
            _volume.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            _fishCamInputProvider.enabled = false;
            _playerController._cmPerlin.m_FrequencyGain = 0;
        }
        else if (_isPaused == false) 
        {
            switch (_cameraManager.inFishCam)
            {
                case true:
                    _playerController.controlsEnabled = false;
                    break;
                default:
                    _playerController.controlsEnabled = true;
                    break;
            }
            _menuCanvas.SetActive(false);
            _volume.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            _fishCamInputProvider.enabled = true;
        }

        if (isStarted == false)
        {
            _playerController.controlsEnabled = false;
            _playerController.isMoving = false; 
            _startCanvas.SetActive(true);
            _menuCanvas.SetActive(false);
            _volume.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            _fishCamInputProvider.enabled = false;
            _playerController._cmPerlin.m_FrequencyGain = 0;
        } 
        else if (isStarted)
        {
            _startCanvas.SetActive(false);
            _fishCamInputProvider.enabled = true;
        }
        
    }
    
}
