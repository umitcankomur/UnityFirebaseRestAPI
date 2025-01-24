using FirebaseRestAPI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace FirebaseRestAPI.Example
{
    public class UIManager : MonoBehaviour
    {
        public enum TabName
        {
            Database,
            Firestore,
            UserInfo
        }
        [System.Serializable]
        public class Tab
        {
            public TabName name;
            public GameObject panel;
            public Button button;
        }

        [SerializeField] private GameObject loginPanel;
        [SerializeField] private GameObject userPanel;
        [SerializeField] private Tab[] tabs;

        private TabName selectedTab;

        private void Start()
        {
            SetSelectedTab(TabName.UserInfo);
        }
        private void Update()
        {
            if (FirebaseAuth.IsAuthenticated)
            {
                loginPanel.SetActive(false);
                userPanel.SetActive(true);

                foreach (var tab in tabs)
                {
                    tab.button.image.color = tab.name == selectedTab ? Color.cyan : Color.white;
                }
            }
            else
            {
                loginPanel.SetActive(true);
                userPanel.SetActive(false);
            }
        }

        public void SetSelectedTab(TabName tabName)
        {
            selectedTab = tabName;

            foreach (var tab in tabs)
            {
                tab.panel.SetActive(tab.name == tabName);
            }
        }

        public void OnTabClick(TabName tabName)
        {
            SetSelectedTab(tabName);
        }
    }
}
