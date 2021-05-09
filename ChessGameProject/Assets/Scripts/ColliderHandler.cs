using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderHandler : MonoBehaviour
{
    RaycastHit hit, tempHit;
    RaycastHit SquareHit;
    bool hitScanPiece;
    //레이어마스크: 플레이어 설정 필수 
    public LayerMask layerMaskPiece;
    public LayerMask layerMaskSquare;
    public Camera playerCamera;
    protected SquareGenerator theSquareGenerator;
    Piece tempPiece;


    void Awake()
    {
        theSquareGenerator = FindObjectOfType<SquareGenerator>();
    }
    

    public void SelectPiece()
    {
        //뭔가 선택되었다면 hit를 사용 
        MousePosRaycast();
        if (hitScanPiece) { 
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Piece"))    // Piece 의 레이어마스크 값 
            {
                Debug.Log("Piece 초기화 진입");
                tempPiece = hit.transform.GetComponent<Piece>();
                tempPiece.SelectedPiece();
                //SelectedPiece 필요 없는것 같다
                //내가 직접 여기서 타입을 가져와서, Board 에 SelectedPiece 변수에 직접 넣는게 편할 것 같다. 
                PieceType p1 = tempPiece.GetPieceType();
                theSquareGenerator.GenerateSquare(tempPiece);
            }
        }
        else if (SquareHit.transform.gameObject.layer == LayerMask.NameToLayer("Square"))
        {          
            //오류 이유: hit가 사라져서 
             Debug.Log("사각형 위치: " + SquareHit.transform.position);
            // = SquareHit.transform.position;
        }

        
    }//end function
   


    private void MousePosRaycast()
    {
        //     hit = null;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        //여기서 검출되면 hitScanPiece 가 true 됨 
        if (hitScanPiece = Physics.Raycast(ray, out hit, 100, layerMaskPiece))
        {
            //상황: 기물 밑에 사각형이 있을 때
            if(Physics.Raycast(ray, out SquareHit, 100, layerMaskSquare))
            {
                Debug.Log("기물 밑 사각형 클릭");
                hitScanPiece = false;
            }
            else
                Debug.Log("기물 클릭");
        }
        else if (Physics.Raycast(ray, out SquareHit, 100, layerMaskSquare))
        {
            Debug.Log("순수 사각형 클릭");
        }
        else
        {
            Debug.Log("Nothing Raycast");
            
        }//endif
    }//end function
}
