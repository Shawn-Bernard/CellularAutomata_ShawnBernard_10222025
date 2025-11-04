using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public InputAction moveAction;
    private Controller controller => GetComponent<Controller>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();

        //controller.Move(new Vector3(move.x, 0f, move.y));
    }

    void Move(InputAction.CallbackContext context)
    {
    }
}
