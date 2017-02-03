//The basic class that all different kinds of steps inherit from
namespace Step
{

    [System.Serializable]
    public abstract class Step
    {
        public enum StepType
        {
            CREATE_ACTOR, RECEIVE_MESSAGE, SEND_MESSAGE, CHANGE_STATE
        }

        public StepType typeOfStep;

    }
}
