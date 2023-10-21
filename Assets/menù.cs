using UnityEngine;
using UnityEngine.UI;
//using GoogleMobileAds.Api;
//using GoogleMobileAds.Placement;
using Firebase.Auth;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;

public class menù : MonoBehaviour
{
    public enum scelta { tutorial, game }
    public Slider[] slider;
    public Slider numAsteroidiBatOfflSli, numPlayersBatOfflSli;
    public Text[] valore;
    public UnityEngine.Audio.AudioMixer mainMixer;
    public Text errore;
    //BannerAdGameObject bannerAd;
    //InterstitialAdGameObject interstitialAd;
    public FirebaseAuth auth;
    public FirebaseUser user;
    public Text nomeAccount, nomeAccountRiq;
    public GameObject loginPan, accountPan, registraPan, messaggio, listaClassifica, battagliaOfflinePan, mappaSistemaSolarePan;
    public enum Sfide { battagliaOnline, battagliaOffline, corsaATempo, tutorial}
    public List<SfideUI> sfide;
    public Sfide sfidaSel;
    public static Sprite skin;
    public InputField numPlayersBatOffl, numAsteroidiBatOffl;
    public shopManager shop;

    [Header("Punti")]
    public Text puntiT;
    static int _punti;
    public static int punti
    {
        get { return _punti; }
        set
        {
            if (value != _punti)
            {
                if (FirebaseAuth.DefaultInstance.CurrentUser != null)
                {
                    print("ll");
                    _punti = value;
                    classifica.Aggiorna(value, "Punti");
                    PlayerPrefs.SetInt("punti", value);
                }
                else
                    _punti = value = 0;
            }
            PlayerPrefs.SetInt("punti", value);
            Text[] testi = FindObjectsOfType<Text>();
            foreach (Text t in testi)
                if (t.gameObject.name == "trofei t")
                    t.text = value.ToString();
        }
    }

    [Header("Scelta sfida")]
    public Text titoloSfidaT;
    public Text descSfidaT;
    public Text nomeSfidaInMenuT;
    public RectTransform immaginiSfida;

    public static Sprite skinCasuale()
    {
        return shopManager.listaSkin[Random.Range(0, shopManager.listaSkin.Count)];
    }
    //public InputField aaa;
    void Start()
    {
        /*string s = "";
        for (int i = 0; i < aaa.text.Length; i++)
        {
            if ("" + aaa.text[i] == " ")
                s += "\n";
            else
                s += aaa.text[i] + "";
        }
        int o = 0;
        string ss = "";
        string ss1 = "";
        for (int i = 0; i < s.Length; i++)
        {
            if (o % 2 != 0)
                ss += s[i] + "";
            else
                ss1 += s[i] + "";
            if ("" + s[i] == "\n")
                o += 1;
        }
        File.WriteAllText("/Users/alessioantonucci/Downloads/FirstNames", ss);
        File.WriteAllText("/Users/alessioantonucci/Downloads/LastNames", ss1);
        string email = "giochiperciechi";
        s = "";
        for (int i = 0; i < 10000; i++)
        {
            s += "\n" + email + "+" + i.ToString() + "@gmail.com";
        }
        File.WriteAllText("/Users/alessioantonucci/Downloads/Emails", s);
        */skin = shop.listaArticoli[0].skin;
        for (int i = 0; i < shop.listaArticoli.Count; i++)
            shopManager.listaSkin.Add(shop.listaArticoli[i].skin);
        InitializeFirebase();
        Button[] bots = FindObjectsOfType<Button>();
        for (int i = 0; i < bots.Length; i++)
            if (bots[i].name == "Start")
                bots[i].onClick.AddListener(() => mostraAd(1));
            else
            {
                if (bots[i].name == "Tutorial")
                    bots[i].onClick.AddListener(() => mostraAd(3));
            }
        if (!PlayerPrefs.HasKey("volume musica"))
        {
            PlayerPrefs.SetFloat("volume musica", 1f);
            PlayerPrefs.SetFloat("volume suoni", 1f);
        }
        if (!PlayerPrefs.HasKey("punti"))
            PlayerPrefs.SetInt("punti", 0);
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
            _punti = PlayerPrefs.GetInt("punti");
        else
            _punti = 0;
        punti = _punti;
        slider[0].value = PlayerPrefs.GetFloat("volume musica");
        slider[1].value = PlayerPrefs.GetFloat("volume suoni");
        CambiaVolume1();
        CambiaVolume2();
        //MobileAds.Initialize(initStatus => { });
        //bannerAd = MobileAds.Instance.GetAd<BannerAdGameObject>("banner ad");
        //bannerAd.LoadAd();
        //interstitialAd = MobileAds.Instance.GetAd<InterstitialAdGameObject>("interstitial ad");
        classifica.CreaClassifica("Punti", listaClassifica.GetComponent<RectTransform>());
        //this.RequestBanner();
        //RequestInterstitial();
    }

    public void CambiaVolume1()
    {
        valore[0].text = ((int)(slider[0].value * 10)).ToString();
        mainMixer.SetFloat("musica", Mathf.Log10(slider[0].value + 0.0001f) * 20);
        PlayerPrefs.DeleteKey("volume musica");
        PlayerPrefs.SetFloat("volume musica", slider[0].value);
    }

