using System;
using UnityEngine;

namespace ManStretchArm.Code
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private Transform _body;

        [SerializeField] 
        private DistanceJoint2D _distanceJoint;
        
        private bool _isPicked;

        private void Update() {

            if(Input.GetMouseButtonUp(0)) {
            
                _isPicked = false;
                _distanceJoint.enabled = true;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                var mousePos = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
                if ((mousePos - (Vector2) _body.position).sqrMagnitude < 1)
                {
                    _isPicked = true;
                    _distanceJoint.enabled = false;
                }
                
            }
        }

        private void FixedUpdate()
        {
            if(_isPicked) {
                var mousePos = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
                _body.position = mousePos;
                // if you want to smooth movement then lerp it
            }
        }
    }
}