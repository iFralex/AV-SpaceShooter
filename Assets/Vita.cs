using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Vita : MonoBehaviourPun
{
    public static int guarireVel;
    public static int vitaMax;
    public float vita, dannoAsteroide, dannoPianeta, dannoStella, tempo;
    public RectTransform barraVita, barraVitaPiccola;
    public AudioClip suonoColpito;
    public Text dannoT;
    bool mostrato;

    void Start()
    {
        if (photonView.IsMine)
        {
            vitaMax = upgrade.UpgradeData.Get(upgrade.upgrades.maxHealth, 0).valoreCorr;
            guarireVel = upgrade.UpgradeData.Get(upgrade.upgrades.healing, 0).valoreCorr;
            vita = vitaMax;
            barraVita = variabili.barraVita;
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            tempo += Time.deltaTime * guarireVel;
            if (tempo >= 6)
            {
                tempo = 5;
                vita = Mathf.Min(vita + 2, vitaMax);
                barraVita.localScale = new Vector3(vita / vitaMax, 1, 1);
                barraVita.GetComponent<Image>().color = Color.Lerp(vita > vitaMax / 2 ? Color.yellow : Color.red, vita > vitaMax / 2 ? Color.green : Color.yellow, vita / (vitaMax / 2) - (vita > vitaMax / 2 ? 1 : 0));
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        float d = 0;
        switch (col.gameObject.tag)
        {
            case "ostacolo":
                d = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.magnitude * dannoAsteroide);
                break;
            case "stella":
                d = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.magnitude * dannoPianeta);
                break;
            case "pianeta":
                d = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.magnitude * dannoStella);
                   break;
        }
        if (d == 0)
            return;
        Danno(d);
    }

    public void Danno(float d, int viewID = -1)
    {
        tempo = 0;
        GetComponent<AudioSource>().clip = suonoColpito;
        GetComponent<AudioSource>().Play();
        vita -= d;
        if (photonView.IsMine)
        {
            if (vita <= 0)
            {
                transform.position = partitaManager.pm.PosCasualePlayer();
                transform.localScale = Vector3.one * .3f;
                vita = vitaMax;
                if (viewID >= 0)
                    PhotonView.Find(viewID).RPC("AggiungiPunti", RpcTarget.All, 1);
            }
            barraVita.localScale = new Vector3(vita / vitaMax, 1, 1);
            barraVita.GetComponent<Image>().color = Color.Lerp(vita > vitaMax / 2 ? Color.yellow : Color.red, vita > vitaMax / 2 ? Color.green : Color.yellow, vita / (vitaMax / 2) - (vita > vitaMax / 2 ? 1 : 0));
        }
        barraVitaPiccola.localScale = barraVita.localScale;
        if (photonView.IsMine)
            photonView.RPC("DannoPun", RpcTarget.All, d);
    }

    [PunRPC]
    public void DannoPun(float d)
    {
        dannoT.text = ((int)d).ToString();
        if (mostrato)
        {
            StopCoroutine(dannoCor);
            mostrato = false;
        }
        dannoCor = StartCoroutine(MostraDanno());
    }
    Coroutine dannoCor;
    IEnumerator MostraDanno()
    {
        mostrato = true;
        dannoT.gameObject.SetActive(true);
        for (float i = 0; i < 1f; i += .05f)
        {
            dannoT.rectTransform.anchoredPosition = new Vector2(0, Mathf.Lerp(1, 3, i));
            dannoT.color = new Color(1, 1, 1, 1 - i);
            yield return new WaitForSeconds(.05f);
        }
        dannoT.gameObject.SetActive(false);
        mostrato = false;
    }

    public void MostraBarraPiccola()
    {
        StartCoroutine(MostraBarra());
    }

    IEnumerator MostraBarra()
    {
        barraVitaPiccola.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        barraVitaPiccola.gameObject.SetActive(false);
    }

    [PunRPC]
    public void AggiungiPunti(int p)
    {
        if (photonView.IsMine)
        {
            partitaManager.punti += p;
            menù.punti += 1;
        }
    }
}