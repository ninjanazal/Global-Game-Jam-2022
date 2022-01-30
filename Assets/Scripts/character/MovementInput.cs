using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FacingDirection
{
    X_Axis_Left_To_Right,
    X_Axis_Right_To_Left,
    I_X_Axis_Left_To_Right,
    I_X_Axis_Right_To_Left,
    Z_Axis_Left_To_Right,
    Z_Axis_Right_To_Left,
    I_Z_Axis_Left_To_Right,
    I_Z_Axis_Right_To_Left,
}

public class MovementInput : MonoBehaviour
{
    // Parameters
    [SerializeField] private float playerSpeed = 5.0f;          // Player Movement Speed Value
    [SerializeField] private float playerRotateTime = 0.25f;     // Time it takes to rotate a player

    // Cached
    private CharacterController characterController;
    private float inputHorizontal = 0f;                         // Horizontal Input
    private float inputVertical = 0f;                           // Vertical Input
    private Vector3 movementVector = new Vector3();             // Movement Vector to be applied

    // States
    public FacingDirection currentDirection;

    // Start is called before the first frame update
    void Start()
    {
        currentDirection = FacingDirection.X_Axis_Left_To_Right;
        characterController = gameObject.GetComponent<CharacterController>();    
    }

    // Update is called once per frame
    void Update()
    {
        ReadInput();
    }

    private void ReadInput()
    {
        // A D -> inputHorizontal < 0 = Left || inputHorizontal > 0 = Right
        inputHorizontal = Input.GetAxis("Horizontal");
        // W S
        inputVertical = Input.GetAxis("Vertical");

        InterpretInput();
    }

    #region Interpret Input

    /// <summary>
    /// Interpret the input of the user based on the direction the character is using
    /// </summary>
    private void InterpretInput()
    {
        switch (currentDirection)
        {
            case FacingDirection.X_Axis_Left_To_Right:
                if(inputHorizontal < 0)
                {
                    StartCoroutine(RotateCharacter(Vector3.up * 180, playerRotateTime));
                    SwitchState(FacingDirection.X_Axis_Right_To_Left);
                }
                break;

            case FacingDirection.X_Axis_Right_To_Left:
                if (inputHorizontal > 0)
                {
                    StartCoroutine(RotateCharacter(Vector3.up * -180, playerRotateTime));
                    SwitchState(FacingDirection.X_Axis_Left_To_Right);
                }
                break;


            case FacingDirection.I_X_Axis_Left_To_Right:

                break;

            case FacingDirection.I_X_Axis_Right_To_Left:
                if (inputHorizontal < 0)
                {
                    StartCoroutine(RotateCharacter(Vector3.up * 180, playerRotateTime));
                    SwitchState(FacingDirection.Z_Axis_Right_To_Left);
                }
                break;

            case FacingDirection.Z_Axis_Left_To_Right:
                if (inputHorizontal > 0)
                {
                    StartCoroutine(RotateCharacter(Vector3.up * 180, playerRotateTime));
                    SwitchState(FacingDirection.Z_Axis_Right_To_Left);
                }
                break;

            case FacingDirection.Z_Axis_Right_To_Left:

                break;

            case FacingDirection.I_Z_Axis_Left_To_Right:

                break;

            case FacingDirection.I_Z_Axis_Right_To_Left:

                break;
        }

        ApplyMovement();
    }

    #endregion

    #region Apply Movement

    /// <summary>
    /// Apply Movement Based on direction that the character is facing
    /// </summary>
    private void ApplyMovement()
    {
        switch (currentDirection)
        {
            #region X Axis Left to Right
            case FacingDirection.X_Axis_Left_To_Right:
                movementVector = characterController.transform.right * inputHorizontal;
                break;
            #endregion

            #region X Axis Right to Left
            case FacingDirection.X_Axis_Right_To_Left:
                movementVector = characterController.transform.right * inputHorizontal * -1;
                break;
            #endregion

            #region Z Axis Left To Right
            case FacingDirection.Z_Axis_Left_To_Right:
                movementVector = characterController.transform.forward * inputHorizontal;
                break;
            #endregion

            #region Z Axis Right To Left
            case FacingDirection.Z_Axis_Right_To_Left:
                movementVector = characterController.transform.forward * inputHorizontal * -1;
                break;
            #endregion
        }
        characterController.Move(movementVector * playerSpeed * Time.deltaTime);
    }

    #endregion

    #region Rotate Character

    /// <summary>
    /// Rotates a Character in an animated form in a specific interval
    /// </summary>
    /// <param name="byAngles"></param>
    /// <param name="inTime"></param>
    /// <returns></returns>
    IEnumerator RotateCharacter(Vector3 byAngles, float inTime)
    {
        var fromAngle = transform.rotation;
        var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return inTime;
        }
        
    }

    #endregion

    public void HandleCollision()
    {
        // StartCoroutine(RotateCharacter(Vector3.up * 180, playerRotateTime));

        switch (currentDirection)
        {
            case FacingDirection.X_Axis_Left_To_Right:
                SwitchState(FacingDirection.Z_Axis_Left_To_Right);
                StartCoroutine(RotateCharacter(Vector3.up * 90, playerRotateTime));
                break;

            case FacingDirection.X_Axis_Right_To_Left:
                SwitchState(FacingDirection.Z_Axis_Right_To_Left);
                StartCoroutine(RotateCharacter(Vector3.up * -90, playerRotateTime));
                break;

            case FacingDirection.Z_Axis_Left_To_Right:
                SwitchState(FacingDirection.X_Axis_Left_To_Right);
                StartCoroutine(RotateCharacter(Vector3.up * 90, playerRotateTime));
                break;

            case FacingDirection.Z_Axis_Right_To_Left:
                SwitchState(FacingDirection.X_Axis_Right_To_Left);
                StartCoroutine(RotateCharacter(Vector3.up * -90, playerRotateTime));
                break;
        }
    }

    private void FlipCharacter()
    {
        // Up to Down -> Rotate X from 0 to 180
        // Down to Up -> Rotate Y from 180 to 0
    }

    #region Switch State

    /// <summary>
    /// Switches the current state and current interpretation
    /// </summary>
    /// <param name="newState"></param>
    private void SwitchState(FacingDirection newState)
    {
        currentDirection = newState;
    }

    #endregion
}
