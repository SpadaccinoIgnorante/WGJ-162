using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CMF;

public class MovementInput : CharacterKeyboardInput
{
    [Tooltip("Shift button name on unity input system")]
    public string RunButton;

    public string DashButton;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public virtual bool IsRunButtonPressed()
    {
        if (string.IsNullOrEmpty(RunButton)) return false;

        return Input.GetKey(KeyCode.LeftShift);
    }

    public virtual bool IsDashButtonPressed()
    {
        if (string.IsNullOrEmpty(DashButton)) return false;

        return Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown(DashButton);
    }
}
