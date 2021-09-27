using UnityEngine;

namespace ManStretchArm.Code.Data
{
    [CreateAssetMenu(fileName = nameof(GameConfig),  menuName = "Configs/" + nameof(GameConfig), order = 0)]
    public class GameConfig : ScriptableObject
    {
        //public PlayerConfig playerConfig;
        public GameObject back;
    }
}