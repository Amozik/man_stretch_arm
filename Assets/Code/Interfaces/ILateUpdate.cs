namespace ManStretchArm.Code.Interfaces
{
    public interface ILateUpdate : IController
    {
        void LateUpdate(float deltaTime);
    }
}