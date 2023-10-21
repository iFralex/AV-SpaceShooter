using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;
using System;
using Firebase.Extensions;
using System.Linq;

public class classifica : MonoBehaviour
{
    public static void Aggiorna(int p, string root)
    {
        print("l");
        FirebaseDatabase.DefaultInstance.RootReference.Child(root).Child(Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser.DisplayName).SetValueAsync(p).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
                print("riuscito  " + p);
            else if (task.IsFaulted)
                print("fallito");
            else if (task.IsCanceled)
                print("cancellato");
        });
    }

    public static int Ottieni(string root)
    {
        print("d");
        FirebaseDatabase.DefaultInstance.RootReference.Child(root).Child(Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser.DisplayName).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                print(task.Result.Value);
                return task.Result.Value;
            }
            else
                return null;
        });
        return 0;
    }

    public static void CreaClassifica(string root, RectTransform padre, bool dalPiùAlto = true)
    {
        if (dalPiùAlto)
            FirebaseDatabase.DefaultInstance.RootReference.Child(root).OrderByValue().LimitToLast(10).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    List<DataSnapshot> elenco = task.Result.Children.ToList();
                    /*foreach (DataSnapshot s in task.Result.Children)
                    {
                        print("3");
                        elenco.Add(s);
                        print(4);
                    }*/
                    elenco.Reverse();
                    for (int i = 0; i < elenco.Count; i++)
                    {
                    //print(i + ") " + elenco[i].Key + ": " + elenco[i].Value);
                    padre.GetChild(i).GetChild(0).GetComponent<Text>().text = (i + 1).ToString() + ")";
                        padre.GetChild(i).GetChild(1).GetComponent<Text>().text = Convert.ToString(elenco[i].Key);
                        padre.GetChild(i).GetChild(2).GetComponent<Text>().text = Convert.ToString(elenco[i].Value);
                    }
                    task.Dispose();
                }
            });
        else
            FirebaseDatabase.DefaultInstance.RootReference.Child(root).OrderByValue().LimitToFirst(10).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    List<DataSnapshot> elenco = new List<DataSnapshot>(0);
                    foreach (DataSnapshot s in task.Result.Children)
                        elenco.Add(s);
                    print(elenco[0].Reference.ToString());
                    for (int i = 0; i < elenco.Count; i++)
                    {
                        //print(i + ") " + elenco[i].Key + ": " + elenco[i].Value);
                        padre.GetChild(i).GetChild(0).GetComponent<Text>().text = (i + 1).ToString() + ")";
                        padre.GetChild(i).GetChild(1).GetComponent<Text>().text = Convert.ToString(elenco[i].Key);
                        List<int> lista = corsaManager.secEMin(Convert.ToInt32(elenco[i].Value));
                        padre.GetChild(i).GetChild(2).GetComponent<Text>().text = corsaManager.TempoFormattato(lista[0], lista[1]);
                    }
                    task.Dispose();
                }
            });
    }
}