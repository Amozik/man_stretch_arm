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
        private GameObject _canPicked;
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

            if (_state == PointState.CanPicked)
            {
                transform.Rotate(0,0, 2.5f);
            }
        }

        private void SetState(PointState state)
        {
            _state = state;

            switch (_state)
            {
                case PointState.Idle:
                    _acive.SetActive(false); 
                    _idle.SetActive(true);
                    _canPicked.SetActive(false);
                    break;
                case PointState.CanPicked:
                    _acive.SetActive(false); 
                    _idle.SetActive(false);
                    _canPicked.SetActive(true);
                    break;
                case PointState.Picked:
                    _acive.SetActive(true); 
                    _idle.SetActive(false);
                    _canPicked.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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