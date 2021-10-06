using ManStretchArm.Code.Interfaces;
using ManStretchArm.Code.Views;
using UnityEngine;

namespace ManStretchArm.Code.Controllers
{
    public class GroundController : IUpdate, ICleanup
    {
        private Player _player;
        private Transform _mainCamera;
        private GroundView _ground;
        private Vector3 _offset;

        private Vector3 _lastCameraPosition;
        private bool _isGameEnded;
        
        public GroundController(Transform mainCamera, Player player, GroundView ground)
        {
            _mainCamera = mainCamera;
            _player = player;
            _ground = ground;
            
            _offset = _ground.transform.position - _mainCamera.position;
            _offset.z = _ground.transform.position.z;
            
            _player.Picked += OnPlayerPicked;
            _ground.LevelObjectContactEvent += OnPlayerDead;
        }

        public void Update(float deltaTime)
        {
            if (_isGameEnded)
                return;
            
            var movement = _mainCamera.position - _lastCameraPosition;
            if (movement.y >= 0.01f)
                _ground.Transform.position = _mainCamera.position + _offset;

            _lastCameraPosition = _mainCamera.position;
        }

        public void Cleanup()
        {
            _player.Picked -= OnPlayerPicked;
        }
        
        private void OnPlayerPicked(bool isPicked, Point point)
        {
            if (isPicked)
            {
                _ground.Transform.position = _mainCamera.position + _offset;
            }
        }
        
        private void OnPlayerDead(GroundView obj)
        {
            _isGameEnded = true;
        }
    }
}