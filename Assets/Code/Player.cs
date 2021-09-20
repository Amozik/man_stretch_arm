using System;
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
        private Transform _point;
        
        private bool _isPicked;

        private void Awake()
        {
            _armSprite.enabled = false;
            _springJoint.distance = 0.02f;
            
            _dragRigidbody.OnDragStart += OnDragStart;
            _dragRigidbody.OnDragEnd += OnDragEnd;
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
            _dragRigidbody.OnDragStart -= OnDragStart;
            _dragRigidbody.OnDragStart -= OnDragEnd;
        }

        private void OnDragStart()
        {
            Debug.Log("Start Drag");
            _distanceJoint.enabled = false;
        }
        
        private void OnDragEnd()
        {
            if ((_body.position - _point.transform.position).sqrMagnitude < 1.2f)
                return;
            Debug.Log("End Drag");
            //_distanceJoint.enabled = true;
            _armSprite.enabled = true;
            _springJoint.connectedBody = null;
            StartCoroutine(nameof(DisableJoint));
        }

        private IEnumerator DisableJoint()
        {
            yield return new WaitForSeconds(.1f);
            _springJoint.enabled = false;
        }
    }
}