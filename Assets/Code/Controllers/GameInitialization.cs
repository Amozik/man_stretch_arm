using ManStretchArm.Code.Data;
using ManStretchArm.Code.Views;
using UnityEngine;

namespace ManStretchArm.Code.Controllers
{
    internal sealed class GameInitialization
    {
        public GameInitialization(CompositeControllers controllers, GameConfig data)
        {
            Camera camera = Camera.main;
            
            var playerInitialization = new PlayerInitialization();
            var player = playerInitialization.GetPlayer();
            var ground = Object.FindObjectOfType<GroundView>();
            
            controllers.Add(new CameraController(player, camera, ground));
            controllers.Add(new ParallaxController(camera.transform, data.back));
            controllers.Add(new CoinsController());
            controllers.Add(new GroundController(camera.transform, player, ground));
        }
    }
}