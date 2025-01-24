using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json;

namespace FirebaseRestAPI.Example
{
    public class LoginPanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField emailInput;
        [SerializeField] private TMP_InputField passwordInput;
        [SerializeField] private Button signUpButton;
        [SerializeField] private Button signInButton;
        [SerializeField] private Button passwordResetButton;

        void Awake()
        {
            signUpButton.onClick.AddListener(SignupWithEmailAndPassword);
            signInButton.onClick.AddListener(SignInWithEmailAndPassword);
            passwordResetButton.onClick.AddListener(PasswordReset);
        }

        private void SignupWithEmailAndPassword()
        {
            var email = emailInput.text;
            var password = passwordInput.text;

            FirebaseAuth.SignUpWithEmailAndPassword(email, password, (user) =>
            {
                Toast.Show("User signed up");
                Debug.Log($"User signed up: {JsonConvert.SerializeObject(user)}");

            }, (error) =>
            {
                if (error.Message == AuthError.EMAIL_EXISTS)
                {
                    Toast.Show("Email already exists");
                }
                else if (error.Message == AuthError.OPERATION_NOT_ALLOWED)
                {
                    Toast.Show("Password sign-in is disabled for this project.");
                }
                else if (error.Message == AuthError.WEAK_PASSWORD)
                {
                    Toast.Show("Weak password");
                }
                else
                {
                    Toast.Show($"Signup failed: {error.Message}");
                }
            });
        }
        private void SignInWithEmailAndPassword()
        {
            var email = emailInput.text;
            var password = passwordInput.text;

            FirebaseAuth.SignInWithEmailAndPassword(email, password, (user) =>
            {
                Toast.Show("User signed in");
                Debug.Log($"User signed in: {JsonConvert.SerializeObject(user)}");

            }, (error) =>
            {
                if (error.Message == AuthError.EMAIL_NOT_FOUND)
                {
                    Toast.Show("Email not found");
                }
                else if (error.Message == AuthError.INVALID_PASSWORD)
                {
                    Toast.Show("Invalid password");
                }
                else if (error.Message == AuthError.USER_DISABLED)
                {
                    Toast.Show("User disabled");
                }
                else
                {
                    Toast.Show($"Signin failed: {error.Message}");
                }
            });
        }
        private void PasswordReset()
        {
            var email = emailInput.text;

            if (string.IsNullOrEmpty(email))
            {
                Toast.Show("Email is required");
                return;
            }

            FirebaseAuth.SendPasswordResetEmail(email, () =>
            {
                Toast.Show("Password reset email sent");

            }, (error) =>
            {
                if (error.Message == AuthError.EMAIL_NOT_FOUND)
                {
                    Toast.Show("Email not found");
                }
                else
                {
                    Toast.Show($"Password reset failed: {error.Message}");
                }
            });
        }
    }
}
