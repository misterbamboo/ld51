using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 velocity;
    private float accelVelocitySpeed = 2f;
    private float slowDownVelocitySpeed = 1.5f;
    private float movementSpeed = 4f;
    private bool accelerating;

    void Update()
    {
        UpdateVelocity();
        Move();
    }

    private void Move()
    {
        var directionVector = DirectionVector();
        if (directionVector != Vector3.zero)
        {
            var lastPos = transform.position;
            var newPos = lastPos + directionVector;
            transform.position = newPos;
            var diff = (newPos - lastPos);

            if (accelerating)
            {
                transform.rotation = Quaternion.LookRotation(diff);
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
