namespace DefaultNamespace {
    public enum PlayerStates {
        NoBall,
        Aiming,
        Dribbling,
        Throwing,
    }

    public class PlayerState {
        private PlayerStates currentState;
        private static PlayerState instance;
        public static PlayerState Instance => instance ??= new PlayerState();


        public PlayerStates GetCurrentState() => currentState;
        public void SetState_NoBall() => currentState = PlayerStates.NoBall;
        public void SetState_Aiming() => currentState = PlayerStates.Aiming;
        public void SetState_Dribbling() => currentState = PlayerStates.Dribbling;
        public void SetState_Throwing() => currentState = PlayerStates.Throwing;

        public bool IsAiming() => currentState == PlayerStates.Aiming;
        public bool IsThrowing() => currentState == PlayerStates.Throwing;
        public bool HasBall() => currentState != PlayerStates.NoBall && currentState != PlayerStates.Throwing;
    }
}