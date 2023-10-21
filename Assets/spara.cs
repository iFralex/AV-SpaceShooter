using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class spara : MonoBehaviourPun
{
    public static int danno;
    public GameObject cannone, proiet;
    public AudioClip suonoSparo;
    Transform _target;
    public Transform target
    {
        get { return _target; }
        set
        {
            if (value == _target)
                return;
            _target = value;
            if (value != null)
                if (Vector2.Distance(value.position, transform.position) > 15)
                    value = null;
            variabili.linea.gameObject.SetActive(value != null);
            variabili.sparaBt.interactable = value != null;
            variabili.sparaBt.gameObject.SetActive(value != null);
        }
    }

    void Start()
    {
        if (photonView.IsMine)
        {
            variabili.sparaBt.onClick.AddListener(Spara);
            danno = upgrade.UpgradeData.Get(upgrade.upgrades.power, 0).valoreCorr;
        }
        else
            Destroy(this);
    }

    void Update()
    {
        if (target != null)
        {
            if (Vector2.Distance(target.position, transform.position) > 15)
                target = null;
            else
                variabili.linea.SetPositions(new Vector3[] { transform.position, target.position });
        }
        else
        {
            variabili.linea.gameObject.SetActive(false);
            variabili.sparaBt.interactable = false;
            variabili.sparaBt.gameObject.SetActive(false);
        }

        if (Input.touchCount > 0)
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                RaycastHit2D ray = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero, 100, layerMask: LayerMask.GetMask("player toccabile"));
                if (ray && ray.transform != transform.parent)
                {
                    target = ray.transform;
                    if (target.GetComponentInParent<Vita>())
                        target.GetComponentInParent<Vita>().MostraBarraPiccola();
                    else if (target.GetComponentInParent<AIOnline>())
                        target.GetComponentInParent<AIOnline>().MostraBarraPiccola();
                }
            }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, 15);
            for (int i = 0; i < col.Length; i++)
                if (col[i] != GetComponent<Collider2D>() && (col[i].GetComponent<movimento >() || col[i].GetComponent<AIOnline>()))
                {
                    print(col[i].name);
                    target = col[i].transform;
                    if (target.GetComponentInParent<Vita>())
                        target.GetComponentInParent<Vita>().MostraBarraPiccola();
                    else if (target.GetComponentInParent<AIOnline>())
                        target.GetComponentInParent<AIOnline>().MostraBarraPiccola();
                }
        }

        if (Input.GetKeyDown(KeyCode.S) && target != null)
            Spara();
    }

    public void Spara()
    {
        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan((target.position.y - transform.position.y) / (target.position.x - transform.position.x)) * Mathf.Rad2Deg + (target.position.x > transform.position.x ? -90 : 90));
        GameObject a = PhotonNetwork.Instantiate(proiet.name, cannone.transform.position, transform.rotation, 0);
        a.GetComponent<proiettile>().ownerID = photonView.ViewID;
        a.GetComponent<proiettile>().danno = danno;
        GetComponent<AudioSource>().clip = suonoSparo;
        GetComponent<AudioSource>().Play();
        StartCoroutine(AspettaSparo());
    }

    IEnumerator AspettaSparo()
    {
        variabili.sparaBt.interactable = false;
        yield return new WaitForSeconds(.3f);
        variabili.sparaBt.interactable = true;
    }
}