// Source: https://sharpcoderblog.com/blog/unity-3d-fps-controller

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


[RequireComponent(typeof(CharacterController))]

public class FPSController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public Transform playerCamera;
    public Transform player;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    private CharacterController Cc;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    float rotationY = 0;

     void Awake() 
     {
        Cc = player.GetComponent<CharacterController>();
     }

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Player and Camera rotation
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        rotationY = Input.GetAxis("Mouse X") * lookSpeed;
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);      
        transform.rotation *= Quaternion.Euler(0, rotationY, 0);

        // Recalculate move direction based on axes 
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        float curSpeedX = walkingSpeed * Input.GetAxis("Vertical");
        float curSpeedY = walkingSpeed * Input.GetAxis("Horizontal"); 

        // Move the controller  
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        Cc.Move(moveDirection * Time.deltaTime);    

    }

}