using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class mappaManager : MonoBehaviourPunCallbacks
{
    public GameObject sistSolPref;

    [Header("Sistema Solare Selezionato")]
    public Text nomeSistT;
    public Text numPlaSistT;
    public RectTransform numPlaSistRT;
    public Button joinSistBt;

    [Header("Crea Sistema Solare")]
    public InputField nomeSistemaIF;
    public Slider numMaxSistSl;
    public Text numMaxSistT;
    public Toggle sistApertoTo;
    public Button creaSistemaBt;

    [Header("Cerca sistema")]
    public Button cercaSistemaBt;
    public InputField nomeCercaSistemaIF;

    [Header("Pannelli")]
    public RectTransform sistemiRT;
    public GameObject sistemiGM;
    public GameObject sistemaSelGM;
    public RectTransform creaSistemaRT;
    public GameObject cercaSistemaGM;
    public GameObject pulsantiGM;
    public GameObject loading;

    [Header("Posiziona sistemi solari")]
    int numSistemi;
    float diametri;
    List<float> radiantiPerCirconferenza = new List<float>(), diametroIniz = new List<float>();
    List<int> partiOccupate = new List<int>(), circonferenze = new List<int>();
    List<string> roomListPrec = new List<string>();
    [HideInInspector] public bool appenaAperto;

    [Header("Zoom e Scrooling")]
    public RectTransform scrollRectRT;
    public float _minimumScale = 0.5f;
    public float _maximumScale = 3f;
    ScrollRect scrollRectSR;
    float dist1;
    float _scaleIncrement;
    bool scrollabile = true;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (roomList.Count != 2)
        {
            if (roomList.Count == 1)
            {
                if (roomList[0].Name == "ss1")
                    roomList.Add(new RoomInfo() { Name = "ss2", PlayerCount = 16 });
                else
                    roomList.Add(new RoomInfo() { Name = "ss1", PlayerCount = 16 });
            }
            else
                roomList = new List<RoomInfo> { new RoomInfo() { Name = "ss1", PlayerCount = 16 }, new RoomInfo() { Name = "ss2", PlayerCount = 16 } };
        }
        if (appenaAperto)
        {
            numSistemi = roomList.Count;
            sistemiGM.SetActive(true);
            //pulsantiGM.SetActive(true);
            diametri = sistSolPref.GetComponent<RectTransform>().sizeDelta.x * 5f / 3f;
            float diam = diametri + (diametri / 2);
            for (int i = 0; i < numSistemi;)
            {
                int a = NumCirc(diam, diametri);
                circonferenze.Add(a);
                diametroIniz.Add(diam);
                radiantiPerCirconferenza.Add((diam * Mathf.PI / a) * Mathf.PI * 2 / (diam * Mathf.PI));
                diam += diametri * 2;
                i += a;
            }
            float r = diametri / 6f;
            int n = 0;
            if (circonferenze.Count > 1)
                for (int _o = 0; _o < circonferenze.Count - 1; _o++)
                    for (int i = 0; i < circonferenze[_o]; i++)
                    {
                        n++;
                        if (n >= numSistemi)
                            break;
                        RectTransform sistema = Instantiate(sistSolPref, sistemiRT).GetComponent<RectTransform>();
                        sistema.anchoredPosition = new Vector2(Mathf.Cos(radiantiPerCirconferenza[_o] * i) * (diametroIniz[_o] / 2) + Random.Range(-r, r), Mathf.Sin(radiantiPerCirconferenza[_o] * i) * (diametroIniz[_o] / 2) + Random.Range(-r, r));
                        sistema.GetComponent<sistemaSolareIMappa>().mm = this;
                    }
            int o = circonferenze.Count - 1;
            for (int i = 0; i < numSistemi - n; i++)
            {
                int p = Random.Range(0, circonferenze[o]);
                int _mm = NumCas(p, partiOccupate, numSistemi - n);
                float x = radiantiPerCirconferenza[o] * _mm;
                RectTransform sistema = Instantiate(sistSolPref, sistemiRT).GetComponent<RectTransform>();
                sistema.anchoredPosition = new Vector2(Mathf.Cos(x) * (diametroIniz[o] / 2) + Random.Range(-r, r), Mathf.Sin(x) * (diametroIniz[o] / 2) + Random.Range(-r, r));
                sistema.GetComponent<sistemaSolareIMappa>().mm = this;
                partiOccupate.Add(_mm);
            }
            for (int i = 0; i < sistemiRT.childCount; i++)
                sistemiRT.GetChild(i).GetComponent<sistemaSolareIMappa>().roomInfo = roomList[i];
            print(roomList[0].Name);
            float larg = circonferenze.Count * diametri * 2 + diametri * 4;
            sistemiRT.GetComponent<RectTransform>().sizeDelta = new Vector2(larg, larg);
            sistemiGM.GetComponent<ScrollRect>().horizontalNormalizedPosition = .5f;
            sistemiGM.GetComponent<ScrollRect>().verticalNormalizedPosition = .5f;
            loading.SetActive(false);
            appenaAperto = false;
        }
        else if (false)
        {
            List<string> roomRimosse = new List<string>();
            List<string> roomModificate = new List<string>();
            for (int i = 0; i < roomList.Count; i++)
            {
                if (roomList[i].PlayerCount == 0)
                    roomRimosse.Add(roomList[i].Name);
                else if (!roomListPrec.Contains(roomList[i].Name))
                    AggiungiSistema(roomList[i]);
                else
                    roomModificate.Add(roomList[i].Name);
            }

            for (int i = 0; i < sistemiRT.childCount; i++)
            {
                if (roomRimosse.Contains(sistemiRT.GetChild(i).GetComponent<sistemaSolareIMappa>().roomInfo.Name))
                {
                    if (sistemaSelGM.activeInHierarchy && nomeSistT.text == sistemiRT.GetChild(i).GetComponent<sistemaSolareIMappa>().roomInfo.Name)
                        sistemaSelGM.SetActive(false);
                    Destroy(sistemiRT.GetChild(i).gameObject);
                }
                else if (roomModificate.Contains(sistemiRT.GetChild(i).GetComponent<sistemaSolareIMappa>().roomInfo.Name))
                    for (int o = 0; o < roomList.Count; o++)
                        if (roomList[o].Name == sistemiRT.GetChild(i).GetComponent<sistemaSolareIMappa>().roomInfo.Name)
                        {
                            sistemiRT.GetChild(i).GetComponent<sistemaSolareIMappa>().roomInfo = roomList[o];
                            if (sistemaSelGM.activeInHierarchy && nomeSistT.text == roomList[o].Name)
                                SelezionaSistema(roomList[o].Name, roomList[o].PlayerCount, roomList[o].MaxPlayers, sistemiRT.GetChild(i).GetComponent<RectTransform>());
                        }
            }
            
        }
        roomListPrec.Clear();
        for (int i = 0; i < roomList.Count; i++)
            roomListPrec.Add(roomList[i].Name);
    }

    public void AggiungiSistema(RoomInfo roomInfo)
    {
        void AggCirc()
        {
            float diam = diametroIniz.Count > 0 ? diametroIniz[diametroIniz.Count - 1] + diametri * 2 : diametri + (diametri / 2);
            int a = NumCirc(diam, diametri);
            circonferenze.Add(a);
            diametroIniz.Add(diam);
            radiantiPerCirconferenza.Add((diam * Mathf.PI / a) * Mathf.PI * 2 / (diam * Mathf.PI));
            partiOccupate.Clear();
        }
        float r = diametri / 6f;
        if (circonferenze.Count != 0)
        {
            if (NumCas(0, partiOccupate, circonferenze[circonferenze.Count - 1]) == -1)
                AggCirc();
        }
        else
            AggCirc();
        int o = circonferenze.Count - 1;
        int p = Random.Range(0, (int)circonferenze[o]);
        int _mm = NumCas(p, partiOccupate, (int)circonferenze[o]);
        float x = radiantiPerCirconferenza[o] * _mm;
        RectTransform sistema = Instantiate(sistSolPref, sistemiRT).GetComponent<RectTransform>();
        sistema.anchoredPosition = new Vector2(Mathf.Cos(x) * (diametroIniz[o] / 2) + Random.Range(-r, r), Mathf.Sin(x) * (diametroIniz[o] / 2) + Random.Range(-r, r));
        sistema.GetComponent<sistemaSolareIMappa>().mm = this;
        sistema.GetComponent<sistemaSolareIMappa>().roomInfo = roomInfo;
        partiOccupate.Add(_mm);
        float larg = circonferenze.Count * diametri * 2 + diametri * 4;
        sistemiRT.GetComponent<RectTransform>().sizeDelta = new Vector2(larg, larg);
        numSistemi++;
    }

    public int NumCirc(float c, float _diam) => (int)Mathf.Floor(Mathf.PI / Mathf.Asin(_diam / c));

    public int NumCas(int _n, List<int> nOcc, int numMax, int tent = 0)
    {
        if (tent == numMax)
            return -1;
        tent++;
        if (_n == numMax)
            _n = 0;
        if (nOcc.Contains(_n))
            _n = NumCas(_n + 1, nOcc, numMax, tent);
        return _n;
    }

    public void SelezionaSistema(string _nome, int _players, int _maxPlayers, RectTransform sist)
    {
        creaSistemaRT.gameObject.SetActive(false);
        cercaSistemaGM.SetActive(false);
        StartCoroutine(InquadraSistema(_nome, _players, _maxPlayers, sist));
    }

    IEnumerator InquadraSistema(string _nome, int _players, int _maxPlayers, RectTransform sist)
    {
        scrollRectSR.ScrollToCenter(sist, this);
        scrollabile = false;
        for (float i = 0; i < 1.1f; i += .05f)
        {
            scrollRectRT.localScale = Vector3.Lerp(scrollRectRT.localScale, Vector3.one * 3, i);
            yield return new WaitForSeconds(.05f);
        }
        sistemaSelGM.SetActive(true);
        nomeSistT.text = _nome;
        numPlaSistT.text = _players.ToString() + "/" + _maxPlayers.ToString();
        joinSistBt.interactable = _players != _maxPlayers;
        numPlaSistRT.localScale = new Vector3(_players / (float)_maxPlayers, 1, 1);
        joinSistBt.onClick.RemoveAllListeners();
        joinSistBt.onClick.AddListener(() => PhotonNetwork.JoinOrCreateRoom(_nome, new RoomOptions() { MaxPlayers = 16, IsOpen = true, IsVisible = true, CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "posAsterIndex", 0 }, { "scaleAsterIndex", 0 } } }, null));
        joinSistBt.onClick.AddListener(() => loading.SetActive(true));
        scrollabile = true;
        yield return new WaitForSeconds(.5f);
        sistemaSelGM.SetActive(true);
    }

    public void DisattivaTutto()
    {
        sistemiGM.SetActive(false);
        sistemaSelGM.SetActive(false);
        creaSistemaRT.gameObject.SetActive(false);
        cercaSistemaGM.SetActive(false);
        pulsantiGM.SetActive(false);
        loading.SetActive(false);
    }

    public void AttivaCercaSistema()
    {
        sistemaSelGM.SetActive(false);
        creaSistemaRT.gameObject.SetActive(false);
        cercaSistemaGM.SetActive(!cercaSistemaGM.activeInHierarchy);
    }

    public void NomeCercaSistemaIF(string s) => cercaSistemaBt.interactable = s.Length > 6;

    public void CercaSistema()
    {
        bool a = false;
        for (int i = 0; i < sistemiRT.childCount; i++)
            if (sistemiRT.GetChild(i).GetComponent<sistemaSolareIMappa>().roomInfo.Name == nomeCercaSistemaIF.text)
            {
                sistemiRT.GetChild(i).GetComponent<Button>().onClick.Invoke();
                a = true;
                break;
            }
        if (!a)
            PhotonNetwork.JoinRoom(nomeCercaSistemaIF.text);

    }

    public void AttivaCreaSistema()
    {
        cercaSistemaGM.SetActive(false);
        sistemaSelGM.SetActive(false);
        creaSistemaRT.gameObject.SetActive(!creaSistemaRT.gameObject.activeInHierarchy);
    }

    public void RandomNomeSistema()
    {
        string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";
        string str = string.Empty;
        for (int i = 0; i < 20; i++)
            str += glyphs[Random.Range(0, glyphs.Length)];
        nomeSistemaIF.text = str;
    }

    public void NomeSistemaIF(string s) => creaSistemaBt.interactable = s.Length > 6;
    public void MaxPlayersSlider(float n) => numMaxSistT.text = n.ToString();

    public void CreaSistemaSolare()
    {
        loading.SetActive(true);
        PhotonNetwork.CreateRoom(nomeSistemaIF.text, new RoomOptions() { MaxPlayers = (byte)numMaxSistSl.value, IsVisible = !sistApertoTo.isOn, CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "posAsterIndex", Random.Range(0, partitaManager.posAsteroidiList.Count) }, { "scaleAsterIndex", Random.Range(0, partitaManager.scaleAsteroidiList.Count) } } });
    }

    public override void OnCreatedRoom()
    {
        loading.SetActive(true);
        MostraMessaggio.mm.Messaggio(
            MostraMessaggio.Messaggi.avviso,
            "Solar system created.");
    }

    public override void OnJoinedRoom()
    {
        appenaAperto = true;
        /*MostraMessaggio.mm.Messaggio(
            MostraMessaggio.Messaggi.avviso,
            "You joined a solar system.");*/
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnConnectedToMaster()
    {
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        appenaAperto = true;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        loading.SetActive(false);
        if (returnCode == 32766)
            MostraMessaggio.mm.Messaggio(
                MostraMessaggio.Messaggi.errore,
                "There is already a solar system called \"" + nomeSistemaIF.text + "\".\nDo you want to join the already existing solar system?",
                new List<MostraMessaggio.Azione>() {
                    new MostraMessaggio.Azione() {
                        buttonName = "Cancel",
                        buttonColor = Color.red,
                        eventi = new List<UnityEngine.Events.UnityAction>() {
                            () => MostraMessaggio.chiudi = true } },
                    new MostraMessaggio.Azione() {
                        buttonName = "Join",
                        buttonColor = new Color(0, .75f, 0, 1),
                        eventi = new List<UnityEngine.Events.UnityAction>() {
                            () => MostraMessaggio.chiudi = true,
                            () => PhotonNetwork.JoinRoom(nomeSistemaIF.text) } } }, -1);
        else
            MostraMessaggio.Errore(returnCode, message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        loading.SetActive(false);
        if (returnCode == 32765)
            MostraMessaggio.mm.Messaggio(
                MostraMessaggio.Messaggi.errore,
                "The solar system has reached the maximum number of players.");
        else if (returnCode == 32758)
            MostraMessaggio.mm.Messaggio(
                MostraMessaggio.Messaggi.errore,
                "There is no solar system with this name.");
        else
            MostraMessaggio.Errore(returnCode, message);
    }

    public void Scrolla(Vector2 v)
    {
        if (!scrollabile)
            return;
        sistemaSelGM.SetActive(false);
        cercaSistemaGM.SetActive(false);
        creaSistemaRT.gameObject.SetActive(false);
    }

    private void Start()
    {
        scrollRectSR = scrollRectRT.GetComponent<ScrollRect>();
    }

    void Update()
    {
        if (Input.touchCount > 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began)
            {
                dist1 = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                scrollRectSR.horizontal = scrollRectSR.vertical = false;
                sistemaSelGM.SetActive(false);
                creaSistemaRT.gameObject.SetActive(false);
                return;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(1).phase == TouchPhase.Ended)
            {
                scrollRectSR.horizontal = scrollRectSR.vertical = true;
                return;
            }
            if (!scrollabile)
                return;
            float dist2 = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
            Vector2 relativeMousePosition, mouseDalCentro;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(scrollRectSR.content, (Input.GetTouch(0).position + Input.GetTouch(1).position) / 2, null, out relativeMousePosition);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(scrollRectSR.content, new Vector2(Screen.width, Screen.height) / 2, null, out mouseDalCentro);
            _scaleIncrement = (dist2 - dist1) / 500f;
            scrollRectRT.localScale = new Vector3(Mathf.Clamp(scrollRectRT.localScale.x + _scaleIncrement, _minimumScale, _maximumScale), Mathf.Clamp(scrollRectRT.localScale.y + _scaleIncrement, _minimumScale, _maximumScale), 1f);
            scrollRectSR.normalizedPosition -= (mouseDalCentro - relativeMousePosition) / scrollRectSR.content.sizeDelta.x * _scaleIncrement;
            dist1 = dist2;
        }
    }

    public virtual void onEnable() => base.OnEnable();

    public override void OnEnable()
    {
        onEnable();
        DisattivaTutto();
        loading.SetActive(true);
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        for (int _o = 0; _o < sistemiRT.childCount; _o++)
            Destroy(sistemiRT.transform.GetChild(_o).gameObject);
        numSistemi = 0;
        circonferenze.Clear();
        diametroIniz.Clear();
        radiantiPerCirconferenza.Clear();
        partiOccupate.Clear();
        scrollRectRT.localScale = Vector3.one;
        PhotonNetwork.LeaveLobby();
    }
}


