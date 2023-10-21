using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class sistemaSolareIMappa : MonoBehaviour
{
    public mappaManager mm;
    public TMPro.TextMeshProUGUI nomeT;
    public Text playerCountT;
    RoomInfo _roomInfo;
    public RoomInfo roomInfo
    {
        get { return _roomInfo; }
        set
        {
            if (_roomInfo != value)
            {
                _roomInfo = value;
                UpdateInfo();
            }
        }
    }

    void UpdateInfo()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
        nomeT.text = roomInfo.Name;
        playerCountT.text = roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers;
        playerCountT.color = roomInfo.PlayerCount == roomInfo.MaxPlayers ? new Color(1, .25f, 0, 1) : new Color(0, .5f, 1, 1);
        GetComponent<Button>().onClick.AddListener(() => mm.SelezionaSistema(roomInfo.Name, roomInfo.PlayerCount, roomInfo.MaxPlayers, GetComponent<RectTransform>()));
    }
}
