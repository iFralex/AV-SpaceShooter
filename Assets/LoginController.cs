using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace SocialApp
{
    public class LoginController : MonoBehaviour
    {
        public InputField NickNameInput, PasswordInput;
        public Button loginBt;
        public menù menu;

        private void OnEnable()
        {
            ClearFields();
        }

        public void SendLogIn()
        {
            if (!loginBt.interactable)
                return;

            OnLogin(NickNameInput.text.Trim() + "@gmail.com", PasswordInput.text.Trim().ToUpper());
        }

        public void AutoLogin(string _mail, string _password)
        {
            OnLogin(_mail, _password);
        }

        private void OnLogin(string _mail, string _password)
        {
            FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(_mail, _password).ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    MostraMessaggio.mm.Messaggio(MostraMessaggio.Messaggi.errore, "Login was cancelled");
                    print("cance");
                    task.Dispose();
                    return;
                }
                if (task.IsFaulted)
                {
                    print("fallito");
                    MostraMessaggio.mm.Messaggio(MostraMessaggio.Messaggi.errore, "OOPS, there must have been an error. Check your internet connection and that you have entered the correct credentials.");
                    task.Dispose();
                    return;
                }

                FirebaseUser newUser = task.Result;
                print(newUser.UserId);
                task.Dispose();
                MostraMessaggio.mm.Messaggio(MostraMessaggio.Messaggi.successo, "here you are again " + newUser.DisplayName + "!\nWe have been waiting for you to have fun together");
                menu.Loggato(true);
                Firebase.Database.FirebaseDatabase.DefaultInstance.RootReference.Child("Punti").Child(newUser.DisplayName).GetValueAsync().ContinueWithOnMainThread(task3 =>
                {
                    if (task3.IsCanceled)
                    {
                        print("cancellato");
                        return;
                    }
                    if (task3.IsFaulted)
                    {
                        print("fallito");
                        return;
                    }
                    menù.punti = System.Convert.ToInt32(task3.Result.Value);
                });

                Firebase.Database.FirebaseDatabase.DefaultInstance.RootReference.Child("Tempi").Child(newUser.DisplayName).GetValueAsync().ContinueWithOnMainThread(task3 =>
                {
                    if (task3.IsCanceled)
                    {
                        print("cancellato");
                        return;
                    }
                    if (task3.IsFaulted)
                    {
                        PlayerPrefs.SetInt("miglior tempo", System.Convert.ToInt32(3600));
                        return;
                    }
                    PlayerPrefs.SetInt("miglior tempo", System.Convert.ToInt32(task3.Result.Value));
                });
            });
        }

        public void OnLoginSuccess()
        {
            // successo
        }

        private void ClearFields()
        {
            print(gameObject.name + "  " + transform.parent.gameObject.name);
            print(name);
            NickNameInput.text = string.Empty;
            PasswordInput.text = string.Empty;
            loginBt.interactable = false;
        }

        public void CheckError()
        {
            bool IsError = false;
            if (NickNameInput.text.Length < 4)
            {
                IsError = true;
            }
            else if (PasswordInput.text.Length < 6)
            {
                IsError = true;
            }
            loginBt.interactable = !IsError;
        }

        public void OnSignOut()
        {
            
        }
    }
}
