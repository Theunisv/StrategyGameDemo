using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _mainCamera;
    private Transform _cameraPos;

    [SerializeField]
    private GameObject targetObject;
    private Transform _targetPos;
    private void Start()
    {
        _mainCamera = Camera.main;
        _cameraPos = _mainCamera.transform;
        _targetPos = targetObject.transform;
        _cameraPos.position = new Vector3(_targetPos.position.x, _targetPos.position.y - 0.8f, _cameraPos.position.z);
    }
}
