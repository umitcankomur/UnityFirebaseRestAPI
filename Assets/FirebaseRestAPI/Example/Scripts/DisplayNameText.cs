using TMPro;
using UnityEngine;

namespace FirebaseRestAPI.Example
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class DisplayNameText : MonoBehaviour
    {
        private TextMeshProUGUI text;
        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
        }
        void Update()
        {
            if (FirebaseAuth.IsAuthenticated && FirebaseAuth.CurrentUser.UserData != null)
            {
                text.text = FirebaseAuth.CurrentUser.UserData.DisplayName;
            }
        }
    }
}
