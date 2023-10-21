using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class networkManager : mappaManager
{
    public menù menu;
    
    public override void OnEnable()
    {
        base.onEnable();
        DisattivaTutto();
        loading.SetActive(true);
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = "1";
        }
        else
        {
            //if (!PhotonNetwork.InLobby)
            //  PhotonNetwork.JoinLobby();
            OnConnectedToMaster();
        }
        PhotonNetwork.LocalPlayer.NickName = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser != null ? Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser.DisplayName : "Me";
    }

    public override void OnConnectedToMaster()
    {
        //if (!PhotonNetwork.InLobby)
        //PhotonNetwork.JoinLobby();
        PhotonNetwork.JoinOrCreateRoom("ss" + (PlayerPrefs.HasKey("ss") ? PlayerPrefs.GetInt("ss") : 1), new RoomOptions() { MaxPlayers = 16, IsOpen = true, IsVisible = true, CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "posAsterIndex", 0 }, { "scaleAsterIndex", 0 } } }, null);
    }

    public override void OnJoinedRoom()
    {
        appenaAperto = true;
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnJoinedLobby()
    {
        appenaAperto = true;
        if (menu.auth != null)
            PhotonNetwork.NickName = menu.auth.CurrentUser != null ? menu.auth.CurrentUser.DisplayName : "Me";
        else
            PhotonNetwork.NickName = "Me";
    }

    public override void OnErrorInfo(ErrorInfo errorInfo)
    {
        print(errorInfo.Info);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        if (!appenaAperto)
        {
            PhotonNetwork.LeaveLobby();
            print("disabilitato");
            if (PhotonNetwork.InRoom)
                PhotonNetwork.LeaveRoom();
        }
    }
}