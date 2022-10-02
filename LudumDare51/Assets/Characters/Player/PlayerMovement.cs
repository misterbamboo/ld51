using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool IsAccelerating => accelerating;
    [SerializeField] Rigidbody playerBody;

    private Vector3 velocity;
    private float accelVelocitySpeed = 3f;
    private float slowDownVelocitySpeed = 3f;
    private float movementSpeed = 3.5f;
    private bool accelerating;
    private Quaternion targetRotation;
    private float targetYPos;

    private void Start()
    {
        targetYPos = playerBody.position.y;
    }

    void Update()
    {
        UpdateVelocity();
        Move();
    }

    private void Move()
    {
        playerBody.velocity = Vector3.zero;
        playerBody.angularVelocity = Vector3.zero;

        var directionVector = DirectionVector();
        if (directionVector != Vector3.zero)
        {
            var targetPos = playerBody.position + directionVector;
            targetPos.y = targetYPos;
            playerBody.MovePosition(targetPos);

            if (accelerating)
            {
                var lastPos = playerBody.position;
                var newPos = lastPos + directionVector;
                var diff = (newPos - lastPos);
                targetRotation = Quaternion.LookRotation(diff);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
            }
        }
    }

    private void UpdateVelocity()
    {
        var xInput = Input.GetAxisRaw("Horizontal");
        var zInput = Input.GetAxisRaw("Vertical");
        accelerating = false;
        accelerating = xInput != 0 || zInput != 0;

        var updatedVelocity = velocity;
        updatedVelocity.x = UpdateVelocity(velocity.x, xInput);
        updatedVelocity.z = UpdateVelocity(velocity.z, zInput);
        velocity = updatedVelocity;
    }

    private Vector3 DirectionVector()
    {
        return velocity * movementSpeed * Time.deltaTime;
    }

    private float UpdateVelocity(float velocity, float input)
    {
        var velocitySpeed = accelVelocitySpeed;
        if (ShouldSlowDown(input))
        {
            input = SimulateInvertedInput(velocity);
            velocitySpeed = slowDownVelocitySpeed;
        }

        var value = velocity + input * velocitySpeed * Time.deltaTime;
        return ClampToZero(velocity, value);
    }

    private static bool ShouldSlowDown(float input) => input == 0;

    private static float SimulateInvertedInput(float velocity) => -Math.Sign(velocity);

    private static float ClampToZero(float velocity, float value)
    {
        if (velocity > 0)
        {
            value = Mathf.Clamp(value, 0, 1);
        }
        if (velocity < 0)
        {
            value = Mathf.Clamp(value, -1, 0);
        }

        return value;
    }
}
