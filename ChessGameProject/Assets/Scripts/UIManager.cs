using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class UIManager : MonoBehaviourPunCallbacks
{
    public Text[] usertext;
    int my;
    public Camera UserCamera;

    void Start()
    {
        NickName();
        CameraSet();
    }

    public void NickName()
    {
        for(int i=0; i<2; i++)
        {
            if (PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer)
                usertext[i].text = "여기가 나임";
        }
    }

    public void CameraSet()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            UserCamera.transform.position = new Vector3(3.5f, 8, -1);
            UserCamera.transform.Rotate(130, -180, 0);
        }
    }
}
