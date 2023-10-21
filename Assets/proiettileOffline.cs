using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class proiettileOffline : MonoBehaviour
{
    public float velocitàProiet;
    public bool colpisciAsteroide = true;
    public ia proprietarioIA;
    public movimentoOffline proprietario;

    void Start()
    {
        StartCoroutine(Distrouggi());
        GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * velocitàProiet, ForceMode2D.Impulse);
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
            else// if (coll.gameObject.GetComponent<ia>())
            {
                coll.gameObject.GetComponent<ia>().vita -= 1;
                print("dd");
            }
            //print(coll.gameObject.name);

        }
        else
        {
            if (coll.gameObject.tag != "Power up")
            { }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            if (coll.gameObject.GetComponentInParent<ia>())
            {
                if (coll.gameObject.GetComponentInParent<ia>() != proprietarioIA)
                {
                    coll.gameObject.GetComponentInParent<ia>().vita -= 1;
                    Dist();
                }
            }
            else if (coll.gameObject.GetComponentInParent<movimentoOffline>())
                if (coll.gameObject.GetComponentInParent<movimentoOffline>() != proprietario)
                {
                    coll.gameObject.GetComponentInParent<movimentoOffline>().vita -= 1;
                    Dist();
                }
        }
        //print(coll.gameObject.name);
    }

    public void Dist()
    {
        FindObjectOfType<gameManagerOffline>().proiettili.Remove(this);
        Destroy(gameObject);
    }

    IEnumerator Asp()
    {
        yield return new WaitForSeconds(0.5f);
        PhotonNetwork.Destroy(gameObject);
    }
}