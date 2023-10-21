using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Extensions;
using System.Collections;

public class RegistrationController : MonoBehaviour
{
    private FirebaseAuth Auth;
    public InputField NickNameInput, PasswordInput, ConfermPasswordInput;
    public Button registraBt;
    public Text nomeAccount, nomeAccountRiq;
    public menù menu;
    
    void OnEnable()
    {
        NickNameInput.text = "";
        PasswordInput.text = "";
        ConfermPasswordInput.text = "";
        registraBt.interactable = false;
    }

    void Start()
    {
        Auth = FirebaseAuth.DefaultInstance;
    }

    public void CheckError()
    {
        bool IsError = false;

        if (PasswordInput.text.Length < 4)
        {
            IsError = true;
        }
        if (ConfermPasswordInput.text.Length < 6)
        {
            IsError = true;
        }
        if (ConfermPasswordInput.text != PasswordInput.text)
        {
            IsError = true;
        }
        if (NickNameInput.text.Length < 4)
        {
            IsError = true;
        }
        registraBt.interactable = !IsError;
    }

    public void SendRegistration()
    {
        if (!registraBt.interactable)
            return;
        string s = NickNameInput.text;
        FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(NickNameInput.text.Trim() + "@gmail.com", PasswordInput.text.Trim().ToUpper()).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                MostraMessaggio.mm.Messaggio(MostraMessaggio.Messaggi.errore, "Registration cancelled");
                print("cance");
                task.Dispose();
                return;
            }
            if (task.IsFaulted)
            {
                MostraMessaggio.mm.Messaggio(MostraMessaggio.Messaggi.errore, "OOPS, there must have been an error. Check your internet connection and that you have entered the correct data.");
                print("task.");
                task.Dispose();
                return;
            }
            if (task.IsCompleted)
            {
                FirebaseUser newUser = task.Result;
                UserProfile profilo = new UserProfile { DisplayName = s };
                print(s);
                Auth.CurrentUser.UpdateUserProfileAsync(profilo).ContinueWithOnMainThread(task1 =>
                {
                    if (task1.IsCanceled)
                    {
                        Debug.LogError("UpdateUserProfileAsync was canceled.");
                        return;
                    }
                    if (task1.IsFaulted)
                    {
                        Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                        return;
                    }
                    if (task1.IsCompleted)
                    {
                        print("dddw  " + Auth.CurrentUser.DisplayName);
                        menu.Loggato(true);
                        Debug.Log("User profile updated successfully.");
                        MostraMessaggio.mm.Messaggio(MostraMessaggio.Messaggi.successo, "Hi " + s + ", welcome! \nAre you ready to play?");

                        Firebase.Database.FirebaseDatabase.DefaultInstance.RootReference.Child("Punti").Child(s).SetValueAsync(0).ContinueWith(task2 =>
                        {
                            if (task2.IsCompleted)
                                menù.punti = 0;
                            else if (task2.IsFaulted)
                                print("fallito");
                            else if (task2.IsCanceled)
                                print("cancellato");
                        });

                        Firebase.Database.FirebaseDatabase.DefaultInstance.RootReference.Child("Tempi").Child(newUser.DisplayName).SetValueAsync(3600).ContinueWithOnMainThread(task3 =>
                        {
                            if (task3.IsCanceled)
                            {
                                print("cancellato");
                                return;
                            }
                            if (task3.IsFaulted)
                            {
                                //PlayerPrefs.SetInt(newUser.DisplayName, System.Convert.ToInt32(3600));
                                return;
                            }
                            PlayerPrefs.SetInt("miglior tempo", 3600);
                        });
                    }
                });
                task.Dispose();
            }
        });
    }

    IEnumerator AggiornaNomi(string s)
    {
        print(4);
        yield return new WaitWhile(() => Auth.CurrentUser.DisplayName == "");
        print("1");
        FindObjectOfType<menù>().Loggato(true, s);
        print(2);
    }

    public void OnRegistrationComplete()
    {
        // registrazione completata
    }

    public void OnLoginComplete()
    {
        // login riuscito
    }

    public void OnSetUserDataComplete()
    {
        // dati scaricati
    }
}