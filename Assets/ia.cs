using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ia : MonoBehaviour
{
    public float velocità, rotazione, carburante, ricarica, vita = 100;
    public GameObject cannone, proiettile;
    bool spara = true, accendi;
    public AudioClip suonoSparo, suonoColpito;
    public ParticleSystem fuoco;
    public enum Direzioni { su, giù, destra, sinistra, suDestra, suSinistra, giùDestra, giùSinistra, suSuDestra, suDestraDestra, giùGiùDestra, giùDestraDestra, suSuSinistra, suSinistraSinistra, giùGiùSinistra, giùSinistraSinistra, fermo }
    public Direzioni stato;
    public Transform target, proiet;
    public gameManagerOffline gm;
    List<Direzioni> dirProib = new List<Direzioni>(0);
    float cambiamento;
    bool fermo;

    public Direzioni SceltaMigliore(bool normaleMovimento = false)
    {
        List<float> _punti = new List<float>();
        for (int i = 0; i < (int)Direzioni.fermo; i++)
            _punti.Add(10);

        if (normaleMovimento)
        {
            _punti[(int)stato] += .9f - cambiamento;
            List<Direzioni> vicine = DirezioniVicine(stato);
            foreach (Direzioni dir in vicine)
                _punti[(int)dir] += .3f;
        }
        for (int i = 0; i < _punti.Count; i++)
        {
            if (dirProib.Contains((Direzioni)i))
            {
                _punti[i] -= 50;
                continue;
            }
            Vector2 direz = vettoreDirezione((Direzioni)i);
            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, direz, 10f);

            for (int o = 0; o < hit.Length; o++)
            {
                if (hit[o].collider.gameObject == gameObject || hit[o].collider.gameObject == transform.GetChild(2).gameObject)
                {
                    //print("ops");
                    continue;
                }

                if (hit[o].collider != null)
                {
                    //print(direz + hit[o].collider.tag + "   " + hit[o].collider.name + hit[o].point);
                    if (hit[o].collider.gameObject.tag == "Player")
                    {
                        if (Vector2.Distance(hit[o].point, transform.position) > 5)
                        {
                            _punti[i] += 5;
                            Spara(hit[o].collider.transform);
                        }
                        else
                            _punti[i] -= 5;
                    }
                    if (hit[o].collider.gameObject.tag == "ostacolo")
                        _punti[i] -= 4f;
                    if (hit[o].collider.gameObject.name == "lliderer terreno")
                        _punti[i] -= 1;
                    if (new Vector2((int)hit[o].point.x, (int)hit[o].point.y) != new Vector2((int)transform.position.x, (int)transform.position.y))
                        _punti[i] -= 3f / Vector2.Distance(hit[o].point, transform.position);
                    else
                        _punti[i] -= 50;
                    //Debug.DrawRay(transform.position, direz, Color.white, 100);
                }
            }
            //print(_punti[i]);
            if (_punti[i] > 10)
                break;
        }

        //foreach (float f in _punti)
        //print(f);
        //print(NumeroPiùGrande(_punti));
        Direzioni risult = NumeroPiùGrande(_punti);
        if (risult == stato)
            cambiamento += .01f;
        else
            cambiamento = 0;
        //print("chiamato000");
        return risult;
    }

    Vector2 vettoreDirezione(Direzioni dir)
    {
        switch (dir)
        {
            case Direzioni.su:
                return Vector2.up;
            case Direzioni.giù:
                return Vector2.down;
            case Direzioni.sinistra:
                return Vector2.left;
            case Direzioni.destra:
                return Vector2.right;
            case Direzioni.suDestra:
                return Vector2.up + Vector2.right;
            case Direzioni.suSinistra:
                return Vector2.up + Vector2.left;
            case Direzioni.giùDestra:
                return Vector2.down + Vector2.right;
            case Direzioni.giùSinistra:
                return Vector2.down + Vector2.left;
            case Direzioni.suSuDestra:
                return Vector2.up + Vector2.up + Vector2.right;
            case Direzioni.suDestraDestra:
                return Vector2.up + Vector2.right + Vector2.right;
            case Direzioni.giùGiùDestra:
                return Vector2.down + Vector2.down + Vector2.right;
            case Direzioni.giùDestraDestra:
                return Vector2.down + Vector2.right + Vector2.right;
            case Direzioni.suSuSinistra:
                return Vector2.up + Vector2.up + Vector2.left;
            case Direzioni.suSinistraSinistra:
                return Vector2.up + Vector2.left + Vector2.left;
            case Direzioni.giùGiùSinistra:
                return Vector2.down + Vector2.left + Vector2.down;
            case Direzioni.giùSinistraSinistra:
                return Vector2.down + Vector2.left + Vector2.left;
        }
        return Vector2.zero;
    }

    public Direzioni NumeroPiùGrande(List<float> lista)
    {
        float num = -100;
        for (int i = 0; i < lista.Count - 1; i++)
            if (num < lista[i] || num < lista[i + 1])
            {
                if (lista[i] > lista[i + 1])
                    num = lista[i];
                else
                    num = lista[i + 1];
            }
        if (num == -100)
            return (Direzioni)Random.Range(0, (int)Direzioni.fermo);
        return (Direzioni)lista.IndexOf(num);
    }

    Direzioni DirezioneDaAngolo(float angolo)
    {
        angolo = AngoloApprossimato(angolo);
        switch (angolo)
        {
            case 0:
                return Direzioni.destra;
            case 45:
                return Direzioni.suDestra;
            case 90:
                return Direzioni.su;
            case 135:
                return Direzioni.suSinistra;
            case 180:
                return Direzioni.sinistra;
            case -45:
                return Direzioni.giùDestra;
            case -90:
                return Direzioni.giù;
            case -135:
                return Direzioni.giùSinistra;
            case -180:
                return Direzioni.sinistra;
            case 23:
                return Direzioni.suDestraDestra;
            case 68:
                return Direzioni.suSuDestra;
            case 113:
                return Direzioni.suSuSinistra;
            case 158:
                return Direzioni.suSinistraSinistra;
            case -23:
                return Direzioni.giùDestraDestra;
            case -68:
                return Direzioni.giùGiùDestra;
            case -113:
                return Direzioni.giùGiùSinistra;
            case -158:
                return Direzioni.giùSinistraSinistra;
        }
        return Direzioni.su;
    }

    float AngoloDaVettore(Vector2 vettore)
    {
        return Mathf.Acos(vettore.x / vettore.magnitude) * Mathf.Rad2Deg;
    }

    List<Direzioni> DirezioniVicine(Direzioni principale)
    {
        float angolo = AngoloDaVettore(vettoreDirezione(principale));
        //print("direzioni consigliate: " + DirezioneDaAngolo(angolo + 46) + "  e  " + DirezioneDaAngolo(angolo - 46));
        return new List<Direzioni>() { DirezioneDaAngolo(angolo - 22), DirezioneDaAngolo(angolo + 23) };
    }

    int AngoloApprossimato(float angolo)
    {
        float a = angolo;
        angolo = (int)Mathf.Round(angolo / 22.5f);
        for (; angolo > 8;)
            if (angolo > 0)
                angolo -= 16;
        for (; angolo < -8;)
            if (angolo < 0)
                angolo += 16;
        return (int)Mathf.Round(angolo * 22.5f);
    }

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = menù.skinCasuale();
        GetComponentInChildren<Text>().text = "Player 1";
        StartCoroutine(ControllaProiettili());
        StartCoroutine(Movimento());
    }

    private void FixedUpdate()
    {
        if (!fermo && carburante > 0)
        {
            GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * 7);
        }
    }

    IEnumerator ControllaProiettili()
    {
        for (; ; )
        {
            yield return null;// new WaitForSeconds(.5f);
            //for (int i1 = 0; i1 < 10; i1++)
            //{
            if (carburante > 0)
                if (gm.proiettili.Count > 0)
                {
                    List<proiettileOffline> listaPr = new List<proiettileOffline>(0);
                    foreach (proiettileOffline p in gm.proiettili)
                        listaPr.Add(p);
                    float m = -1 / ((transform.position.y - target.position.y) / (transform.position.x - target.position.x));//Mathf.Tan(Mathf.Acos(vettoreDivisivo.x / vettoreDivisivo.magnitude));
                    float q = transform.position.y - (transform.position.x * m);
                    int verso = 0;
                    if (target.position.y > target.position.x * m + q)
                    {
                        verso = 1;
                    }
                    else
                        verso = -1;
                    float angolo = Mathf.Atan(m);
                    //Debug.DrawRay(new Vector2(Mathf.Cos(angolo) * -10, Mathf.Sin(angolo) * -10) + (Vector2)transform.position, new Vector2(Mathf.Cos(angolo) * 100, Mathf.Sin(angolo) * 100) + (Vector2)transform.position, Color.white, .5f);

                    for (int i = listaPr.Count - 1; i >= 0; i--)
                    {
                        float distanza = Vector2.Distance(listaPr[i].transform.position, transform.position);
                        if (distanza > 20)
                        {
                            //print("distanza: " + distanza);
                            listaPr.RemoveAt(i);
                            continue;
                        }

                        if (listaPr[i].transform.position.y * verso < (listaPr[i].transform.position.x * m + q) * verso)
                        {
                            //print("no pericolo");
                            listaPr.RemoveAt(i);
                            continue;
                        }

                        float angoloProiet = (listaPr[i].transform.eulerAngles.z + 90) * Mathf.Deg2Rad;
                        Vector2 punto = new Vector2(Mathf.Cos(angoloProiet) * distanza, Mathf.Sin(angoloProiet) * distanza) + (Vector2)listaPr[i].transform.position;
                        punto = new Vector2((int)(punto.x / 1), (int)(punto.y / 1f));
                        if (punto == new Vector2((int)transform.position.x, (int)transform.position.y))
                        {
                            if (angolo > 0)
                                angolo -= 180;
                            else
                                angolo += 180;
                            //dirProib.Add(DirezioneDaAngolo(angolo * -1));
                            //print(DirezioneDaAngolo(angolo * -1) + ": " + angolo * -1);
                            Muoviti();
                        }
                    }
                }
            // }
        }
    }

    IEnumerator Movimento()
    {
        for (; ; )
        {
            if (carburante > 0)
                Muoviti();
            else
            {
                fermo = true;
                fuoco.Stop();
                int n = Random.Range(1, 100);
                for (int i = 0; i < n; i++)
                {
                    carburante += Time.deltaTime * 3.5f;
                    yield return null;
                    if (carburante > 10)
                        break;
                }
                fermo = false;
                fuoco.Play();
            }
            yield return null;// new WaitForSeconds(.2f);
        }
    }

    public void Movimento(Direzioni dir)
    {
        Vector2 direz = vettoreDirezione(dir);
        GetComponent<Rigidbody2D>().AddForce(direz * velocità, ForceMode2D.Impulse);
        print(direz + "   " + velocità + dir);
    }

    public void Muoviti()
    {
        Direzioni d = SceltaMigliore(true);
        stato = d;
        Vector2 vettore = vettoreDirezione(stato);
        float angolo = AngoloDaVettore(vettore);
        if (vettore.y < 0)
            angolo *= -1;
        //print(d + ":  " + angolo + " | " + vettore);
        GetComponent<Rigidbody2D>().SetRotation(angolo - 90);
        carburante -= Time.deltaTime * 3.5f;
    }

    void Spara(Transform player)
    {
        if (spara)
        { 
            spara = false;
            GameObject a = Instantiate(proiettile, cannone.transform.position, transform.rotation);
            if (GetComponent<SpriteRenderer>().isVisible)
            {
                GetComponent<AudioSource>().clip = suonoSparo;
                GetComponent<AudioSource>().Play();
            }
            gm.proiettili.Add(a.GetComponent<proiettileOffline>());
            StartCoroutine(AspettaSpara());
        }
    }

    IEnumerator AspettaSpara()
    {
        yield return new WaitForSeconds(.5f);
        spara = true;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "ostacolo")
        {
            if (GetComponent<SpriteRenderer>().isVisible)
            {
                GetComponent<AudioSource>().clip = suonoColpito;
                GetComponent<AudioSource>().Play();
            }
            float d = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.magnitude) * 0.3f;
            if (vita - d >= 0)
                vita -= d;
            else
                Destroy(gameObject);
        }
    }
}