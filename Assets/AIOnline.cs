using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AIOnline : MonoBehaviourPun
{
    public GameObject cannone, proiet;
    float _vita = 100;
    public float vita
    {
        get { return _vita; }
        set
        {
            if (_vita != value)
                _vita = value;
            if (value <= 0)
                PhotonNetwork.Destroy(photonView);
        }
    }
    float tempo;
    public UnityEngine.UI.Text dannoT, nickNameT;
    bool mostrato;
    public RectTransform barraVitaPiccola;

    void Start()
    {
        vita = 100;
        StartCoroutine(Muoviti());
    }

    void Update()
    {
        tempo += Time.deltaTime;
        if (tempo >= 6)
        {
            tempo = 5;
            vita = Mathf.Min(vita + 2f, 100);
        }
        nickNameT.transform.parent.rotation = Quaternion.Euler(Vector3.zero);
    }

    IEnumerator Muoviti()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (!photonView.IsMine)
                continue;
            for (int i = 0; i < variabili.pianeti.Count; i++)
                if (((Vector2)variabili.pianeti[i].transform.position - (Vector2)transform.position).sqrMagnitude < Mathf.Pow(variabili.pianeti[i].transform.lossyScale.x / 2 + variabili.pianeti[i].maxDistance, 2))
                {
                    transform.eulerAngles = new Vector3(0, 0, Mathf.Atan((transform.position.y - variabili.pianeti[i].transform.position.y) / (transform.position.x - variabili.pianeti[i].transform.position.x)) * Mathf.Rad2Deg + (transform.position.x > variabili.pianeti[i].transform.position.x ? -90 : 90) + Random.Range(-30, 30));
                    break;
                }
            float dist = 10000;
            float a = 0;
            GameObject _a = null;
            for (int i = variabili.players.Count - 1; i >= 0; i--)
            {
                if (variabili.players[i] == null)
                {
                    variabili.players.RemoveAt(i);
                    continue;
                }
                a = (variabili.players[i].transform.position - transform.position).sqrMagnitude;
                if (a < 225 && dist > a)
                {
                    dist = a;
                    _a = variabili.players[i];
                }
            }
            if (_a != null)
            {
                transform.eulerAngles = new Vector3(0, 0, Mathf.Atan((transform.position.y - _a.transform.position.y) / (transform.position.x - _a.transform.position.x)) * Mathf.Rad2Deg + (transform.position.x > _a.transform.position.x ? -90 : 90) + 180);
                Spara(_a.transform);
            }
            else
                transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + Random.Range(-5f, 5f));
            GetComponent<Rigidbody2D>().velocity = transform.up * 2;
            GetComponent<Rigidbody2D>().angularVelocity = 0;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + 180 + Random.Range(-30, 30));
        /*float d = 0;
        switch (col.gameObject.tag)
        {
            case "ostacolo":
                d = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.magnitude * dannoAsteroide);
                transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + 180 + Random.Range(-30, 30));
                break;
            case "stella":
                d = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.magnitude * dannoPianeta);
                transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + 180 + Random.Range(-30, 30));
                break;
            case "pianeta":
                d = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.magnitude * dannoStella);
                break;
        }
        if (d == 0)
            return;
        GetComponent<AudioSource>().clip = suonoColpito;
        GetComponent<AudioSource>().Play();
        if (vita > 0)
        {
            if (vita - d >= 0)
            {
                vita -= d;
            }
            else
            {
                vita = 0;
            }
        }*/
    }

    public void Spara(Transform target)
    {
        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan((target.position.y - transform.position.y) / (target.position.x - transform.position.x)) * Mathf.Rad2Deg + (target.position.x > transform.position.x ? -90 : 90));
        GameObject a = PhotonNetwork.Instantiate(proiet.name, cannone.transform.position, transform.rotation, 0);
        a.GetComponent<proiettile>().ownerID = photonView.ViewID;
        a.GetComponent<proiettile>().danno = 10;
    }

    public void Danno(float d, int viewID = -1)
    {
        tempo = 0;
        vita -= d;
        if (photonView.IsMine)
        {
            photonView.RPC("DannoPun", RpcTarget.All, d);
            if (viewID >= 0 && vita <= 0)
                PhotonView.Find(viewID).RPC("AggiungiPunti", RpcTarget.All, 1);
        }
    }

    [PunRPC]
    public void DannoPun(float d)
    {
        dannoT.text = ((int)d).ToString();
        if (mostrato)
        {
            StopCoroutine(MostraDanno());
            mostrato = false;
        }
        barraVitaPiccola.localScale = new Vector3(vita / 100f, 1, 1);
        StartCoroutine(MostraDanno());
    }

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
            menù.punti += p;
        }
    }
}