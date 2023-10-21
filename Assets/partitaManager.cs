using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class partitaManager : MonoBehaviourPunCallbacks
{
    public static List<List<Vector2>> posAsteroidiList = new List<List<Vector2>>()
    {
        new List<Vector2>()
        {
            new Vector2(376.5f, 135f),
            new Vector2(-141.5f, 242f),
            new Vector2(-174f, -75f),
            new Vector2(194.5f, -231.5f),
            new Vector2(280.5f, 258f),
            new Vector2(6f, -324.5f),
            new Vector2(-362f, -259f),
            new Vector2(-197.5f, -314f),
            new Vector2(23.5f, -62.5f),
            new Vector2(97f, -17f),
            new Vector2(-124.5f, -127f),
            new Vector2(-212f, 366.5f),
            new Vector2(361f, -160.5f),
            new Vector2(-210.5f, -39.5f),
            new Vector2(218f, -159f),
            new Vector2(-332f, -235.5f),
            new Vector2(259.5f, 291.5f),
            new Vector2(279f, 272.5f),
            new Vector2(352f, -212f),
            new Vector2(-83f, -130f),
            new Vector2(251.5f, 161f),
            new Vector2(-337.5f, 374.5f),
            new Vector2(286.5f, 395.5f),
            new Vector2(-88f, -259.5f),
            new Vector2(17.5f, -162f)
        },
        new List<Vector2>()
        {
            new Vector2(73f, 265f),
            new Vector2(96.5f, -245f),
            new Vector2(39.5f, 331f),
            new Vector2(94f, -238f),
            new Vector2(-42.5f, -55.5f),
            new Vector2(396f, -327f),
            new Vector2(345f, 127f),
            new Vector2(81.5f, -265.5f),
            new Vector2(-143f, 343f),
            new Vector2(43f, -152.5f),
            new Vector2(126f, -141f),
            new Vector2(-382.5f, -363.5f),
            new Vector2(30f, -110.5f),
            new Vector2(309.5f, 107.5f),
            new Vector2(63.5f, -315.5f),
            new Vector2(240.5f, -192f),
            new Vector2(-44f, 228f),
            new Vector2(-203f, 83.5f),
            new Vector2(-236f, 354.5f),
            new Vector2(-7f, 219f),
            new Vector2(-80f, -94f),
            new Vector2(-51.5f, -388.5f),
            new Vector2(-54.5f, 164f),
            new Vector2(39f, -400f),
            new Vector2(213.5f, -395f)
        },
        new List<Vector2>()
        {
            new Vector2(250.5f, -64f),
            new Vector2(-227f, 148.5f),
            new Vector2(110.5f, 183.5f),
            new Vector2(-201f, -378.5f),
            new Vector2(-59f, 376.5f),
            new Vector2(370f, 167f),
            new Vector2(-284.5f, 270.5f),
            new Vector2(-80f, 44.5f),
            new Vector2(214.5f, -90f),
            new Vector2(49f, -126f),
            new Vector2(243.5f, -325f),
            new Vector2(-241.5f, 105f),
            new Vector2(-111f, 226f),
            new Vector2(-370.5f, -319.5f),
            new Vector2(-65.5f, -95.5f),
            new Vector2(-358.5f, -162f),
            new Vector2(17f, -285.5f),
            new Vector2(303f, -236.5f),
            new Vector2(363.5f, -104f),
            new Vector2(197.5f, -57.5f),
            new Vector2(133.5f, 306f),
            new Vector2(-21.5f, -169.5f),
            new Vector2(245.5f, 309f),
            new Vector2(380.5f, 289.5f),
            new Vector2(-359.5f, -223f)
        },
        new List<Vector2>()
        {
            new Vector2(340.5f, 76.5f),
            new Vector2(303.5f, -21f),
            new Vector2(118f, 98f),
            new Vector2(97f, -288f),
            new Vector2(-81.5f, -240.5f),
            new Vector2(-65.5f, 362.5f),
            new Vector2(-284.5f, 75f),
            new Vector2(-397.5f, -198f),
            new Vector2(241.5f, 271.5f),
            new Vector2(-371.5f, -296.5f),
            new Vector2(-384.5f, 361.5f),
            new Vector2(-69.5f, -165.5f),
            new Vector2(-245f, 335f),
            new Vector2(88f, 277f),
            new Vector2(252f, 139.5f),
            new Vector2(-331.5f, -302f),
            new Vector2(25.5f, -172.5f),
            new Vector2(-386.5f, -95.5f),
            new Vector2(-328.5f, 281f),
            new Vector2(-238.5f, 0f),
            new Vector2(-22f, -51f),
            new Vector2(-139f, -337.5f),
            new Vector2(-201.5f, 341f),
            new Vector2(109f, 231.5f),
            new Vector2(88f, 173.5f)
        },
        new List<Vector2>()
        {
            new Vector2(-68.5f, 323.5f),
            new Vector2(-257f, 43f),
            new Vector2(-261f, -147f),
            new Vector2(-355.5f, 96f),
            new Vector2(336.5f, -315f),
            new Vector2(374.5f, -94f),
            new Vector2(313f, -273f),
            new Vector2(300.5f, -158.5f),
            new Vector2(-233f, 160.5f),
            new Vector2(-104.5f, 22f),
            new Vector2(394.5f, -392f),
            new Vector2(-85.5f, -136f),
            new Vector2(60f, -332f),
            new Vector2(-326f, 74.5f),
            new Vector2(-225.5f, -273.5f),
            new Vector2(367f, 70f),
            new Vector2(314f, 18.5f),
            new Vector2(23.5f, 141.5f),
            new Vector2(11.5f, -236.5f),
            new Vector2(-342.5f, 244f),
            new Vector2(316f, 97f),
            new Vector2(52f, 136f),
            new Vector2(-202f, -118f),
            new Vector2(-397f, 168f),
            new Vector2(15f, 259.5f)
        }
    };
    public static List<List<int>> scaleAsteroidiList = new List<List<int>>()
    {
        new List<int>()
        {
            26,
            15,
            12,
            12,
            14,
            15,
            15,
            24,
            13,
            29,
            27,
            28,
            18,
            29,
            14,
            14,
            27,
            24,
            10,
            22,
            29,
            22,
            29,
            24,
            15
        },
        new List<int>()
        {
            15,
            13,
            10,
            22,
            25,
            28,
            28,
            14,
            10,
            29,
            24,
            15,
            17,
            11,
            29,
            29,
            23,
            17,
            19,
            12,
            12,
            19,
            23,
            23,
            18
        },
        new List<int>()
        {
            11,
            12,
            11,
            14,
            25,
            19,
            25,
            27,
            21,
            17,
            22,
            11,
            19,
            15,
            15,
            29,
            15,
            24,
            17,
            18,
            10,
            27,
            19,
            22,
            11
        },
        new List<int>()
        {
            12,
            12,
            13,
            24,
            27,
            13,
            17,
            17,
            12,
            26,
            12,
            20,
            17,
            26,
            19,
            15,
            16,
            20,
            23,
            20,
            19,
            14,
            23,
            26,
            20,
        },
        new List<int>()
        {
            20,
            28,
            28,
            21,
            13,
            13,
            26,
            28,
            19,
            26,
            23,
            11,
            24,
            13,
            18,
            27,
            13,
            24,
            20,
            28,
            18,
            18,
            14,
            18,
            10,
        }
    };
    public static readonly int larg = 800;
    public static GameObject localPlayer;
    public GameObject playerPref;
    public GameObject ostacolo, playerNemico;
    public Transform ost, player;
    public Text vittoria;
    public Camera miniMappaCam;
    public ScrollRect minimappaSR;
    public static int numPlayers, numero;
    public Transform pianeti;
    public List<Transform> spawnZone;
    public static partitaManager pm;
    public GameObject nemicoPref;
    static int _punti;
    public Text sistemaNomeT;
    public static int punti
    {
        get { return _punti; }
        set
        {
            if (value != _punti)
                _punti = value;
            variabili.puntiT.text = value.ToString();
        }
    }

    public static Vector2 PosCasuale()
    {
        float a = Random.Range(0, Mathf.PI * 2);
        return new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * Random.Range(170, larg / 2);
    }

    public Vector2 PosCasualePlayer()
    {
        Vector2 a = new Vector2(Random.Range(0f, Mathf.PI * 2), Random.Range(0, spawnZone.Count));
        //print("a = " + a + " | " + new Vector2(Mathf.Cos(a.x), Mathf.Sin(a.x)) + " * " + Random.Range(0, spawnZone[(int)a.y].lossyScale.x / 2) + " + " + (Vector2)spawnZone[(int)a.y].position + " = " + (new Vector2(Mathf.Cos(a.x), Mathf.Sin(a.x)) * Random.Range(0, spawnZone[(int)a.y].lossyScale.x / 2) + (Vector2)spawnZone[(int)a.y].position));
        a = new Vector2(Mathf.Cos(a.x), Mathf.Sin(a.x)) * Random.Range(0, spawnZone[(int)a.y].lossyScale.x / 2) + (Vector2)spawnZone[(int)a.y].position;
        return a;
    }

    void Awake()
    {
        punti = 0;
        pm = this;
        if (!PhotonNetwork.IsConnected)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        PlayerPrefs.SetInt("ss", "ss1" == PhotonNetwork.CurrentRoom.Name ? 1 : 2);
        sistemaNomeT.text = PhotonNetwork.CurrentRoom.Name;
        if (localPlayer == null)
        {
            localPlayer = PhotonNetwork.Instantiate(playerPref.name, Vector2.zero, Quaternion.identity, 0);
        }
        player = localPlayer.transform;
        Application.targetFrameRate = 60;
        numero *= numPlayers;
        Vector2[] v = new Vector2[] { Vector2.one, new Vector2(-1, 1), Vector2.one * -1, new Vector2(1, -1) };
        for (int o = 0; o < 4; o++)
            for (int i = 0; i < 25; i++)
                Instantiate(ostacolo, posAsteroidiList[(int)PhotonNetwork.CurrentRoom.CustomProperties["posAsterIndex"]][i] * v[o], Quaternion.Euler(0, 0, Random.Range(0, 360)), ost).transform.localScale = Vector2.one * scaleAsteroidiList[(int)PhotonNetwork.CurrentRoom.CustomProperties["scaleAsterIndex"]][i] * 2;
        if (PhotonNetwork.IsMasterClient)
        for (int o = 0; o < 10; o++)
            PhotonNetwork.InstantiateRoomObject(nemicoPref.name, PosCasualePlayer(), Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360f))), 0);
            /*for (int i = 0; i < posAsteroidiList.Count; i++)
                for (int o = 0; o < posAsteroidiList[i].Count; o++)
                    if (posAsteroidiList[i][o].magnitude > 400)
                        print(posAsteroidiList[i][o] + "  " + posAsteroidiList[i][o].magnitude + "  " + i + "  " + o);
            */

            player.position = PosCasualePlayer();
        //playerNemico.transform.position = new Vector2(Random.Range(-scalaX / 2, scalaX / 2), Random.Range(-scalaY / 2, scalaY / 2));
        //transform.localScale = new Vector3(scalaX, scalaY, 1) * numero;
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
            pianeti.eulerAngles = new Vector3(0, 0, pianeti.eulerAngles.z + Time.deltaTime / 7);
        if (Input.GetKeyDown(KeyCode.D))
        {
            Vector3 a = new Vector2(Random.Range(0f, Mathf.PI * 2), Random.Range(0, spawnZone.Count));
            print("a = " + a + " | " + new Vector2(Mathf.Cos(a.x), Mathf.Sin(a.x)) + " * " + Random.Range(0, spawnZone[(int)a.y].lossyScale.x / 2) + " + " + (Vector2)spawnZone[(int)a.y].position + " = " + (new Vector2(Mathf.Cos(a.x), Mathf.Sin(a.x)) * Random.Range(0, spawnZone[(int)a.y].lossyScale.x / 2) + (Vector2)spawnZone[(int)a.y].position));
            a = new Vector2(Mathf.Cos(a.x), Mathf.Sin(a.x)) * Random.Range(0, spawnZone[(int)a.y].lossyScale.x / 2) + (Vector2)spawnZone[(int)a.y].position;
            a.z = 0;
            player.position = a;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            string s = "";
            Vector2 p;
            for (int o = 0; o < 5; o++)
            {
                s = "";
                for (int i = 0; i < 25; i++)
                {
                    p = PosCasuale();
                    s += "\nnew Vector2(" + p.x + "f, " + p.y + "f), ";
                }
                print(s);
            }
            print("Scale");
            float scala = 0;
            for (int o = 0; o < 5; o++)
            {
                s = "";
                for (int i = 0; i < 25; i++)
                {
                    scala = Random.Range(10, 30);
                    s += "\n" + scala + ",";
                }
                print(s);
            }
        }
    }

    public void ApriMinimappa(bool attiva)
    {
        miniMappaCam.backgroundColor = new Color(0, 0, 0, attiva ? .5f : 1);
        miniMappaCam.orthographicSize = attiva ? 410 : 50;
        if (attiva)
        {
            miniMappaCam.transform.SetParent(null);
            miniMappaCam.transform.position = Vector3.back * 15;
            minimappaSR.normalizedPosition = new Vector2(player.position.x + larg / 2, player.position.y + larg / 2) / larg;
        }
        else
        {
            miniMappaCam.transform.SetParent(Camera.main.transform);
            miniMappaCam.transform.localPosition = Vector3.back * 5;
        }
        movimento.puoiMuovere = !attiva;
    }

    public override void OnLeftRoom()
    {
        variabili.players.Clear();
        PhotonNetwork.LoadLevel(0);
    }

    public void LoadWarpScene()
    {
        PhotonNetwork.LeaveRoom();
        UnityEngine.SceneManagement.SceneManager.LoadScene("warp");
    }

    public void Esci() => PhotonNetwork.LeaveRoom();
}