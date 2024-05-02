using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private PlayerInputManager _playerInputManager;
    private Transform _cameraTransform;
    
    [Header("Player Attributes")]
    [SerializeField]private float playerSpeed = 2.0f;
    [SerializeField]private float jumpHeight = 1.0f;
    [SerializeField]private float gravityValue = -9.81f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        _playerInputManager = PlayerInputManager.Instance;
        if (Camera.main != null)
        {
            _cameraTransform = Camera.main.transform;
        }
    }

    void FixedUpdate()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -0.1f;
        }

        Vector2 movement = _playerInputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        move = _cameraTransform.forward * move.z + _cameraTransform.right * move.x;
        Vector3.Normalize(move);
        controller.Move(move * Time.deltaTime * playerSpeed);

        move.y = 0;
        
            if (_playerInputManager.PlayerJumpedThisFrame() && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        
        playerVelocity.y += gravityValue * Time.deltaTime; //DONT FORGET: TURNING THIS OFF CAN MAKE YOU FLOAT
        controller.Move(playerVelocity * Time.deltaTime);

    }
}
