using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;

public class corsaManager : tutorial
{
    public Text timerT, tempoCorrenteT;
    public Text[] recordT, nomiRecordT, mioRecordT;
    int minuti, secondi;
    public enum Stati { inGara, vinto, perso, attesa }
    public Stati statoAtt;
    public Sprite[] powerUpsImg;
    public RectTransform comPan, persoPan, vintoPan, listaClassifica;
    Vector3 posIniz;

    public static string TempoFormattato(float min, float sec)
    {
        if (min < 10)
        {
            if (sec < 10)
                return "0" + min.ToString() + ":0" + sec.ToString();
            else
                return "0" + min.ToString() + ":" + sec.ToString();
        }
        else
        {
            if (sec < 10)
                return min.ToString() + ":0" + sec.ToString();
            else
                return min.ToString() + ":" + sec.ToString();
        }
    }

    public static List<int> secEMin(int secondi)
    {
        return new List<int> { secondi / 60, secondi % 60 };
    }

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = menù.skin;
        posIniz = transform.position;
        AggiornaDati();
    }

    void AggiornaDati()
    {
        classifica.CreaClassifica("Tempi", listaClassifica, false);
        FirebaseDatabase.DefaultInstance.RootReference.Child("Tempi").OrderByValue().LimitToFirst(1).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                int risult = 0;
                string _nomi = "";
                foreach (DataSnapshot o in task.Result.Children)
                {
                    risult = System.Convert.ToInt32(o.Value);
                    _nomi = System.Convert.ToString(o.Key);
                }
                string _tempo = TempoFormattato(secEMin(risult)[0], secEMin(risult)[1]);
                print(_nomi + ": " + _tempo);
                for (int i = 0; i < recordT.Length; i++)
                    recordT[i].text = _tempo;
                for (int i = 0; i < nomiRecordT.Length; i++)
                    nomiRecordT[i].text = _nomi;
                task.Dispose();
            }
        });

        int tempo = 3600;
        if (PlayerPrefs.HasKey("miglior tempo"))
            tempo = PlayerPrefs.GetInt("miglior tempo");
        else
            PlayerPrefs.SetInt("tempo migliore", 3600);

        for (int i = 0; i < mioRecordT.Length; i++)
            mioRecordT[i].text = TempoFormattato(secEMin(tempo)[0], secEMin(tempo)[1]);
    }

    IEnumerator Timer()
    {
        for (; statoAtt == Stati.inGara;)
        {
            yield return new WaitForSeconds(1);
            secondi++;
            if (secondi >= 60)
            {
                secondi = 0;
                minuti++;
            }

            timerT.text = TempoFormattato(minuti, secondi);
        }
    }

    public void IniziaCorsa() => Azione(Stati.inGara);

    void Azione(Stati st)
    {
        statoAtt = st;
        switch (st)
        {
            case Stati.inGara:
                comPan.gameObject.SetActive(true);
                vintoPan.parent.gameObject.SetActive(false);
                transform.position = posIniz;
                transform.eulerAngles = new Vector3(0, 0, 90);
                vita = carburante = 10;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Rigidbody2D>().angularVelocity = 0;
                minuti = secondi = 0;
                StartCoroutine(Timer());
                break;
            case Stati.vinto:
                AggiornaDati();
                vintoPan.parent.gameObject.SetActive(true);
                vintoPan.gameObject.SetActive(true);
                comPan.gameObject.SetActive(false);
                int tempo = minuti * 60 + secondi;
                classifica.Aggiorna(tempo, "Tempi");
                if (tempo <= PlayerPrefs.GetInt("miglior tempo"))
                {
                    PlayerPrefs.SetInt("miglior tempo", tempo);
                    vintoPan.GetChild(1).gameObject.SetActive(false);
                    vintoPan.GetChild(2).gameObject.SetActive(true);
                    vintoPan.GetChild(3).gameObject.SetActive(false);
                    for (int i = 0; i < mioRecordT.Length; i++)
                        mioRecordT[i].text = TempoFormattato(secEMin(tempo)[0], secEMin(tempo)[1]);
                }
                else
                {
                    vintoPan.GetChild(1).gameObject.SetActive(true);
                    vintoPan.GetChild(2).gameObject.SetActive(false);
                    vintoPan.GetChild(3).gameObject.SetActive(false);
                }

                string s = listaClassifica.GetChild(0).GetChild(2).GetComponent<Text>().text;
                int s1 = System.Convert.ToInt32("" + s[0] + s[1]), s2 = System.Convert.ToInt32("" + s[3] + s[4]);
                if (s1 * 60 + s2 >= tempo)
                {
                    vintoPan.GetChild(1).gameObject.SetActive(false);
                    vintoPan.GetChild(2).gameObject.SetActive(false);
                    vintoPan.GetChild(3).gameObject.SetActive(true);
                    for (int i = 0; i < mioRecordT.Length; i++)
                        mioRecordT[i].text = TempoFormattato(secEMin(PlayerPrefs.GetInt("miglior tempo"))[0], secEMin(PlayerPrefs.GetInt("miglior tempo"))[1]);
                }
                tempoCorrenteT.text = TempoFormattato(minuti, secondi);
                menù.punti += 10;
                break;
            case Stati.perso:
                classifica.CreaClassifica("Tempi", listaClassifica, false);
                vintoPan.parent.gameObject.SetActive(true);
                persoPan.gameObject.SetActive(true);
                comPan.gameObject.SetActive(false);
                break;
        }
    }

    public override void OnCollisionEnter2D(Collision2D col)
    {
        if (statoAtt == Stati.inGara)
            if (col.gameObject.tag == "ostacolo")
            {
                GetComponent<AudioSource>().clip = suonoColpito;
                GetComponent<AudioSource>().Play();
                if (vita > 0)
                {
                    float d = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.magnitude * danno);
                    if (vita - d >= 0)
                        vita -= Mathf.Abs(GetComponent<Rigidbody2D>().velocity.magnitude * danno);
                    else
                        vita = 0;

                    if (vita == 0)
                        Azione(Stati.perso);
                }
            }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "linea traguardo")
            Azione(Stati.vinto);
    }

    public void PublicaTempo(int sec)
    {
        
    }
}