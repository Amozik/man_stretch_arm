using ManStretchArm.Code.Interfaces;
using UnityEngine;

namespace ManStretchArm.Code.Controllers
{
    internal sealed class PlayerInitialization : IInitialization
    {
        private readonly Player _player;

        public PlayerInitialization()
        {
            //TODO: здесь нужно вызывать Instantiane(view)
            _player = Object.FindObjectOfType<Player>();
        }
        
        public Player GetPlayer()
        {
            return _player;
        }

        public void Initialization()
        {

        }
    }
}