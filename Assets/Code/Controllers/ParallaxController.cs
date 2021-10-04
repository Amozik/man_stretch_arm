using ManStretchArm.Code.Extensions;
using ManStretchArm.Code.Interfaces;
using UnityEngine;

namespace ManStretchArm.Code.Controllers
{
    public class ParallaxController : IUpdate
    {
        private Transform _camera;
        private Transform _back;
        private Vector3 _backStartPosition;
        private Vector3 _cameraStartPosition;
        private Vector2 _multiplier = new Vector2(0.4f, .5f);
        private float offset = 12;

        public ParallaxController(Transform camera, GameObject back)
        {
            _camera = camera;
            _back = Object.Instantiate(back).transform;
            _backStartPosition = _back.transform.position;
            _cameraStartPosition = _camera.transform.position;

            var doubleBack = Object.Instantiate(back, _back).transform;
            doubleBack.position = doubleBack.position.Change(y: offset);
            
            //TODO можно обойтись двумя, но почему-то не работает
            var thirdBack = Object.Instantiate(back, _back).transform;
            thirdBack.position = thirdBack.position.Change(y: -offset);
        }

        public void Update(float deltaTime)
        {
            var cameraPosition = _camera.position;
            var backPosition = _back.position;
            var deltaCameraPosition = cameraPosition - _cameraStartPosition;
            
            backPosition += (Vector3)(deltaCameraPosition * _multiplier);
            _cameraStartPosition = cameraPosition;

            var offsetPositionY = Mathf.Repeat(cameraPosition.y - backPosition.y, offset);
            backPosition = backPosition.Change(y: cameraPosition.y - offsetPositionY);
            _back.position = backPosition;
        }
    }
}