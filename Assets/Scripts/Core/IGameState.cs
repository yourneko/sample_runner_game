using System.Collections;

namespace Runner.Core
{
    /// <summary>Provides an access to load/unload of manager script of the scene.</summary>
    public interface IGameState
    {
        /// <summary>Initialization coroutine.</summary>
        IEnumerator InitRoutine();
        /// <summary>Unloading coroutine.</summary>
        IEnumerator ExitRoutine();
    }
}
