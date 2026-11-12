using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float horizontalInput = CustomInput.Instance.move.x;
        Vector3 movement = transform.right * (horizontalInput * speed * Time.deltaTime);
        controller.Move(movement);
    }
}
