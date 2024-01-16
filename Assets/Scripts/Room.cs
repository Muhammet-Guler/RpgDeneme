using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class Room : MonoBehaviourPunCallbacks
{

    public GameObject Cube;
    public GameObject InstantieCube;
    public GameObject Panel;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.PlayerList.Length == 1)
            {
                InstantieCube= PhotonNetwork.Instantiate(Cube.name,new Vector3((float)-4.26,(float)80.43,5), Quaternion.identity);
                PhotonNetwork.PlayerList[0].NickName = "aaa";
            }
            if (PhotonNetwork.PlayerList.Length == 2)
            {
                InstantieCube = PhotonNetwork.Instantiate(Cube.name, new Vector3((float)4.26, (float)80.43, 5), Quaternion.identity);
                PhotonNetwork.PlayerList[0].NickName = "bbb";
            }
        }


        
    }

    void Update()
    {
        if (PhotonNetwork.PlayerList.Length == 1)
        {
            Panel.SetActive(true);
        }
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            Panel.SetActive(false);
        }
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Servere girildi");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("lobiye girildi");

    }
    public override void OnLeftRoom()
    {
        Debug.Log("odadan cikildi");
    }
    public override void OnLeftLobby()
    {
        Debug.Log("lobiden cikildi");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Hata:Odaya girilemedi");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Hata:Herhangi bir odaya girilemedi");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Hata:Oda Kurulamadý");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("oyundan cikildi");
    }
    public void CreateRoom()
    {

        RoomOptions roomOptions = new RoomOptions()
        {
            MaxPlayers = 2

        };

        PhotonNetwork.CreateRoom("CreateRoom", roomOptions);
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom("CreateRoom");
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("SampleScene");
    }
}
