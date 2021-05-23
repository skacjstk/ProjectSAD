using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessGameController : MonoBehaviour
{

    private Board theBoard;
    private PiecesGenerator thePieceGenerator;

    public int currentGameTurn;
    void Awake()
    {
        theBoard = FindObjectOfType<Board>();
        thePieceGenerator = FindObjectOfType<PiecesGenerator>();
        currentGameTurn = 1;
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
        Debug.Log("현재 선택한 Piece: " + tempPiece.GetPieceType());
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
        TeamColor tempTeam;

        ChangePawnMove();     //ChessGameController 에서 Pawn에 함수 호출을 위한 징검다리 
        //PiecePosMove 에서 if문으로  그 grid 의 위치를 검사하자, 그렇게 색깔이 다를 경우 먹어버리고, 색깔이 같은데 뭔가 있을 경우 캐슬링 또는 오류를 출력하자 
        //대상 그리드에 뭐가 있을 경우
        if (theBoard.grid[tempAfterPos.y, tempAfterPos.x] != null)     
        {
            //그래픽적인 파괴
            Destroy(theBoard.grid[tempAfterPos.y, tempAfterPos.x].gameObject);
            //시스템적인 파괴
            theBoard.grid[tempAfterPos.y, tempAfterPos.x] = null;   //이후 위치에 네가 없다.
            theBoard.grid[tempAfterPos.y, tempAfterPos.x] = theBoard.grid[tempBeforePos.y, tempBeforePos.x];        //그곳에 내가 있다.
            theBoard.grid[tempBeforePos.y, tempBeforePos.x] = null; //이전 위치에 내가 있었다.

            // 참고로, PiecePosMove 의 경우, 유효하다고 판단한 사각형을 클릭했을 때, 호출되는 함수 이기 때문에, 아군 과 적군의 검사는 SquareGenerator 의 몫
        }
        //상대편이면서, enpassent 가능
        else if (IsMoveEnpassent(tempAfterPos, out tempTeam))
        {
            int tempInt = 1;
            if (tempTeam == TeamColor.White)
            {
                tempInt = -1;
            }
            //파괴할 대상의 위치    grid[tempAfterPos.y + tempInt, tempAfterPos.x]
            Destroy(theBoard.grid[tempAfterPos.y+ tempInt, tempAfterPos.x].gameObject);
            theBoard.grid[tempAfterPos.y + tempInt, tempAfterPos.x] = null;
            theBoard.grid[tempAfterPos.y, tempAfterPos.x] = theBoard.grid[tempBeforePos.y, tempBeforePos.x];
            theBoard.grid[tempBeforePos.y, tempBeforePos.x] = null; //이전 위치에 내가 있었다.
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
            if (tempPiece.MovePiece())
            {
                tempPiece.GetComponent<Pawn>().enpassentTurn = currentGameTurn + 1;
   //             Debug.Log("이 Pawn 을 양파상 가능한 턴: " + tempPiece.GetComponent<Pawn>().enpassentTurn);
            }
        }
    }
    private bool IsMoveEnpassent(Vector2Int tempAfterPos, out TeamColor tempTeam)
    {
        int tempInt = 1;
        Piece tempPiece = GetSelectedPiece();
        Piece afterPiece = null;
        if (tempPiece.GetPieceType() == PieceType.Pawn)
        {
            if (tempPiece.team == TeamColor.White)
            {
                tempInt = -1; 
                tempTeam = TeamColor.White;
            }
            else
                tempTeam = TeamColor.Black;
            if(theBoard.grid[tempAfterPos.y + tempInt, tempAfterPos.x] != null)
            {
                afterPiece = theBoard.grid[tempAfterPos.y + tempInt, tempAfterPos.x].GetComponent<Piece>();
                if (afterPiece.GetPieceType() == PieceType.Pawn)
                {
                    if (tempPiece.team != afterPiece.team)
                    {
                        return afterPiece.GetComponent<Pawn>().enpassent;
                    }
                }
            }    
            return false;
        }
        else
        {
            tempTeam = TeamColor.White;
            return false;
        }
    }

    private void IsEnpassentKill()
    {

    }

    //한 턴이 끝날 때 호출되는 함수
    public void EndTurn()
    {
        ++currentGameTurn;
 //       Debug.Log("EndTurn 이후 턴: " + currentGameTurn);
    }
}
