using UnityEngine;

namespace ManStretchArm.Code.Views
{
    public class BaseView : MonoBehaviour
    {
        [SerializeField]
        private Transform _transform;

        public Transform Transform => _transform;
    }
}