using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private CharacterController characterController;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        
    }
}
