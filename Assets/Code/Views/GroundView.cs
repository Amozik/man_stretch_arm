using System;
using UnityEngine;

namespace ManStretchArm.Code.Views
{
    public class GroundView : BaseView
    {
        public event Action<GroundView> LevelObjectContactEvent;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                LevelObjectContactEvent?.Invoke(this);
            }
        }
    }
}