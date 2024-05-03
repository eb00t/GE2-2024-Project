using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private PlayerInputManager _playerInputManager;
    private Transform _cameraTransform;
    private CapsuleCollider _capsuleCollider;
    private LayerMask _groundLayer;
    private CinemachineVirtualCamera _vcam;
    private CinemachineInputProvider _camera;
    private CinemachineBasicMultiChannelPerlin _cmPerlin;

    [Header("Player Attributes")] 
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private bool isDrone;
    [SerializeField] public bool controlsEnabled;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        _playerInputManager = PlayerInputManager.Instance;
        _groundLayer = LayerMask.NameToLayer("Ground");
        _capsuleCollider = GetComponent<CapsuleCollider>();
        if (Camera.main != null)
        {
            _cameraTransform = Camera.main.transform;
        }

        _vcam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        _camera = GameObject.Find("Virtual Camera").GetComponent<CinemachineInputProvider>();
        _cmPerlin = _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void FixedUpdate()
    {

        if (controlsEnabled)
        {
            _camera.enabled = true;
            float radius = _capsuleCollider.radius * 0.9f;

            Vector3 pos = transform.position + Vector3.up * (radius * 0.9f);

            groundedPlayer = Physics.CheckSphere(pos, radius, _groundLayer);
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = -0.1f;
            }

            Vector2 movement = _playerInputManager.GetPlayerMovement();
            Vector3 move = new Vector3(movement.x, 0f, movement.y);
            move = _cameraTransform.forward.normalized * move.z + _cameraTransform.right.normalized * move.x;
            if (!isDrone)
            {
                move.y = 0;
            }

            if (movement.x != 0 || movement.y != 0) 
            {
                _cmPerlin.m_FrequencyGain = 1; 
            }
            else
            {
                _cmPerlin.m_FrequencyGain = 0; 
            }
            
            controller.Move(move * Time.deltaTime * playerSpeed);

            if (_playerInputManager.PlayerJumpedThisFrame() && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }

            if (!isDrone)
            {
                playerVelocity.y += gravityValue * Time.deltaTime; //DONT FORGET: TURNING THIS OFF CAN MAKE YOU FLOAT
            }

            controller.Move(playerVelocity * Time.deltaTime);
        }
        else
        {
            _camera.enabled = false;
        }
    }
}
