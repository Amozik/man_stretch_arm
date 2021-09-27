using System;
using UnityEngine;

namespace ManStretchArm.Code
{
    public class DragPlayer : MonoBehaviour
    {
        [SerializeField]
        public float _maxSpeed = 10;
        [SerializeField]
        public float _maxDistance = 2;

        private Vector3 _offset;
        private Vector3 _mousePosition;

        private Rigidbody2D _selectedObject;

        private Vector2 _mouseForce;
        private Vector3 _lastPosition;
        
        private bool _isDragged;

        public bool IsDragged
        {
            get => _isDragged;
            private set
            {
                if (_isDragged != value)
                {
                    if (value)
                        DragStart?.Invoke();
                    else
                        DragEnd?.Invoke();
                }
                
                _isDragged = value;
            }
        }
        
        public event Action DragStart;
        public event Action DragEnd;

        private void Update()
        {
            _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (_selectedObject)
            {
                _mouseForce = (_mousePosition - _lastPosition) / Time.deltaTime;
                _mouseForce = Vector2.ClampMagnitude(_mouseForce, _maxSpeed);
                _lastPosition = _mousePosition;
            }

            if (Input.GetMouseButtonDown(0))
            {
                var layerMask = 1 << 8;
                
                var hit = Physics2D.Raycast(_mousePosition, Vector2.zero, Mathf.Infinity, layerMask);

                if (hit.rigidbody)
                {
                    _selectedObject = hit.rigidbody;
                    _offset = _selectedObject.transform.position - _mousePosition;
                    IsDragged = true;
                }
            }

            if (Input.GetMouseButtonUp(0) && _selectedObject)
            {
                _selectedObject.velocity = Vector2.zero;
                _selectedObject.AddForce(_mouseForce, ForceMode2D.Impulse);
                _selectedObject = null;
                IsDragged = false;
            }
        }

        private void FixedUpdate()
        {
            if (_selectedObject)
            {
                //var position = Vector2.ClampMagnitude(mousePosition + offset, _maxDistance);
                var position = _mousePosition + _offset;
                _selectedObject.MovePosition(position);
            }
        }
    }
}