using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.Utility;

namespace ManStretchArm.Code
{
    public class Player : MonoBehaviour
    {
        [SerializeField] 
        private float _forceRatio = 25f;
        [SerializeField]
        private Transform _body;
        [SerializeField] 
        private SpriteRenderer _armSprite;
        [SerializeField] 
        private DistanceJoint2D _distanceJoint;
        [SerializeField] 
        private SpringJoint2D _springJoint;
        [SerializeField] 
        private MoveRigidbody _dragRigidbody;
        [SerializeField] 
        private Line _lineArm;
        [SerializeField]
        private Point _point;
        
        private bool _isPicked;

        public bool IsPicked => _isPicked;
        public Transform Transform
        {
            get => _body;
            private set => _body = value;
        }

        public Rigidbody2D Rigidbody;
        public Transform PickedPoint => _point.transform;
        public Rigidbody2D PickedPointRB => _point.Rigidbody;

        public event Action<bool, Point> Picked;
        
        public bool TryPickPoint(Point point)
        {
            if (_isPicked)
                return false;

            _isPicked = true;
            _springJoint.connectedBody = point.Rigidbody;
            _springJoint.enabled = true;
            _lineArm.EndPoint = point.transform;
            _lineArm.LineRenderer.enabled = true;
            _armSprite.enabled = false;
            _point = point;
            
            Picked?.Invoke(_isPicked, point);

            return true;
        }
        
        
        private void Awake()
        {
            _isPicked = true;
            
            _armSprite.enabled = false;
            _springJoint.distance = 0.1f;
            Rigidbody = _body.GetComponent<Rigidbody2D>();
            
            _dragRigidbody.DragStart += OnDragStart;
            _dragRigidbody.DragEnd += OnDragEnd;
        }

        private void Update() {

            // if(Input.GetMouseButtonUp(0)) {
            //
            //     _isPicked = false;
            //     _distanceJoint.enabled = true;
            // }
            //
            // if (Input.GetMouseButtonDown(0))
            // {
            //     var mousePos = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //
            //     if ((mousePos - (Vector2) _body.position).sqrMagnitude < 1)
            //     {
            //         _isPicked = true;
            //         _distanceJoint.enabled = false;
            //     }
            //     
            // }
        }

        private void FixedUpdate()
        {
            // if(_isPicked) {
            //     var mousePos = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //
            //     _body.position = mousePos;
            //     // if you want to smooth movement then lerp it
            // }
        }

        private void OnDisable()
        {
            _dragRigidbody.DragStart -= OnDragStart;
            _dragRigidbody.DragEnd -= OnDragEnd;
        }

        private void OnDragStart()
        {
            //Debug.Log("Start Drag");
            //_distanceJoint.enabled = false;
        }
        
        private void OnDragEnd()
        {
            if ((_body.position - _point.transform.position).sqrMagnitude < 1.2f)
                return;
            //Debug.Log("End Drag");
            //_distanceJoint.enabled = true;
            
            _lineArm.LineRenderer.enabled = false;
            
            _armSprite.enabled = true;
            _springJoint.connectedBody = null;
            _springJoint.enabled = false;
            
            var mouseForce = (_point.transform.position -_body.position);
            Rigidbody.AddForce(Vector2.ClampMagnitude(mouseForce * _forceRatio, 1000), ForceMode2D.Impulse);

            _isPicked = false;
            _point.UnPick();
            //_point = null;
            Picked?.Invoke(_isPicked, _point);
        }

    }
}