using System;
using ManStretchArm.Code.Interfaces;
using ManStretchArm.Code.Views;
using UnityEngine;

namespace ManStretchArm.Code.Controllers
{
    public class CameraController : IUpdate, ICleanup
    {
        private Player _player;
        private Camera _camera;
        private Transform _mainCamera;
        
        private Vector3 _offset;

        private Vector2 _minMaxX = Vector2.zero; // The minimum and maximum x coordinates the camera can have.
        private Vector2 _minMaxY = new Vector2(0, Mathf.Infinity); // The minimum and maximum y coordinates the camera can have.

        private float smoothTime = 0.3f;
        private Vector3 velocity = Vector3.zero;
        private GroundView _ground;

        public CameraController(Player player, Camera mainCamera, GroundView ground)
        {
            _player = player;
            _camera = mainCamera;
            _mainCamera = mainCamera.transform;
            _ground = ground;
            //_offset = _mainCamera.position - _player.Transform.position;
            _offset = _mainCamera.position - _player.PickedPoint.position;
            _offset.z = _mainCamera.position.z;
            
            //_offset.y = (_mainCamera.position - _player.PickedPoint.position).y;
            
            _minMaxY.x = _player.PickedPoint.position.y + _offset.y;
            
            _player.Picked += OnPlayerPicked;
            _ground.LevelObjectContactEvent += OnPlayerDead;
        }

        public void Update(float deltaTime)
        {
            var playerPosition = _player.Transform.position;
            var targetX = playerPosition.x;
            var targetY = playerPosition.y;

            targetY = (playerPosition + _offset).y;

            targetX = Mathf.Clamp(targetX, _minMaxX.x, _minMaxX.y); 
            targetY = Mathf.Clamp(targetY, _minMaxY.x, _minMaxY.y);

            var targetPosition = new Vector3(targetX, targetY, _offset.z);
            
            _mainCamera.position = Vector3.SmoothDamp(_mainCamera.position, targetPosition, ref velocity, smoothTime);
            
            //_mainCamera.position = new Vector3(targetX, targetY, _offset.z);
        }

        public void Cleanup()
        {
            _player.Picked -= OnPlayerPicked;
            _ground.LevelObjectContactEvent -= OnPlayerDead;
        }

        private void OnPlayerPicked(bool isPicked, Point point)
        {
            if (isPicked)
            {
                _minMaxY.x = point.transform.position.y + _offset.y;
            }
            else
            {
                _minMaxY.x = 0;
            }
        }
        
        private void OnPlayerDead(GroundView ground)
        {
            _minMaxY.x = ground.Transform.position.y + _camera.orthographicSize;
        }
        
    }
}