﻿using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.Utility;

namespace ManStretchArm.Code
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private Transform _body;
        [SerializeField] 
        private SpriteRenderer _armSprite;
        [SerializeField] 
        private DistanceJoint2D _distanceJoint;
        [SerializeField] 
        private SpringJoint2D _springJoint;
        [SerializeField] 
        private DragRigidbody _dragRigidbody;
        [SerializeField] 
        private Line _lineArm;
        [SerializeField]
        private Transform _point;
        
        private bool _isPicked;

        public bool IsPicked => _isPicked;
        public Transform Transform => _body;
        public Transform PickedPoint => _point;

        public event Action<bool, Point> Picked;
        
        public void PickPoint(Point point)
        {
            if (_isPicked)
                return;

            _isPicked = true;
            _springJoint.connectedBody = point.Rigidbody;
            _springJoint.enabled = true;
            _lineArm.EndPoint = point.transform;
            _lineArm.LineRenderer.enabled = true;
            _armSprite.enabled = false;
            _point = point.transform;
            
            Picked?.Invoke(_isPicked, point);
        }
        
        
        private void Awake()
        {
            _isPicked = true;
            
            _armSprite.enabled = false;
            _springJoint.distance = 0.1f;
            
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
            _dragRigidbody.DragStart -= OnDragEnd;
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
            //_springJoint.connectedBody = null;
            StartCoroutine(nameof(DisableJoint));
        }

        private IEnumerator DisableJoint()
        {
            yield return new WaitForSeconds(.15f);
            _springJoint.connectedBody = null;
            _springJoint.enabled = false;
            _isPicked = false;
            Picked?.Invoke(_isPicked, null);
        }
    }
}