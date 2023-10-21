using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class movimentoOffline : MonoBehaviour
{
    public float velocità, rotazione, carburante, ricarica;
    public GameObject cannone, proiettile;
    public Text timer;
    public Image barra, barraVita;
    public float vita
    {
        get { return _vita; }
        set
        {
            if (value != _vita)
            {
                _vita = value;
                barraVita.rectTransform.localScale = new Vector3(value / 10, 1, 1);
                if (value <= 0)
                    SceneManager.LoadScene(0);
            }
        }
    }
    float secondi, minuti, _vita;
    bool spara = true, accendi;
    public AudioClip suonoSparo, suonoColpito;
    public ParticleSystem fuoco;
    public FixedJoystick joystick;
    public gameManagerOffline gM;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = menù.skin;
        vita = 10;
    }
    
    void Update()
    {
        Camera.main.transform.position = transform.position;
        barra.rectTransform.localScale = new Vector3(carburante / 10, 1, 1);
        secondi += Time.deltaTime;
        if (secondi > 60)
        {
            secondi = 0;
            minuti++;
        }

        if (secondi < 10)
            timer.text = minuti.ToString() + ":0" + ((int)secondi).ToString();
        else
            timer.text = minuti.ToString() + ":" + ((int)secondi).ToString();

        if (Input.GetKey(KeyCode.W) || (joystick.Vertical != 0 && joystick.Horizontal != 0))
        {
            if (carburante > 0)
                carburante -= Time.deltaTime * ricarica;
            else
                carburante = 0;

            if (carburante > 0)
            {
                if (!accendi)
                    accendi = true;
            }
            else if (accendi)
                    accendi = false;
        }
        else
        {
            if (carburante < 10)
                carburante += Time.deltaTime * ricarica;

            if (accendi)
                accendi = false;
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

        GetComponentInChildren<Canvas>().transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
            GetComponent<Rigidbody2D>().AddTorque(rotazione);

        if (Input.GetKey(KeyCode.D))
            GetComponent<Rigidbody2D>().AddTorque(-rotazione);
        Vector2 vettore = Vector2.zero;

        if (joystick.transform.GetChild(0).position.y > joystick.transform.position.y)
        {
            vettore = joystick.transform.GetChild(0).position - joystick.transform.position;
            transform.eulerAngles = new Vector3(0, 0, (Mathf.Acos(vettore.x / vettore.magnitude) * Mathf.Rad2Deg) + 270);
        }
        else
        {
            if (joystick.transform.GetChild(0).position.y < joystick.transform.position.y)
            {
                vettore = joystick.transform.position - joystick.transform.GetChild(0).position;
                transform.eulerAngles = new Vector3(0, 0, (Mathf.Acos(vettore.x / vettore.magnitude) * Mathf.Rad2Deg) + 90);
            }
        }
        if (joystick.Vertical != 0 && joystick.Horizontal != 0 && carburante > 0)
            GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * 7);
    }

    IEnumerator AspettaSpara()
    {
        yield return new WaitForSeconds(.5f);
        spara = true;
    }

    public void Esci()
    {
        SceneManager.LoadScene(0);
    }

    public void Spara()
    {
        if (spara)
        {
            GameObject a = Instantiate(proiettile, cannone.transform.position, transform.rotation);
            spara = false;
            GetComponent<AudioSource>().clip = suonoSparo;
            GetComponent<AudioSource>().Play();
            a.GetComponent<proiettileOffline>().proprietario = this;
            gM.proiettili.Add(a.GetComponent<proiettileOffline>());
            StartCoroutine(AspettaSpara());
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "ostacolo")
        {
            GetComponent<AudioSource>().clip = suonoColpito;
            GetComponent<AudioSource>().Play();
            float d = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.magnitude) * 0.3f;
            if (vita - d >= 0)
                vita -= d;
            else
                vita = 0;
        }
    }
}