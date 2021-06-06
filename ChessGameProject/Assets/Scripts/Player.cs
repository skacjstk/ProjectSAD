using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//카메라 좌표
// 블랙: 3.5, 8 ,8  yRotation 180
// 화이트: 3.5, 8, -1, yRotation 0
public class Player : ColliderHandler
{
    // Start is called before the first frame update
    TeamColor playerTeam = TeamColor.Black;
    void Start()
    {
        CameraSet();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SelectPiece();
        }
    }

    private void CameraSet()
    {
        //BlackTeam 을할당 받았을 경우. 카메라 위치를 검은색으로 바꾸기 
        if (playerTeam.Equals(TeamColor.Black))
        {
            Debug.LogWarning("실행됨 카메라셋");
            playerCamera.transform.position = new Vector3(3.5f, 8f, 8f);
            playerCamera.transform.Rotate(130, -180, 0);
        }
    }
}
