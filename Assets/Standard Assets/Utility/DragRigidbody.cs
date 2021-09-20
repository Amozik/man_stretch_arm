using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class DragRigidbody : MonoBehaviour
    {
        const float frequency = 8.0f;
        const float k_Damper = 0.5f;
        const float k_Drag = 10.0f;
        const float k_AngularDrag = 5.0f;
        const float k_Distance = 0.2f;
        const bool k_AttachToCenterOfMass = false;

        private SpringJoint2D m_SpringJoint;

        private bool _isDragged;

        public bool IsDragged
        {
            get => _isDragged;
            private set
            {
                if (_isDragged != value)
                {
                    if (value)
                        OnDragStart?.Invoke();
                    else
                        OnDragEnd?.Invoke();
                }
                
                _isDragged = value;
            }
        }

        public Action OnDragStart;
        public Action OnDragEnd;

        private void Update()
        {
            // Make sure the user pressed the mouse down
            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }

            var mainCamera = FindCamera();

            var layerMask = 1 << 8;
            
            // We need to actually hit an object
            var hit = Physics2D.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition).origin,
                                 Vector2.zero, Mathf.Infinity,
                                 layerMask);
            
            // We need to hit a rigidbody that is not kinematic
            if (!hit.rigidbody || hit.rigidbody.isKinematic)
            {
                return;
            }

            if (!m_SpringJoint)
            {
                var go = new GameObject("Rigidbody dragger");
                var body = go.AddComponent<Rigidbody2D>();
                m_SpringJoint = go.AddComponent<SpringJoint2D>();
                body.isKinematic = true;
            }

            m_SpringJoint.transform.position = hit.point;
            m_SpringJoint.anchor = Vector3.zero;

            m_SpringJoint.distance = k_Distance;
            m_SpringJoint.frequency = frequency;
            m_SpringJoint.dampingRatio = k_Damper;
            //m_SpringJoint.maxDistance = k_Distance;
            m_SpringJoint.autoConfigureDistance = false;
            m_SpringJoint.connectedBody = hit.rigidbody;
            

            IsDragged = true;
            StartCoroutine(nameof(DragObject), hit.distance);
        }


        private IEnumerator DragObject(float distance)
        {
            var oldDrag = m_SpringJoint.connectedBody.drag;
            var oldAngularDrag = m_SpringJoint.connectedBody.angularDrag;
            m_SpringJoint.connectedBody.drag = k_Drag;
            m_SpringJoint.connectedBody.angularDrag = k_AngularDrag;
            var mainCamera = FindCamera();
            while (Input.GetMouseButton(0))
            {
                var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                m_SpringJoint.transform.position = ray.GetPoint(distance);
                yield return null;
            }
            if (m_SpringJoint.connectedBody)
            {
                m_SpringJoint.connectedBody.drag = oldDrag;
                m_SpringJoint.connectedBody.angularDrag = oldAngularDrag;
                m_SpringJoint.connectedBody = null;
                IsDragged = false;
            }
        }


        private Camera FindCamera()
        {
            if (GetComponent<Camera>())
            {
                return GetComponent<Camera>();
            }

            return Camera.main;
        }
    }
}
