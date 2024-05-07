using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using FMOD.Studio;
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
    private GameObject _playerModelBody;
   
    private CinemachineVirtualCamera _vcam;
    private CinemachineInputProvider _cameraInputProvider;
    public CinemachineBasicMultiChannelPerlin _cmPerlin;
    private LayerMask _playerMask;
    private GameObject _hand;
    private bool _holdingSomething = false;
    private bool _canThrow;
    public GameObject objToPickUp;
    private EventInstance _footsteps;
    private Vector3 _oldPos, _newPos;

    [Header("Player Attributes")] 
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private bool isDrone;
    [SerializeField] public bool controlsEnabled;
    [SerializeField] public bool isMoving;
    

    private void Start()
    {
        _playerModel = GameObject.Find("LittleBot");
        _playerModelBody = _playerModel.transform.Find("Body").gameObject;
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
        _vcam = GameObject.FindWithTag("PlayerCam").GetComponent<CinemachineVirtualCamera>();
        _cameraInputProvider = _vcam.GetComponent<CinemachineInputProvider>();
        _cmPerlin = _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _holdingSomething = false;
        _footsteps = AudioManager.Instance.CreateEventInstance(FMODEvents.Instance.Footsteps);
        _oldPos = transform.position;
        _newPos = transform.position;
    }

    void FixedUpdate()
    {

        if (controlsEnabled)
        {
            _newPos = transform.position;
            _cameraInputProvider.enabled = true;
            
            //Rotate the player model
            _playerModel.transform.rotation = Quaternion.Euler( 0, _cameraTransform.eulerAngles.y, 0);
            _playerModelBody.transform.rotation = Quaternion.Euler( _cameraTransform.eulerAngles.x, _cameraTransform.eulerAngles.y, 0);
            
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            Vector2 movement = _playerInputManager.GetPlayerMovement();
            Vector3 move = new Vector3(movement.x, 0f, movement.y);
            move = Vector3.Normalize(_cameraTransform.forward.normalized * move.z + _cameraTransform.right.normalized * move.x);
            if (Math.Abs(_oldPos.x - _newPos.x) > .01 || Math.Abs(_oldPos.z - _newPos.z) > .01)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }
            
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
            
            move.y = playerVelocity.y;


            controller.Move(playerVelocity * Time.deltaTime);
        }
        else
        {
            _cameraInputProvider.enabled = false;
        }

        if (_playerInputManager.PickUp())
        {
            Rigidbody objRb = objToPickUp.GetComponent<Rigidbody>();
            if (!_holdingSomething)
            {
                objToPickUp.tag = "InHand";
                foreach (Transform child in objToPickUp.transform)
                {
                    child.tag = "InHand";
                }
                objRb.isKinematic = true;
                objToPickUp.transform.SetParent(_hand.transform);
                objToPickUp.transform.position = _hand.transform.position;
                objToPickUp.transform.rotation = _hand.transform.rotation;
                _holdingSomething = true;
                StartCoroutine(PickUpCd());
            }
        }

        if (_playerInputManager.Throw())
        {
            Rigidbody objRb = objToPickUp.GetComponent<Rigidbody>();
            if (_holdingSomething && _canThrow)
            {
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Throw, _hand.transform.position);
                objRb.isKinematic = false;
                objToPickUp.transform.SetParent(null);
                objRb.AddForce(_hand.transform.forward * 20, ForceMode.Impulse);
                objToPickUp.tag = "Pickup";
                foreach (Transform child in objToPickUp.transform)
                {
                    child.tag = "Untagged";
                }
                objToPickUp = null;
                _holdingSomething = false;
                _canThrow = false;
            }
        }
        
        if (_playerInputManager.Drop())
        {
            Rigidbody objRb = objToPickUp.GetComponent<Rigidbody>();
            if (_holdingSomething && _canThrow)
            {
                objRb.isKinematic = false;
                objToPickUp.transform.SetParent(null);
                objRb.AddForce(_hand.transform.forward, ForceMode.Impulse);
                objToPickUp.tag = "Pickup";
                foreach (Transform child in objToPickUp.transform)
                {
                    child.tag = "Untagged";
                }
                objToPickUp = null;
                _holdingSomething = false;
                _canThrow = false;
            }
        }

        if (_holdingSomething)
        {
            if (objToPickUp != null) objToPickUp.transform.position = _hand.transform.position;
        }
        UpdateSound();
        _oldPos = _newPos;
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

    IEnumerator PickUpCd()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        _canThrow = true;
    }

    private void UpdateSound()
    {
        if (isMoving && groundedPlayer)
        {
            PLAYBACK_STATE playbackState;
            _footsteps.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                _footsteps.start();
            }
        }
        else
        {
            _footsteps.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
    
    }
