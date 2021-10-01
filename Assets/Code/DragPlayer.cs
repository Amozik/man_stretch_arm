using System;
using System.Collections;
using UnityEngine;

namespace ManStretchArm.Code
{
    public class DragPlayer : MonoBehaviour
    {
        [SerializeField]
        private float _frequency = 1.0f;
        [SerializeField]
        private float _dampingRatio = 0.5f;
        [SerializeField]
        private float _drag = 10.0f;
        [SerializeField]
        private float _angularDrag = 5.0f;
        [SerializeField]
        private float _distance = 0.2f;
        
        [SerializeField] 
        private Player _player;

        [SerializeField] 
        private float _maxSpeed = 100;
        
        private SpringJoint2D _springJoint;

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
            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }

            var mainCamera = Camera.main;
            var layerMask = 1 << 8;
            var hit = Physics2D.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition).origin,
                Vector2.zero, Mathf.Infinity,
                layerMask);
            
            if (!hit.rigidbody || hit.rigidbody.isKinematic)
            {
                return;
            }

            if (!_springJoint)
            {
                var go = new GameObject("Rigidbody dragger");
                var body = go.AddComponent<Rigidbody2D>();
                _springJoint = go.AddComponent<SpringJoint2D>();
                body.isKinematic = true;
            }

            _springJoint.transform.position = hit.point;
            _springJoint.anchor = Vector3.zero;

            _springJoint.distance = _distance;
            _springJoint.frequency = _frequency;
            _springJoint.dampingRatio = _dampingRatio;
            _springJoint.autoConfigureDistance = false;
            _springJoint.connectedBody = hit.rigidbody;
            

            IsDragged = true;
            StartCoroutine(nameof(DragObject), hit.distance);
        }
        
        private IEnumerator DragObject(float distance)
        {
            var oldDrag = _springJoint.connectedBody.drag;
            var oldAngularDrag = _springJoint.connectedBody.angularDrag;
            _springJoint.connectedBody.drag = _drag;
            _springJoint.connectedBody.angularDrag = _angularDrag;
            var mainCamera = Camera.main;
            while (Input.GetMouseButton(0))
            {
                var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                _springJoint.transform.position = ray.GetPoint(distance);
                yield return null;
            }

            if (_springJoint.connectedBody)
            {
                _springJoint.connectedBody.drag = oldDrag;
                _springJoint.connectedBody.angularDrag = oldAngularDrag;
                //_springJoint.transform.position = _player.PickedPoint.position*5;
                _springJoint.connectedBody = null;

                var mouseForce = (_player.PickedPoint.position - _springJoint.transform.position);
                _player.Rigidbody.AddForce(Vector2.ClampMagnitude(mouseForce * 12.5f, _maxSpeed), ForceMode2D.Impulse);
                //_springJoint.enabled = false;
                IsDragged = false;
            }
        }
        
    }
}