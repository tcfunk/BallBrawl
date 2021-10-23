using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Marble : MonoBehaviour
{
    [SerializeField]private float inputForceStrength;
    
    private MarblePlayerInput inputManager;
    private MarblePlayerInput.PlayerActions inputActions;

    private Vector2 moveInputDirection;
    private Camera cam;
    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        Debug.Log(cam.name);
        Debug.Log(cam.transform.right);
        Debug.Log(cam.transform.forward);
    }

    private void Awake()
    {
        inputManager = new MarblePlayerInput();
        inputActions = inputManager.Player;

        inputActions.Move.performed += ctx => moveInputDirection = ctx.ReadValue<Vector2>();
        inputActions.Move.canceled += context => moveInputDirection = Vector2.zero;
    }

    private void OnEnable()
    {
        inputManager?.Enable();
    }

    private void OnDisable()
    {
        inputManager?.Disable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moveInputDirection != Vector2.zero)
        {
            var cameraTransform = cam.transform;
            var right = new Vector3(cameraTransform.right.x, 0f, cameraTransform.right.z).normalized;
            var forward = new Vector3(cameraTransform.forward.x, 0f, cameraTransform.forward.z).normalized;
            var inputForce = (right * moveInputDirection.x + forward * moveInputDirection.y) * inputForceStrength * Time.fixedDeltaTime;
            
            rb.AddForce(inputForce, ForceMode.Force);
        }
    }
}