public static class ScrollToCenterHelper
{
    private const int MaxCornersCount = 4;
    private const float ScrollTimeStep = 0.10f;
    private const int MaxScrollTimeSec = 2;

    private static readonly Vector3[] _corners = new Vector3[MaxCornersCount];
    private static readonly WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();
    private static Coroutine _coroutine;

    public static void ScrollToCenter(this ScrollRect scrollRect, RectTransform target, MonoBehaviour monoBehaviour)
    {
        // The scroll rect's view's space is used to calculate scroll position
        var view = scrollRect.viewport != null ? scrollRect.viewport : scrollRect.GetComponent<RectTransform>();

        // Calcualte the scroll offset in the view's space
        var viewRect = view.rect;
        var elementBounds = target.TransformBoundsTo(view);
        var offset = viewRect.center - (Vector2)elementBounds.center;

        // Normalize and apply the calculated offset
        var scrollPos = scrollRect.normalizedPosition - scrollRect.NormalizeScrollDistance(Vector2.up, offset);

        if (_coroutine != null)
        {
            monoBehaviour.StopCoroutine(_coroutine);
        }
        Debug.Log(scrollPos);
        _coroutine = monoBehaviour.StartCoroutine(VerticalNormalizedPositionSmooth(scrollRect, new Vector2(Mathf.Clamp(scrollPos.x, 0f, 1f), Mathf.Clamp(scrollPos.y, 0f, 1f))));
    }

