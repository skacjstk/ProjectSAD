using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderHandler : MonoBehaviour
{
    RaycastHit hit, tempHit;
    RaycastHit SquareHit;
    bool hitScanPiece, hitScanSquare;
    //레이어마스크: 플레이어 설정 필수 
    public LayerMask layerMaskPiece;
    public LayerMask layerMaskSquare;
    public Camera playerCamera;
    private SquareGenerator theSquareGenerator;
    private ChessGameController theChessGameController;
    Piece tempPiece;

    void Awake()
    {
        theSquareGenerator = FindObjectOfType<SquareGenerator>();
        theChessGameController = FindObjectOfType<ChessGameController>();
    }

    public void SelectPiece()
    {
        //뭔가 선택되었다면 hit를 사용 
        MousePosRaycast();
        if (hitScanPiece) { 
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Piece"))    // Piece 의 레이어마스크 값 
            {
                Debug.Log("Piece 할당 및 선택한Piece 갱신");
                tempPiece = hit.transform.GetComponent<Piece>(); //직전에 선택한 Piece의 컴포넌트 할당 
                theChessGameController.SetSelectedPiece(tempPiece);       //다른 스크립트에서 직전 선택 Piece 가져올 때 사용해야 한다.
                theSquareGenerator.GenerateSquare(tempPiece);
            }//endif
        }
        else if (hitScanSquare && SquareHit.transform.gameObject.layer == LayerMask.NameToLayer("Square"))
        {
            Debug.Log("사각형 위치: " + SquareHit.transform.position);
            //보드에 위치 정보 갱신
            theChessGameController.PiecePosMove(tempPiece.transform.position, SquareHit.transform.position);
            //그래픽적인 위치 이동 
            tempPiece.transform.position = SquareHit.transform.position;           
            theSquareGenerator.SquareClear();  
        }
        else {  //사각형, 기물 둘다 아닐 때, Nothing raycast 일 때 
         theSquareGenerator.SquareClear();  
        }//endif
    }//end function


    private void MousePosRaycast()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        //여기서 검출되면 hitScanPiece 가 true 됨 
        if (hitScanPiece = Physics.Raycast(ray, out hit, 100, layerMaskPiece))
        {
            //상황: 기물 밑에 사각형이 있을 때
            if (hitScanSquare = Physics.Raycast(ray, out SquareHit, 100, layerMaskSquare))
            {
                Debug.Log("기물 밑 사각형 클릭");
                hitScanPiece = false;
            }
            else //endif
            { Debug.Log("기물 클릭");
                hitScanSquare = false;
            }
        }
        else if (hitScanSquare = Physics.Raycast(ray, out SquareHit, 100, layerMaskSquare))
        {
            Debug.Log("순수 사각형 클릭");
        }
        else
        {
            hitScanSquare = false;
            hitScanPiece = false;
            Debug.Log("Nothing Raycast");
            
        }//endif
    }//end function
}
