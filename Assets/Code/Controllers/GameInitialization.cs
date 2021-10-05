using ManStretchArm.Code.Data;
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
            
            controllers.Add(new CameraController(player, camera.transform));
            controllers.Add(new ParallaxController(camera.transform, data.back));
            controllers.Add(new CoinsController());
        }
    }
}