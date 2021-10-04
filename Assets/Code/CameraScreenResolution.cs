using System;
using UnityEngine;

namespace ManStretchArm.Code
{
    public class CameraScreenResolution : MonoBehaviour
    {
        private Camera _camera;
        private float _initialSize;
        private float _targetAspect;
        private Vector3 _cameraPosition;
        
#if UNITY_EDITOR
        private Vector2 _resolution;
#endif
        
        private void Awake()
        {
            _camera = Camera.main;
            _cameraPosition = _camera.transform.position;
            _initialSize = _camera.orthographicSize;
            _targetAspect = (float) 1080 / 1920;

            CalculateCamera();
#if UNITY_EDITOR
            _resolution = new Vector2(Screen.width, Screen.height);
#endif
        }
        
#if UNITY_EDITOR
        private void Update()
        {
            if (_resolution.x != Screen.width || _resolution.y != Screen.height)
            {
                CalculateCamera();
     
                _resolution.x = Screen.width;
                _resolution.y = Screen.height;
            }
        }
#endif
        
        private void CalculateCamera()
        {
            _camera.orthographicSize = _initialSize * (_targetAspect / _camera.aspect);
            _camera.transform.position = new Vector3(_cameraPosition.x,
                _cameraPosition.y - 1 * (_initialSize - _camera.orthographicSize), _cameraPosition.z);
        }
    }
}