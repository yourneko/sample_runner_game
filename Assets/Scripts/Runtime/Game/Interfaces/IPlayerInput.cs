namespace Runner.Game
{
    /// <summary>Provides an access to player input.</summary>
    interface IPlayerInput 
    {
        /// <summary>MOVE LEFT is -1. MOVE RIGHT is 1. DO NOTHING is 0.</summary>
        /// <remarks>Mechanics read this value on FixedUpdate.</remarks>
        int GetInputValue();
    }
}
