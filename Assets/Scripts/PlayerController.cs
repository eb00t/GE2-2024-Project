using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private GameObject _playerModel;
   
    private CinemachineVirtualCamera _vcam;
    private CinemachineInputProvider _cameraInputProvider;
    public CinemachineBasicMultiChannelPerlin _cmPerlin;
    private LayerMask _playerMask;
    private GameObject _hand;
    private bool _holdingSomething = false;

    [Header("Player Attributes")] 
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private bool isDrone;
    [SerializeField] public bool controlsEnabled;

    private void Start()
    {
        _playerModel = GameObject.Find("LittleBot");
        controller = GetComponent<CharacterController>();
        _playerInputManager = PlayerInputManager.Instance;
        _groundLayer = LayerMask.NameToLayer("Ground");
        _capsuleCollider = GetComponent<CapsuleCollider>();
        if (Camera.main != null)
        {
            _cameraTransform = Camera.main.transform;
        }

        _hand = GameObject.Find("Hold");
        _playerMask = LayerMask.GetMask("IgnoreByPlayerCam");
        _vcam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        _cameraInputProvider = GameObject.Find("Virtual Camera").GetComponent<CinemachineInputProvider>();
        _cmPerlin = _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _holdingSomething = false;
    }

    void FixedUpdate()
    {

        if (controlsEnabled)
        {
            _cameraInputProvider.enabled = true;
            Vector3 playerModelRotation = new Vector3(0, 0, _cameraTransform.rotation.y);
            
            //Rotate the player model
            _playerModel.transform.rotation = Quaternion.LookRotation(playerModelRotation);
            
            float radius = _capsuleCollider.radius * 0.9f;

            Vector3 pos = transform.position + Vector3.up * (radius * 0.9f);

            groundedPlayer = Physics.CheckSphere(pos, radius, _groundLayer);
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = -0.1f;
            }

            Vector2 movement = _playerInputManager.GetPlayerMovement();
            Vector3 move = new Vector3(movement.x, 0f, movement.y);
            move = Vector3.Normalize(_cameraTransform.forward.normalized * move.z + _cameraTransform.right.normalized * move.x);
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
            _cameraInputProvider.enabled = false;
        }

        /* if (_playerInputManager.PickUp())
         {
             Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
             RaycastHit hit;
             GameObject heldObj = null;
             if (Physics.Raycast(ray, out hit, 5f, _playerMask))
             {
                 Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow);
                 if (!_holdingSomething && hit.transform.CompareTag("Pickup"))
                 {

                     Debug.Log("RayCast Attempted!");
                     var rayRb = hit.rigidbody;
                     rayRb.isKinematic = true;
                     hit.transform.SetParent(_hand.transform);
                     hit.transform.position = _hand.transform.position;
                     _holdingSomething = true;
                 }
                 else
                 {
                     heldObj.transform.SetParent(null);
                     heldObj.GetComponent<Rigidbody>().AddForce(new Vector3(10f, 0f, 0f), ForceMode.Impulse);
                     _holdingSomething = false;
                 }
             }
             else
             {
                 Debug.Log("Nothing.");
             } THIS WOULD NOT WORK AT ALL */ 
        }
    }
