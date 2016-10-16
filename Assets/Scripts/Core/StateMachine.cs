namespace Core
{
    public class StateMachine<T> where T : class
    {
        private T agent;

        private IState<T> currentState;

        private IState<T> defaultState;

        public IState<T> previousState;

        public StateMachine(T agent, IState<T> initialState, IState<T> defaultState = null)
        {
            this.agent = agent;
            this.currentState = initialState;
            this.defaultState = defaultState;
        }

        public IState<T> CurrentState
        {
            get
            {
                return this.currentState;
            }
        }

        public IState<T> DefaultState
        {
            get
            {
                return this.defaultState;
            }
            set
            {
                this.defaultState = value;
            }
        }

        public IState<T> PreviousState
        {
            get
            {
                return this.previousState;
            }
        }

        public void Update()
        {
            this.defaultState.UpdateState(this.agent);
            this.currentState.UpdateState(this.agent);
        }

        public void ChangeState(IState<T> newState)
        {
            if (this.currentState != null)
            {
                this.currentState.Exit(this.agent);
            }

            this.previousState = this.currentState;
            this.currentState = newState;
            this.currentState.Enter(this.agent);
        }
    } 
}
