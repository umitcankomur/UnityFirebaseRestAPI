using UnityEngine;
using UnityEngine.UI;

namespace FirebaseRestAPI.Example
{
    [RequireComponent(typeof(Button))]
    public class SelectTabButton : MonoBehaviour
    {
        [SerializeField] private UIManager.TabName tabName;
        private UIManager uiManager;

        private void Awake()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(() => uiManager.OnTabClick(tabName));
        }
        private void Start()
        {
            uiManager = FindFirstObjectByType<UIManager>();
        }
    }
}
