namespace Runner.Game
{
    /// <summary>Pooled static GameObject, placed to one of 3 tracks.</summary>
    interface IObjectOnTrack
    {
        ObjectType Type { get; }
        void Hit();
    }
}
