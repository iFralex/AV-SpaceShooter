using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutorial : MonoBehaviour
{
    public RectTransform joystick, carb, vit;
    public ParticleSystem fuoco;
    public Text[] testi;
    int stato;
    public float carburante = 10, vita = 10, danno = 1.5f;
    public AudioClip suonoColpito, suonoSparo;
    public GameObject asteroide, proiettile;
    bool spara;

    void Start()
    {
        Application.targetFrameRate = 60;
        GetComponent<SpriteRenderer>().sprite = menù.skin;
    }

    public void Update()
    {
        Camera.main.gameObject.transform.position = transform.position;
        carb.localScale = new Vector3(carburante / 10, 1, 1);
        vit.localScale = new Vector3(vita / 10, 1, 1);
        if (joystick.GetComponent<FixedJoystick>().Vertical != 0)
        {
            if (carburante > 0)
            {
                carburante -= Time.deltaTime * 1.5f;
                if (!fuoco.isPlaying)
                {
                    fuoco.Play();
                }
            }
            else
            {
                carburante = 0;
                if (fuoco.isPlaying)
                {
                    fuoco.Stop();
                }
            }
        }
        else
        {
            if (fuoco.isPlaying)
            {
                fuoco.Stop();
            }
            if (carburante < 10)
            {
                carburante += Time.deltaTime * 1.5f;
            }
        }
    }

    private void FixedUpdate()
    {
        //GetComponent<Rigidbody2D>().AddTorque(-joystick.GetComponent<FixedJoystick>().Horizontal / 2);
        //if (joystick.GetComponent<FixedJoystick>().Vertical > 0)
        //{
        
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

        if (joystick.GetComponent<FixedJoystick>().Vertical != 0 && joystick.GetComponent<FixedJoystick>().Horizontal != 0 && carburante > 0)
        {
            GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * 7);
        }
        //}
    }

    public void Continua()
    {
        stato++;
        if (stato >= 4)
        {
            testi[0].transform.parent.gameObject.SetActive(false);
            testi[0].transform.parent.parent.GetChild(3).gameObject.SetActive(false);
            testi[0].transform.parent.parent.GetChild(testi[0].transform.parent.parent.childCount - 2).GetComponent<Button>().enabled = true;
            testi[0].transform.parent.parent.GetChild(testi[0].transform.parent.parent.childCount - 2).GetComponent<Image>().enabled = true;
            testi[0].transform.parent.parent.GetChild(testi[0].transform.parent.parent.childCount - 2).SetAsFirstSibling();
            StartCoroutine(SpownAst());
        }
        else
        {
            testi[stato].gameObject.SetActive(true);
            testi[stato - 1].gameObject.SetActive(false);
            testi[0].transform.parent.parent.GetChild(4).SetAsFirstSibling();
            testi[0].transform.parent.parent.GetChild(stato).SetSiblingIndex(4);
        }
    }

    public virtual void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "ostacolo")
        {
            GetComponent<AudioSource>().clip = suonoColpito;
            GetComponent<AudioSource>().Play();
            if (vita > 0)
            {
                float d = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.magnitude * danno);
                if (vita - d >= 0)
                {
                    vita -= Mathf.Abs(GetComponent<Rigidbody2D>().velocity.magnitude * danno);
                }
                else
                {
                    vita = 0;
                }

                if (vita == 0)
                {
                    Fine();
                }
            }
        }
    }

    IEnumerator SpownAst()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(3);
            float n = Random.Range(10, 40);
            GameObject a = Instantiate(asteroide, new Vector2((Mathf.Cos(transform.eulerAngles.z) * 20 * Mathf.Rad2Deg) + transform.position.x, (Mathf.Sin(transform.eulerAngles.z) * 20 * Mathf.Rad2Deg) + transform.position.y), Quaternion.identity);
            a.transform.localScale = new Vector2(n, n);
        }
    }

    public void Fine()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void Spara()
    {
        GameObject a = Instantiate(proiettile, transform.Find("cannone").position, transform.rotation);
        GetComponent<AudioSource>().clip = suonoSparo;
        GetComponent<AudioSource>().Play();
        StartCoroutine(AspettaSpara());
    }

    public IEnumerator AspettaSpara()
    {
        yield return new WaitForSeconds(.5f);
        spara = true;
    }
}