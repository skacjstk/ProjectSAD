using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PiecesGenerator))]
public class ChessGameController : MonoBehaviour
{

    private Board theBoard;
    private PiecesGenerator thePieceGenerator;
    void Awake()
    {
        theBoard = FindObjectOfType<Board>();
        thePieceGenerator = FindObjectOfType<PiecesGenerator>();
    }
    // Start is called before the first frame update
    void Start()
    {

        //의존관계 설정 후 게임 초기화 ( 씬이 불러와질 때 )

        StartNewGame();

    }
    //게임이 시작될 때, 뭔가를 초기화 하는데 쓰려고 만듬 
    private void StartNewGame()
    {
        thePieceGenerator.InitializePieces();
    }
    /// <summary>
    /// Board 에 InitializeGrid 로 가는 중간다리 
    /// </summary>
    /// <param name="coords"></param>
    /// <param name="tempObject"></param>
    public void InitializeGrid(Vector3 coords, GameObject tempObject)
    {
        theBoard.InitializeGrid(coords, tempObject);
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

    //Piece 가 beforePos 에서 afterPos 로 움직일 때 
    public void PiecePosMove(Vector3 beforePos, Vector3 afterPos)
    {
        Vector2Int tempBeforePos = theBoard.CalculatePositionToCoords(beforePos);
        Vector2Int tempAfterPos = theBoard.CalculatePositionToCoords(afterPos);

        ChangePawnMove();     //ChessGameController 에서 Pawn에 함수 호출을 위한 징검다리 

        //PiecePosMove 에서 if문으로  그 grid 의 위치를 검사하자, 그렇게 색깔이 다를 경우 먹어버리고, 색깔이 같은데 뭔가 있을 경우 캐슬링 또는 오류를 출력하자 
        //대상 그리드에 뭐가 있을 경우
        if (theBoard.grid[tempAfterPos.y, tempAfterPos.x] != null)
        {
            //그래픽적인 파괴
            Destroy(theBoard.grid[tempAfterPos.y, tempAfterPos.x].gameObject);
            //시스템적인 파괴
            theBoard.grid[tempAfterPos.y, tempAfterPos.x] = null;
            theBoard.grid[tempAfterPos.y, tempAfterPos.x] = theBoard.grid[tempBeforePos.y, tempBeforePos.x];

            // 참고로, PiecePosMove 의 경우, 유효하다고 판단한 사각형을 클릭했을 때, 호출되는 함수 이기 때문에, 아군 과 적군의 검사는 SquareGenerator 의 몫
        }
        else  //평범한 이동일 경우
        {
            theBoard.grid[tempAfterPos.y, tempAfterPos.x] = theBoard.grid[tempBeforePos.y, tempBeforePos.x];
            theBoard.grid[tempBeforePos.y, tempBeforePos.x] = null;
        }
    } //end function

    public void ChangePawnMove()
    {
        if (GetSelectedPiece().GetPieceType() == PieceType.Pawn)
        {
            Piece tempPiece = GetSelectedPiece();
            tempPiece.GetComponent<Pawn>().PawnMove();
        }
    }
}
