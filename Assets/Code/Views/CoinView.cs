using System;
using UnityEngine;

namespace ManStretchArm.Code.Views
{
    public class CoinView : BaseView
    {
        public event Action<CoinView> LevelObjectContactEvent;

        private void Update()
        {
            Transform.Rotate(0, 0, 0.5f);   
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                LevelObjectContactEvent?.Invoke(this);
            }
        }
    }
}