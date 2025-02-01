using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    [SerializeField] GameObject cameraObj;
    [SerializeField] float mouseSensitivity = 100f;
    
    [Header("Smoothing Parameters")]
    [SerializeField] float smoothTime = 0.1f;
    
    private float xRotation = 0f;
    private float yRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        /*if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }*/
    }

    // Update is called once per frame
    private void Update()
    {
        CameraLook();
    }
    void CameraLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -70f, 70f);
        float targetXRotation = xRotation;
        cameraObj.transform.localRotation = Quaternion.Lerp(cameraObj.transform.localRotation, Quaternion.Euler(targetXRotation, 0f, 0f), smoothTime);

        yRotation += mouseX;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0f, yRotation, 0f), smoothTime);
    }
}
