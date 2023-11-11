

namespace Game.StateMachineManagement
{
    public interface IState
    {

        void OnFixedUpdate();//Add Fixed Update method in State
        void OnUpdate();
        void OnEnter();
        void OnExit();    
    }

}
