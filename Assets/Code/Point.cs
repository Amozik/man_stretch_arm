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

        private PointState _state = PointState.Idle;

        private Player _player;

        public Rigidbody2D Rigidbody;
        
        
        private void Awake()
        {
            SetState(PointState.Idle);

            _player = FindObjectOfType<Player>();
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (_state == PointState.CanPicked)
                {
                    _player.PickPoint(this);
                }
            }
        }

        private void SetState(PointState state)
        {
            _state = state;
            
            if (_state == PointState.CanPicked)
            {
                _acive.SetActive(true); 
                _idle.SetActive(false);
            }
            else
            {
                _acive.SetActive(false); 
                _idle.SetActive(true);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                SetState(PointState.CanPicked);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                SetState(PointState.Idle);
            }
        }
    }
}