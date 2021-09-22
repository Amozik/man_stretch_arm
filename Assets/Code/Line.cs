using System;
using UnityEngine;

namespace ManStretchArm.Code
{
    public class Line : MonoBehaviour
    {
        [SerializeField] 
        private Transform _startPoint;
        
        [SerializeField] 
        private Transform _endPoint;

        private LineRenderer _lineRenderer;

        public LineRenderer LineRenderer => _lineRenderer;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.useWorldSpace = true;
        }

        private void Update()
        {
            _lineRenderer.SetPosition(0, (Vector2) _startPoint.position);
            _lineRenderer.SetPosition(1, (Vector2) _endPoint.position);
        }
    }
}