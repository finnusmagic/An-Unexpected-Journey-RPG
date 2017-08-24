using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

public class ThirdPersonCamera : MonoBehaviour
{

    public float mouseSensitivity = 5;
    public Transform player;
    public float distanceFromTarget = 10f;
    public float rotationToPlayer = 40f;

    public Vector2 pitchMinMax = new Vector2(-40, 85);

    public float rotationSmoothTime = 0.12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    float yaw;
    float pitch;

    private void Start()
    {
        pitch = Mathf.Clamp(pitch, rotationToPlayer, rotationToPlayer);
    }

    void LateUpdate()
    {
        if (FindObjectOfType<PlayerMovement>().canMove)
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
                pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
                pitch = Mathf.Clamp(pitch, rotationToPlayer, rotationToPlayer);
            }

            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
            transform.eulerAngles = currentRotation;

            Vector3 targetRotation = new Vector3(pitch, yaw);
            transform.eulerAngles = targetRotation;

            transform.position = player.position - transform.forward * distanceFromTarget;
        }
    }
}