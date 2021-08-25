using UnityEngine;
using UnityEngine.UI;

namespace Runner.Menu
{
    class FractionSelector : MonoBehaviour
    {
        const string SELECTED_FRACTION_PLAYER_PREFS_ID = "SelectedFraction";
        
        [SerializeField] GameObject leftSelected, rightSelected;
        [SerializeField] Button selectLeftBtn, selectRightBtn;
        int fraction;
        bool changed;

        void Start() {
            SelectFraction(PlayerPrefs.HasKey(SELECTED_FRACTION_PLAYER_PREFS_ID)
                               ? PlayerPrefs.GetInt(SELECTED_FRACTION_PLAYER_PREFS_ID)
                               : 0);
            selectLeftBtn.onClick.AddListener(() => {
                SelectFraction(-1);
                changed = true;
            });
            selectRightBtn.onClick.AddListener(() => {
                SelectFraction(1);
                changed = true;
            });
        }

        void SelectFraction(int f) {
            fraction = f;
            leftSelected.SetActive(fraction == -1);
            rightSelected.SetActive(fraction == 1);
            selectLeftBtn.gameObject.SetActive(fraction != -1);
            selectRightBtn.gameObject.SetActive(fraction != 1);
        }

        void OnDestroy() {
            if (!changed) return;
            
            PlayerPrefs.SetInt(SELECTED_FRACTION_PLAYER_PREFS_ID, fraction);
            PlayerPrefs.Save();
        }
    }
}
