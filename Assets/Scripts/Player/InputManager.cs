using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction moveInput;
    private InputAction useInput;
    private InputAction scrollInput;

    public static Vector2 Movement;
    public static float MouseScrollInput;

    public static bool useWasPressed;
    public static bool useWasHeld;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveInput = playerInput.actions["Move"];
        useInput = playerInput.actions["Attack"];
        scrollInput = playerInput.actions["Scroll"];
    }

    // Update is called once per frame
    void Update()
    {
        Movement = moveInput.ReadValue<Vector2>();
        // Debug.Log($"[InputManager.Update] mouse scroll input: {MouseScrollInput}");
        MouseScrollInput = scrollInput.ReadValue<float>();
        useWasPressed = useInput.WasPressedThisFrame();
        useWasHeld = useInput.IsPressed();
    }
}
