using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessGameController : MonoBehaviour
{

    private Board theBoard;
    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        theBoard = FindObjectOfType<Board>();
        //의존관계 설정 후 게임 초기화 ( 씬이 불러와질 때 )


    }
    //Board 의 함수를 반환하는 체스 컨트롤러의 함수 
    public void SetSelectedPiece(Piece tempPiece)
    {
        theBoard.SetSelectedPiece(tempPiece); 
    }
    public Piece GetSelectedPiece()
    {
        return theBoard.GetSelectedPiece();
    }

    public void PiecePosMove(Vector3 beforePos, Vector3 afterPos)
    {
        Vector2Int tempBeforePos = theBoard.CalculatePositionToCoords(beforePos);
       Vector2Int tempAfterPos = theBoard.CalculatePositionToCoords(afterPos);

        theBoard.BoardInPiece[tempAfterPos.y, tempAfterPos.x] = theBoard.BoardInPiece[tempBeforePos.y, tempBeforePos.x];
        theBoard.BoardInPiece[tempBeforePos.y, tempBeforePos.x] = null;


    }


}
