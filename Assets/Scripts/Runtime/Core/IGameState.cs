using System.Collections;

namespace Runner.Core
{
    /// <summary>Provides an access to load/unload of manager script of the scene.</summary>
    public interface IGameState
    {
        IEnumerator Init();
        IEnumerator Exit();
    }
}