    private static Bounds TransformBoundsTo(this RectTransform source, Transform target)
    {
        var bounds = new Bounds();

        if (source != null)
        {
            source.GetWorldCorners(_corners);

            var vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            var matrix = target.worldToLocalMatrix;

            for (int j = 0; j < MaxCornersCount; j++)
            {
                Vector3 v = matrix.MultiplyPoint3x4(_corners[j]);
                vMin = Vector3.Min(v, vMin);
                vMax = Vector3.Max(v, vMax);
            }

            bounds = new Bounds(vMin, Vector3.zero);
            bounds.Encapsulate(vMax);
        }

        return bounds;
    }

    private static Vector2 NormalizeScrollDistance(this ScrollRect scrollRect, Vector2 axis, Vector2 distance)
    {
        var viewport = scrollRect.viewport;
        var viewRect = viewport != null ? viewport : scrollRect.GetComponent<RectTransform>();
        var viewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);

        var content = scrollRect.content;
        var contentBounds = content != null ? content.TransformBoundsTo(viewRect) : new Bounds();

        var hiddenLength = new Vector2(contentBounds.size[(int)axis.x] - viewBounds.size[(int)axis.x], contentBounds.size[(int)axis.y] - viewBounds.size[(int)axis.y]);

        return distance / hiddenLength;
    }

    private static IEnumerator VerticalNormalizedPositionSmooth(ScrollRect scrollRect, Vector2 position)
    {
        var maxTime = System.DateTime.Now.AddSeconds(MaxScrollTimeSec).Second;

        while (true)
        {
            scrollRect.normalizedPosition = Vector2.Lerp(scrollRect.normalizedPosition, position, ScrollTimeStep);

            yield return _waitForEndOfFrame;

            var pos1 = new Vector2(Mathf.Round(scrollRect.horizontalNormalizedPosition * 1000.0f), Mathf.Round(scrollRect.verticalNormalizedPosition * 1000.0f)) * 0.001f;
            var pos2 = new Vector2(Mathf.Round(position.x * 1000.0f), Mathf.Round(position.y * 1000.0f)) * 0.001f;

            if (pos1 == pos2 || maxTime <= System.DateTime.Now.Second)
            {
                scrollRect.normalizedPosition = position;
                Debug.Log(position);
                yield break;
            }
        }
    }
}