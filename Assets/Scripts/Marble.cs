using Cinemachine;
using Mirror;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class Marble : NetworkBehaviour
{
    [SerializeField]private float inputForceStrength;
    
    private MarblePlayerInput inputManager;
    private MarblePlayerInput.PlayerActions inputActions;

    private Vector2 moveInputDirection;
    private Camera cam;
    private Rigidbody rb;
    
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField]
    private float maxCameraDutch = 5f;
    [SerializeField]
    private float cameraTiltSmoothTime = 0.5f;

    private float cameraTiltVelocity = 0f;

    [SerializeField]
    private GameObject innerPrefab;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        
        var inst = Instantiate(innerPrefab, transform.position, Quaternion.identity, null);
        var inner = inst.GetComponent<MarbleInner>();
        if (inner)
        {
            inner.SetParent(transform);
            virtualCamera.Follow = inst.transform;
            virtualCamera.LookAt = inst.transform;
        }
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        virtualCamera.gameObject.SetActive(true);
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

    void Update()
    {
        if (!isLocalPlayer) return;
        
        if (moveInputDirection != Vector2.zero)
        {
            var cameraTransform = cam.transform;
            var right = new Vector3(cameraTransform.right.x, 0f, cameraTransform.right.z).normalized;
            var forward = new Vector3(cameraTransform.forward.x, 0f, cameraTransform.forward.z).normalized;
            var inputForce = (right * moveInputDirection.x + forward * moveInputDirection.y) * inputForceStrength * Time.deltaTime;
            
            rb.AddForce(inputForce, ForceMode.Force);
        }
        
        DutchCamera();
    }

    void DutchCamera()
    {
        var right = moveInputDirection.x;
        var currentDutch = virtualCamera.m_Lens.Dutch;
        virtualCamera.m_Lens.Dutch = Mathf.SmoothDamp(currentDutch, right * maxCameraDutch, ref cameraTiltVelocity, cameraTiltSmoothTime);
    }
}
