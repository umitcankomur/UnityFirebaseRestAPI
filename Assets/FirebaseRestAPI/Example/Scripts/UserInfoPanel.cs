using FirebaseRestAPI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FirebaseRestAPI.Example
{
    public class UserInfoPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI userId;
        [SerializeField] private TextMeshProUGUI email;
        [SerializeField] private TextMeshProUGUI displayName;
        [SerializeField] private TextMeshProUGUI photoUrl;
        [SerializeField] private TextMeshProUGUI tokenExpires;
        [SerializeField] private Button getUserDataButton;

        [Header("Change Password")]
        [SerializeField] private TMP_InputField newPasswordInputField;
        [SerializeField] private Button changePasswordButton;

        [Header("Update Profile")]
        [SerializeField] private TMP_InputField displayNameInputField;
        [SerializeField] private TMP_InputField photoUrlInputField;
        [SerializeField] private Button updateProfileButton;

        private void Start()
        {
            changePasswordButton.onClick.AddListener(ChangePassword);
            getUserDataButton.onClick.AddListener(GetUserData);
            updateProfileButton.onClick.AddListener(UpdateProfile);
        }
        void Update()
        {
            if (FirebaseAuth.IsAuthenticated)
            {
                userId.text = $"User Id : {FirebaseAuth.CurrentUser.UserId}";
                email.text = $"Email : {FirebaseAuth.CurrentUser.Email}";
                tokenExpires.text = $"Token expires : {FirebaseAuth.CurrentUser.TokenExpiration}";

                if (FirebaseAuth.CurrentUser.UserData != null)
                {
                    displayName.text = $"Display name : {FirebaseAuth.CurrentUser.UserData.DisplayName}";
                    photoUrl.text = $"Photo url : {FirebaseAuth.CurrentUser.UserData.PhotoUrl}";
                }
            }
        }

        private void ChangePassword()
        {
            string newPassword = newPasswordInputField.text;

            if (string.IsNullOrEmpty(newPassword))
            {
                Toast.Show("New password is empty");
                return;
            }

            FirebaseAuth.ChangePassword(newPassword, () =>
            {
                Toast.Show("Password changed successfully");
            }, (error) =>
            {
                if (error.Message == AuthError.INVALID_ID_TOKEN)
                {
                    Toast.Show("Invalid id token");
                }
                else if (error.Message == AuthError.WEAK_PASSWORD)
                {
                    Toast.Show("Weak password");
                }
                else
                {
                    Toast.Show($"Change password failed: {error.Message}");
                }
            });
        }

        private void GetUserData()
        {
            FirebaseAuth.GetUserData((userData) =>
            {
                Toast.Show("User data received");
            }, (error) =>
            {
                if (error.Message == AuthError.INVALID_ID_TOKEN)
                {
                    Toast.Show("Invalid id token");
                }
                else if (error.Message == AuthError.USER_NOT_FOUND)
                {
                    Toast.Show("User not found");
                }
                else
                {
                    Toast.Show($"Get user data failed: {error.Message}");
                }
            });
        }

        private void UpdateProfile()
        {
            string displayName = displayNameInputField.text;
            string photoUrl = photoUrlInputField.text;

            if (string.IsNullOrEmpty(displayName) && string.IsNullOrEmpty(photoUrl))
            {
                Toast.Show("Display name and photo url are empty");
                return;
            }

            FirebaseAuth.UpdateProfile(displayName, photoUrl, () =>
            {
                Toast.Show("Profile updated successfully");
            }, (error) =>
            {
                if (error.Message == AuthError.INVALID_ID_TOKEN)
                {
                    Toast.Show("Invalid id token");
                }
                else
                {
                    Toast.Show($"Update profile failed: {error.Message}");
                }
            });
        }
    }
}
