using UnityEngine;

namespace Runner.Game
{
    /// <summary>Pooled static GameObject, placed to one of 3 tracks.</summary>
    interface IObjectOnTrack
    {
        ///<summary>Represents a default position of ObjectOnTrack. (placed on Track #0, with Z = 0)</summary> 
        Vector3 BaseOffset { get; }
        ///<summary>Transform is used to set the position of ObjectOnTrack.</summary>
        Transform transform { get; }
        ///<summary>When player hits the ObjectOnTrack, this method is invoked.</summary>
        void OnPlayerCollision(PlayerAvatar player);
    }
}