    public void CambiaVolume2()
    {
        valore[1].text = ((int)(slider[1].value * 10)).ToString();
        mainMixer.SetFloat("suoni", Mathf.Log10(slider[1].value + 0.0001f) * 20);
        PlayerPrefs.DeleteKey("volume suoni");
        PlayerPrefs.SetFloat("volume suoni", slider[1].value);
    }

    public void CambiaScena()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void ConnettitiADiscord()
    {
        Application.OpenURL("https://discord.gg/Jyyb7RgVb2");
    }

    public void ContattaiFralex()
    {
        Application.OpenURL("mailto:ifralex.developer@gmail.com");
    }
    int scena;
    public void mostraAd(int a)
    {
        scena = a;
        //interstitialAd.ShowIfLoaded();
    }

    public void Inizia()
    {
        if (scena == 1)
        {
            
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("tutorial");
        }
    }

    public void ApriScena(int i) => UnityEngine.SceneManagement.SceneManager.LoadScene(i);

    public void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    public void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        ////print("d");
        if (auth.CurrentUser != user)
        {
            if (user != null)
            {
                Loggato(false);
                Debug.Log("Signed out " + user.UserId);
            }
            if (user == null && auth.CurrentUser != null)
            {
                //print("ss");
                Loggato(true);
            }
            user = auth.CurrentUser;
        }
        else if (auth.CurrentUser == null)
        {
            Loggato(false);
        }
        else if (auth.CurrentUser != null)
        {
            Loggato(true);
        }
    }

    public void Loggato(bool b, string s = "")
    {
        if (s == "" && auth.CurrentUser != null)
            s = auth.CurrentUser.DisplayName;
            
        ////print("ci");
        if (b)
        {
            accountPan.SetActive(true);
            loginPan.SetActive(false);
            registraPan.SetActive(false);
            nomeAccount.text = nomeAccountRiq.text = s;
        }
        else
        {
            nomeAccount.text = nomeAccountRiq.text = "Not logged in";
            accountPan.SetActive(false);
            loginPan.SetActive(true);
            registraPan.SetActive(false);
            punti = 0;
        }
    }

    public void LogOut()
    {
        auth.SignOut();
    }

    public void SelezionaSfida(int sel)
    {
        SfideUI sf = null;
        sfidaSel = (Sfide)sel;
        for (int i = 0; i < sfide.Count; i++)
            if (sfide[i].sfida == sfidaSel)
                sf = sfide[i];
        titoloSfidaT.text = nomeSfidaInMenuT.text = sf.titolo;
        descSfidaT.text = sf.descrizione;
        for (int i = 0; i < immaginiSfida.childCount; i++)
            immaginiSfida.GetChild(i).GetComponent<Image>().sprite = sf.immagini[i];
    }

    public void MostraSfide() => SelezionaSfida((int)sfidaSel);

    public void IniziaSfida()
    {
        Debug.Log(sfidaSel);
        switch (sfidaSel)
        {
            case Sfide.battagliaOnline:
                mappaSistemaSolarePan.SetActive(true);
                break;
            case Sfide.battagliaOffline:
                battagliaOfflinePan.SetActive(true);
                break;
            case Sfide.corsaATempo:
                SceneManager.LoadScene(4);
                break;
            case Sfide.tutorial:
                SceneManager.LoadScene(5);
                break;
        }
    }

    public void NumeroPlayersBattagliaOffline(float valore) => numPlayersBatOffl.text = valore.ToString();

    public void NumeroPlayersBattagliaOfflineCt(string valore)
    {
        if (valore == "")
            valore = 2.ToString();
        int a = Mathf.Max(System.Convert.ToInt32(valore), 2);
        numPlayersBatOffl.text = Mathf.Min(a, 7).ToString();
        ModificaSliderNumeroPlayer(numPlayersBatOffl.text);
    }

    public void ModificaSliderNumeroPlayer(string valore) => numPlayersBatOfflSli.value = System.Convert.ToInt32(valore);

    public void NumeroAsteroidiBattagliaOffline(float valore) => numAsteroidiBatOffl.text = valore.ToString();

    public void NumeroAsteroidiBattagliaOfflineCt(string valore)
    {
        if (valore == "")
            valore = 1.ToString();
        int a = Mathf.Max(System.Convert.ToInt32(valore), 1);
        numAsteroidiBatOffl.text = Mathf.Min(a, 9).ToString();
        ModificaSliderNumeroAsteroidi(numAsteroidiBatOffl.text);
    }

    public void ModificaSliderNumeroAsteroidi(string valore) => numAsteroidiBatOfflSli.value = System.Convert.ToInt32(valore);

    public void IniziaBattagliaOffline()
    {
        partitaManager.numPlayers = System.Convert.ToInt32(numPlayersBatOffl.text);
        partitaManager.numero = System.Convert.ToInt32(numAsteroidiBatOffl.text);
        ApriScena(3);
    }
}

[System.Serializable]
public class SfideUI
{
    public string titolo, descrizione;
    public menù.Sfide sfida;
    public Sprite[] immagini;
}