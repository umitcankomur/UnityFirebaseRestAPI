using UnityEngine;
using UnityEngine.UI;

namespace FirebaseRestAPI.Example
{
    [RequireComponent(typeof(Button))]
    public class SignOutButton : MonoBehaviour
    {
        private void Start()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                FirebaseAuth.SignOut();
            });
        }
    }
}
