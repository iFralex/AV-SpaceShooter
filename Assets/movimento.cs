using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class movimento : MonoBehaviourPun, IPunObservable
{
    public static int velocita;
    public float rotazione, ricarica;
    //public float carburante;
    public Text nickNameT;
    //RectTransform barraCarburante;
    bool accendi;
    public ParticleSystem fuoco;
    public SpriteRenderer minimapIcon;
    Camera cam;
    Rigidbody2D rb;
    RectTransform areaMov, pomelloMov;
    float raggio;
    Vector2 direzione;
    public static bool puoiMuovere = true;

    void Start()
    {
        variabili.players.Add(gameObject);
        cam = Camera.main;
        raggio = Screen.height * 150 / 1440;
        rb = GetComponent<Rigidbody2D>();
        nickNameT.text = !string.IsNullOrEmpty(photonView.Owner.NickName) ? photonView.Owner.NickName : PhotonNetwork.LocalPlayer.UserId.ToString();

        if (photonView.IsMine)
        {
            velocita = upgrade.UpgradeData.Get(upgrade.upgrades.speed, 0).valoreCorr;
            GetComponent<SpriteRenderer>().sprite = menù.skin;
            areaMov = variabili.areaMov;
            pomelloMov = variabili.pomelloMov;
            //barraCarburante = variabili.barraCarburante;
            minimapIcon.color = Color.blue;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = menù.skinCasuale();
            GetComponent<AudioSource>().enabled = false;
        }
    }

    void Update()
    {
        if (!puoiMuovere)
        {
            direzione = Vector2.zero;
            return;
        }
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            areaMov.position = Input.mousePosition;
            pomelloMov.position = areaMov.position;
            pomelloMov.gameObject.SetActive(true);
            areaMov.gameObject.SetActive(true);
        }
        else if (Input.GetMouseButton(0))
        {
            pomelloMov.position = Input.mousePosition;
            Vector2 dist = pomelloMov.position - areaMov.position;
            if (dist.sqrMagnitude > 10000)
                areaMov.position = new Vector2(Mathf.Clamp(areaMov.position.x, pomelloMov.position.x - raggio, pomelloMov.position.x + raggio), Mathf.Clamp(areaMov.position.y, pomelloMov.position.y - raggio, pomelloMov.position.y + raggio));
            direzione = dist;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            pomelloMov.gameObject.SetActive(false);
            areaMov.gameObject.SetActive(false);
            direzione = Vector2.zero;
        }
#else
        if (Input.touchCount > 0)
        {
            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Began:
                    areaMov.position = Input.GetTouch(0).position;
                    pomelloMov.position = areaMov.position;
                    pomelloMov.gameObject.SetActive(true);
                    areaMov.gameObject.SetActive(true);
                    break;
                case TouchPhase.Moved:
                    pomelloMov.position = Input.GetTouch(0).position;
                    Vector2 dist = pomelloMov.position - areaMov.position;
                    if (dist.sqrMagnitude > 10000)
                        areaMov.position = new Vector2(Mathf.Clamp(areaMov.position.x, pomelloMov.position.x - raggio, pomelloMov.position.x + raggio), Mathf.Clamp(areaMov.position.y, pomelloMov.position.y - raggio, pomelloMov.position.y + raggio));
                    direzione = dist;
                    break;
                case TouchPhase.Ended:
                    pomelloMov.gameObject.SetActive(false);
                    areaMov.gameObject.SetActive(false);
                    direzione = Vector2.zero;
                    break;
            }
        }
#endif
        if (photonView.IsMine)
        {
            cam.transform.position = transform.position;
            //barraCarburante.localScale = new Vector3(carburante / 25, 1, 1);
            
            if (direzione.y != 0 && direzione.x != 0)
            {
                /*if (carburante > 0)
                {
                    carburante -= Time.deltaTime * ricarica;
                }
                else
                {
                    carburante = 0;
                }

                if (carburante > 0)
                {*/
                    if (!accendi)
                    {
                        accendi = true;
                    }
                /*}
                else
                {
                    if (accendi)
                    {
                        accendi = false;
                    }
                }*/
            }
            else
            {
                /*if (carburante < 25)
                {
                    carburante += Time.deltaTime * ricarica * 3;
                }
                */
                if (accendi)
                {
                    accendi = false;
                }
            }
        }

        if (accendi)
        {
            if (!fuoco.isPlaying)
            {
                fuoco.Play();
                fuoco.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            }
        }
        else
        {
            if (fuoco.isPlaying)
            {
                fuoco.Stop();
                fuoco.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
            }
        }

        nickNameT.transform.parent.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            Vector2 vettore;
            if (direzione.y > 0)
            {
                vettore = direzione;
                transform.eulerAngles = new Vector3(0, 0, (Mathf.Acos(vettore.x / vettore.magnitude) * Mathf.Rad2Deg) + 270);
            }
            else
            {
                if (direzione.y < 0)
                {
                    vettore = direzione * -1;
                    transform.eulerAngles = new Vector3(0, 0, (Mathf.Acos(vettore.x / vettore.magnitude) * Mathf.Rad2Deg) + 90);
                }
            }

            if (direzione.y != 0 && direzione.x != 0)// && carburante > 0)
            {
                rb.AddRelativeForce(Vector2.up * velocita);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "warp zone")
        {
            variabili.upgradeBt.gameObject.SetActive(true);
            variabili.warpBt.gameObject.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "warp zone")
        {
            variabili.upgradeBt.gameObject.SetActive(true);
            variabili.warpBt.gameObject.SetActive(false);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(accendi);
        }
        else
        {
            accendi = (bool)stream.ReceiveNext();
        }
    }
}