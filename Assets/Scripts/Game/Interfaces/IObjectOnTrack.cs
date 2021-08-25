namespace Runner.Game
{
    /// <summary>Pooled static GameObject, placed to one of tracks.</summary>
    interface IObjectOnTrack
    {
        ObjectType GetObjectType();
    }
}
