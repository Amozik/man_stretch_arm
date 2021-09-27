﻿using System;
using ManStretchArm.Code.Interfaces;
using UnityEngine;

namespace ManStretchArm.Code.Controllers
{
    public class CameraController : IUpdate, ICleanup
    {
        private Player _player;
        private Transform _mainCamera;
        
        private Vector3 _offset;

        private Vector2 _minMaxX = Vector2.zero; // The minimum and maximum x coordinates the camera can have.
        private Vector2 _minMaxY = new Vector2(0, Mathf.Infinity); // The minimum and maximum y coordinates the camera can have.
        
        public CameraController(Player player, Transform mainCamera)
        {
            _player = player;
            _mainCamera = mainCamera;
            _offset = _mainCamera.position - _player.Transform.position;
            _offset.z = _mainCamera.position.z;
            
            _offset.y = (_mainCamera.position - _player.PickedPoint.position).y;
            
            _minMaxY.x = _player.PickedPoint.position.y + _offset.y;
            
            _player.Picked += OnPlayerPicked;
        }

        public void Update(float deltaTime)
        {
            // if (_player.IsPicked)
            //     return;
            
            var playerPosition = _player.transform.position;
            var targetX = playerPosition.x;
            var targetY = playerPosition.y;

            targetY = playerPosition.y + _offset.y;

            targetX = Mathf.Clamp(targetX, _minMaxX.x, _minMaxX.y); 
            targetY = Mathf.Clamp(targetY, _minMaxY.x, _minMaxY.y);

            _mainCamera.position = new Vector3(targetX, targetY, _offset.z);
        }

        public void Cleanup()
        {
            _player.Picked -= OnPlayerPicked;
        }

        private void OnPlayerPicked(bool isPicked, Point point)
        {
            if (isPicked)
                _minMaxY.x = point.transform.position.y + _offset.y;
        }
    }
}