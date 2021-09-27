using System.Collections.Generic;
using ManStretchArm.Code.Interfaces;

namespace ManStretchArm.Code.Controllers
{
    internal sealed class CompositeControllers : IInitialization, IUpdate, IFixedUpdate, ICleanup
    {
        private readonly List<IInitialization> _initializeControllers;
        private readonly List<IUpdate> _updateControllers;
        private readonly List<IFixedUpdate> _fixedUpdateControllers;
        private readonly List<ICleanup> _cleanupControllers;
        
        public CompositeControllers()
        {
            _initializeControllers = new List<IInitialization>();
            _updateControllers = new List<IUpdate>();
            _fixedUpdateControllers = new List<IFixedUpdate>();
            _cleanupControllers = new List<ICleanup>();
        }
        
        public void Add(IController controller)
        {
            if (controller is IInitialization initializeController)
            {
                _initializeControllers.Add(initializeController);
            }

            if (controller is IUpdate executeController)
            {
                _updateControllers.Add(executeController);
            }

            if (controller is IFixedUpdate lateExecuteController)
            {
                _fixedUpdateControllers.Add(lateExecuteController);
            }
            
            if (controller is ICleanup cleanupController)
            {
                _cleanupControllers.Add(cleanupController);
            }
        }
        
        public void Initialization()
        {
            for (var index = 0; index < _initializeControllers.Count; ++index)
            {
                _initializeControllers[index].Initialization();
            }
        }
        
        public void Update(float deltaTime)
        {
            for (var index = 0; index < _updateControllers.Count; ++index)
            {
                _updateControllers[index].Update(deltaTime);
            }
        }
        
        public void FixedUpdate(float deltaTime)
        {
            for (var index = 0; index < _fixedUpdateControllers.Count; ++index)
            {
                _fixedUpdateControllers[index].FixedUpdate(deltaTime);
            }
        }

        public void Cleanup()
        {
            for (var index = 0; index < _cleanupControllers.Count; ++index)
            {
                _cleanupControllers[index].Cleanup();
            }
        }
    }
}