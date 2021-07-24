namespace Runner.Game
{
    /// <summary>Provides an access to player input.</summary>
    interface IPlayerInput //todo: attach to UI-in-gameplay parent or to any child of it
    {
        /// <summary>TRUE if PlayerInput can be used on current platform.</summary>
        bool IsValid { get; }
        /// <summary>MOVE LEFT is -1. MOVE RIGHT is 1. DO NOTHING is 0.</summary>
        /// <remarks>Mechanics read this value on FixedUpdate.</remarks>
        int InputValue { get; }
    }
}
