using System;
using UnityEngine;


public class FaceCamera : MonoBehaviour
{
    private Transform _mainCameraTransform;

    private void Awake()
    {
        _mainCameraTransform = Camera.main.transform;
    }
    
    private void LateUpdate()
    {
        /*Quaternion rotation = Quaternion.LookRotation(_mainCameraTransform.position - transform.position);
        transform.rotation = rotation;*/
        
        /*transform.LookAt(transform.position + _mainCameraTransform.forward,
            _mainCameraTransform.up);*/
        
        // Looking at Camera's forward and up vector
        transform.LookAt(transform.position + _mainCameraTransform.rotation * Vector3.forward,
            _mainCameraTransform.rotation * Vector3.up);
    }
}