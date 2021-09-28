using System;
using UnityEngine;

namespace ManStretchArm.Code
{
    enum PointState
    {
        Idle,
        Picked,
        CanPicked,
    }
    
    public class Point : MonoBehaviour
    {
        [SerializeField] 
        private GameObject _acive;
        [SerializeField] 
        private GameObject _idle;
        [SerializeField]
        private PointState _state = PointState.Idle;

        private Player _player;

        public Rigidbody2D Rigidbody;

        public void UnPick()
        {
            SetState(PointState.Idle);
        }
        
        private void Awake()
        {
            SetState(_state);

            _player = FindObjectOfType<Player>();
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (_state == PointState.CanPicked)
                {
                    if (_player.TryPickPoint(this))
                        SetState(PointState.Picked);
                }
            }
        }

        private void SetState(PointState state)
        {
            _state = state;
            
            if (_state == PointState.Idle)
            {
                _acive.SetActive(false); 
                _idle.SetActive(true);
            }
            else
            {
                _acive.SetActive(true); 
                _idle.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && _state != PointState.Picked)
            {
                SetState(PointState.CanPicked);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player") && _state == PointState.CanPicked)
            {
                SetState(PointState.Idle);
            }
        }
    }
}