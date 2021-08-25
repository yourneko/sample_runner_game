using UnityEngine;

namespace Runner.Game
{
    class KeyboardInput : MonoBehaviour, IPlayerInput
    {
        int inputValue;

        public int GetInputValue() {
            int result = inputValue;
            inputValue = 0;
            return result;
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                inputValue = -1;
            if (Input.GetKeyDown(KeyCode.RightArrow))
                inputValue = 1;
            if (inputValue == -1 && Input.GetKeyUp(KeyCode.LeftArrow)
             || inputValue == 1 && Input.GetKeyUp(KeyCode.RightArrow))
                inputValue = 0;
        }
    }
}
