using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

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

        ChangePawnMove();
        //PiecePosMove 에서 if문으로  그 grid 의 위치를 검사하자, 그렇게 색깔이 다를 경우 먹어버리고, 색깔이 같은데 뭔가 있을 경우 캐슬링 또는 오류를 출력하자 
        //대상 그리드에 뭐가 있을 경우
        if (theBoard.grid[tempAfterPos.y, tempAfterPos.x] != null)     
        {
            //그래픽적인 파괴
            Destroy(theBoard.grid[tempAfterPos.y, tempAfterPos.x].gameObject);
            //시스템적인 파괴
            theBoard.grid[tempBeforePos.y, tempBeforePos.x].hasMoved = true;
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
            theBoard.grid[tempBeforePos.y, tempBeforePos.x].hasMoved = true;
            theBoard.grid[tempAfterPos.y, tempAfterPos.x] = theBoard.grid[tempBeforePos.y, tempBeforePos.x];
            theBoard.grid[tempBeforePos.y, tempBeforePos.x] = null;
            //킹은 위에서 이미 이동했음 
            if (IsMoveCastling(tempBeforePos, tempAfterPos))
            {
                Debug.Log("캐슬링 했다");
                //한쪽 위치의 rook 가져오기 
                //afterPos 가 4보다 클 경우 7, 4보다 작을 경우 0
                if (tempAfterPos.x < 4)
                {
                    tempBeforePos = new Vector2Int(0, tempAfterPos.y);
                    tempAfterPos = new Vector2Int(tempAfterPos.x+1,tempAfterPos.y);
                }
                else if (4 < tempAfterPos.x)    //4보다 크면
                {
                    tempBeforePos = new Vector2Int(7, tempAfterPos.y);
                    tempAfterPos = new Vector2Int(tempAfterPos.x - 1, tempAfterPos.y);
                }
                theBoard.grid[tempBeforePos.y, tempBeforePos.x].hasMoved = true;
                Debug.Log("Rook 의 시스템적 이동: " + tempBeforePos + "\t---->\t" + tempAfterPos);
                theBoard.grid[tempAfterPos.y, tempAfterPos.x] = theBoard.grid[tempBeforePos.y, tempBeforePos.x];
                theBoard.grid[tempBeforePos.y, tempBeforePos.x] = null;
                //캐슬링 용 그래픽적 이동
                theBoard.grid[tempAfterPos.y, tempAfterPos.x].transform.position = new Vector3(tempAfterPos.x, 0f, tempAfterPos.y);
            }
            if(GetSelectedPiece().GetPieceType() == PieceType.Pawn)
            {
                theBoard.Promotion(tempAfterPos);
            }
        }//
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
    private bool IsMoveCastling(Vector2Int tempBeforePos, Vector2Int tempAfterPos)
    {
        //킹일때만
        if (theBoard.grid[tempAfterPos.y, tempAfterPos.x] == null)
            return false;
        if(theBoard.grid[tempAfterPos.y, tempAfterPos.x].GetPieceType().Equals(PieceType.King))
        {
            Debug.LogWarning("좌표검사:" + tempBeforePos + tempAfterPos);
            int calCast = tempBeforePos.x - tempAfterPos.x;
            if (calCast == 2 || calCast == -2)  //킹은 두칸 이동이 안되는데, x 좌표로 2칸 이동한건 캐슬링밖에 없지 
                return true;
            else
                return false;
        }

        return false;
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
