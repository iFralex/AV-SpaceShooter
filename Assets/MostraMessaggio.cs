using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MostraMessaggio : MonoBehaviour
{
    public enum Messaggi { avviso, errore, successo }
    public class Azione
    {
        public string buttonName;
        public Color buttonColor;
        public List<UnityEngine.Events.UnityAction> eventi;
    }
    public static MostraMessaggio mm;
    public static bool chiudi;

    public Text titoloT;
    public Text messaggioT;
    public List<Button> pulsantiList;
    bool mostrato, blocca;
    Coroutine cor, chiudiCor;

    public void Awake()
    {
        mm = this;
        gameObject.SetActive(false);
    }

    public void Messaggio(Messaggi _tipo, string _msg, List<Azione> azioni = null, float _time = 3)
    {
        if (mostrato)
        {
            print("mostra");
            blocca = true;
            StartCoroutine(ChiudiMessaggio(GetComponent<RectTransform>(), () =>
            {
                //if (cor != null)
                    //StopCoroutine(cor);
                print("aaa");
                if (azioni == null)
                    azioni = new List<Azione>();
                switch (_tipo)
                {
                    case Messaggi.errore:
                        titoloT.color = Color.red;
                        titoloT.text = "ERROR";
                        break;
                    case Messaggi.successo:
                        titoloT.color = Color.green;
                        titoloT.text = "SUCCESS";
                        break;
                    case Messaggi.avviso:
                        titoloT.color = Color.white;
                        titoloT.text = "MESSAGE";
                        break;
                }
                messaggioT.text = _msg;
                for (int i = 0; i < pulsantiList.Count; i++)
                {
                    if (i < azioni.Count)
                    {
                        pulsantiList[i].gameObject.SetActive(true);
                        pulsantiList[i].image.color = azioni[i].buttonColor;
                        pulsantiList[i].GetComponentInChildren<Text>().text = azioni[i].buttonName;
                        pulsantiList[i].interactable = true;
                        pulsantiList[i].onClick.RemoveAllListeners();
                        for (int o = 0; o < azioni[i].eventi.Count; o++)
                            pulsantiList[i].onClick.AddListener(azioni[i].eventi[o]);
                        pulsantiList[i].onClick.AddListener(() => pulsantiList[i].interactable = false);
                    }
                    else
                        pulsantiList[i].gameObject.SetActive(false);
                }
                chiudi = blocca = false;
                print("cioa");
                cor = StartCoroutine(Mostra(_time));
            }));
        }
        else
        {
            if (azioni == null)
                azioni = new List<Azione>();
            switch (_tipo)
            {
                case Messaggi.errore:
                    titoloT.color = Color.red;
                    titoloT.text = "ERROR";
                    break;
                case Messaggi.successo:
                    titoloT.color = Color.green;
                    titoloT.text = "SUCCESS";
                    break;
                case Messaggi.avviso:
                    titoloT.color = Color.white;
                    titoloT.text = "MESSAGE";
                    break;
            }
            messaggioT.text = _msg;
            for (int i = 0; i < pulsantiList.Count; i++)
            {
                if (i < azioni.Count)
                {
                    pulsantiList[i].gameObject.SetActive(true);
                    pulsantiList[i].image.color = azioni[i].buttonColor;
                    pulsantiList[i].GetComponentInChildren<Text>().text = azioni[i].buttonName;
                    pulsantiList[i].interactable = true;
                    pulsantiList[i].onClick.RemoveAllListeners();
                    for (int o = 0; o < azioni[i].eventi.Count; o++)
                        pulsantiList[i].onClick.AddListener(azioni[i].eventi[o]);
                    pulsantiList[i].onClick.AddListener(() => pulsantiList[i].interactable = false);
                }
                else
                    pulsantiList[i].gameObject.SetActive(false);
            }
            gameObject.SetActive(true);
            cor = StartCoroutine(Mostra(_time));
        }
    }

    IEnumerator Mostra(float _time)
    {
        mostrato = true;
        RectTransform rt = GetComponent<RectTransform>();
        for (float i = 0; i < 1f; i += .05f)
        {
            rt.localScale = new Vector2(Mathf.Lerp(rt.localScale.x, 1, i), rt.localScale.y);
            yield return new WaitForSeconds(.03f);
        }
        if (_time > 0)
            yield return new WaitForSeconds(_time);
        else
        {
            yield return new WaitWhile(() => !chiudi);
            chiudi = false;
        }
        print("blocca " + blocca);
        if (!blocca)
            chiudiCor = StartCoroutine(ChiudiMessaggio(rt));
        else
            blocca = false;
    }

    IEnumerator ChiudiMessaggio(RectTransform rt, System.Action action = null)
    {
        StartCoroutine(InterrompiChiudi());
        for (float i = 0; i < 1f; i += .05f)
        {
            rt.localScale = new Vector2(Mathf.Lerp(rt.localScale.x, 0, i), rt.localScale.y);
            yield return new WaitForSeconds(.03f);
        }
        print("chiuso" + (action == null));
        mostrato = false;
        if (action == null)
            gameObject.SetActive(false);
        else
            action.Invoke();
    }

    IEnumerator InterrompiChiudi()
    {
        yield return new WaitWhile(() => !blocca);
        blocca = true;
        if (chiudiCor != null)
            StopCoroutine(chiudiCor);
    }

    public static void Errore(int returnCode, string message)
    {
        mm.Messaggio(
            Messaggi.errore,
            "Error code: " + returnCode + "\n" + message);
    }
}