using UnityEngine;
using System.Collections;
using Photon.Pun;

public class proiettile : MonoBehaviourPun
{
    public float velocitàProiet;
    public int ownerID;
    public int danno;
    public bool colpisciAsteroide = true;

    void Start()
    {
        if (photonView.IsMine)
            photonView.RPC("Sincronizza", RpcTarget.Others, ownerID, danno);
        StartCoroutine(Distrouggi());
        GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * velocitàProiet, ForceMode2D.Impulse);
    }

    [PunRPC]
    public void Sincronizza(int v, int d)
    {
        ownerID = v;
        danno = d;
    }

    IEnumerator Distrouggi()
    {
        yield return new WaitForSeconds(5);
        Dist();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (colpisciAsteroide)
        {
            if (coll.gameObject.tag != "Player")
                Dist();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (col.transform.parent.GetComponent<PhotonView>())
                if (ownerID != col.transform.parent.GetComponent<PhotonView>().ViewID)
                {
                    if (col.transform.parent.GetComponent<Vita>())
                    {
                        col.transform.parent.GetComponent<Vita>().Danno(danno, ownerID);
                        col.transform.parent.GetComponent<AudioSource>().clip = col.transform.parent.GetComponent<Vita>().suonoColpito;
                        col.transform.parent.GetComponent<AudioSource>().Play();
                    }
                    else
                        col.transform.parent.GetComponent<AIOnline>().Danno(danno, ownerID);
                    if (col.transform.parent.GetComponent<AudioSource>())
                    {
                        
                    }
                    col.transform.parent.Find("colpito").GetComponent<ParticleSystem>().Play();
                    StartCoroutine(Asp());
                }
        }
        else
        {
            Dist();
        }
    }

    public void Dist()
    {
        Destroy(gameObject);
    }

    IEnumerator Asp()
    {
        yield return new WaitForSeconds(0.5f);
        PhotonNetwork.Destroy(gameObject);
    }
}