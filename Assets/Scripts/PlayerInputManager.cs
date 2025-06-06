using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    
    private static PlayerInputManager _instance;

    public static PlayerInputManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        _playerMovement = new PlayerMovement();
    }

    private void OnEnable()
    {
        _playerMovement.Enable();
    }

    private void OnDisable()
    {
        _playerMovement.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return _playerMovement.Player.Move.ReadValue<Vector2>();
    }
    
    public Vector2 GetMouseDelta()
    {
        return _playerMovement.Player.Look.ReadValue<Vector2>() * 1.5f;
    }
    
    public bool PlayerJumpedThisFrame()
    {
        return _playerMovement.Player.Jump.triggered;
    }

    public bool Pause()
    {
        return _playerMovement.Player.Pause.triggered;
    }
    
     public bool PickUp()
     {
         return _playerMovement.Player.PickUp.triggered;
     }
     
     public bool Drop()
     {
         return _playerMovement.Player.Drop.triggered;
     }
     
     public bool Throw()
     {
         return _playerMovement.Player.Throw.triggered;
     }
}
