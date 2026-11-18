using UnityEngine;

public class OnscreenControls : MonoBehaviour
{
    public Joystick movementJoystick;
    public Joystick lookingJoystick;


    void Update()
    {
        if (CustomInput.Instance == null ||
            !CustomInput.Instance.InputEnabled ||
            !CustomInput.Instance.screenInputEnabled)
        {
            return;
        }

        CustomInput.Instance.move = new Vector2(
            movementJoystick.Horizontal, 
            movementJoystick.Vertical
        );
        CustomInput.Instance.look = new Vector2(
            lookingJoystick.Horizontal, 
            lookingJoystick.Vertical
        );
    }
    

    public void SprintTouchDown()
    {
        CustomInput.Instance.sprint = true;
    }

    public void SprintTouchUp()
    {
        CustomInput.Instance.sprint = false;
    }
}
