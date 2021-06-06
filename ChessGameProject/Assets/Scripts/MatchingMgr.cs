using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class MatchingMgr : MonoBehaviourPunCallbacks
{
    private static MatchingMgr instance = null;
    public Text connectionInfo; // 매칭 정보 텍스트
    public Button MatchingBtn; // 매칭 버튼
    private PhotonView PV;

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != null) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        PV = photonView;
        Connect();
    }

    // 마스터 서버에 접속
    private void Connect()
    {
        Screen.SetResolution(1280, 768, false);
        PhotonNetwork.ConnectUsingSettings();
        MatchingBtn.interactable = false;
        connectionInfo.text = "서버에 접속 중...";
    }

    // 마스터 서버에 접속 성공
    public override void OnConnectedToMaster()
    {
        // PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        MatchingBtn.interactable = true;
        PhotonNetwork.AutomaticallySyncScene = true;
        connectionInfo.text = "서버에 접속 성공...";
        PV = photonView;
    }

    // 마스터 서버에 접속 실패 시 자동 재접속
    public override void OnDisconnected(DisconnectCause cause)
    {
        MatchingBtn.interactable = false;
        connectionInfo.text = "서버에 접속 실패, 재접속 중...";
        PhotonNetwork.ConnectUsingSettings();
    }

    // 매칭 버튼 클릭 시
    public void Matching()
    {
        connectionInfo.text = "상대방을 찾는 중...";
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfo.text = "생성된 방이 없음, 방을 생성 함";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PV.RPC("InGame", RpcTarget.AllViaServer);
        }
    }

    [PunRPC]
    private void InGame()
    {
        connectionInfo.text = "매칭 성공. 게임을 시작합니다.";
        PhotonNetwork.LoadLevel("ChessDemo");
    }

}


