using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    //아마 스퀘어셀렉터는 콜라이더 관련해서 통합될 듯 
    //raycast 관련 행동은 모두 하나로 통합할 예정 
    //whiteCamera 기준 0,0은 왼쪽 하얀 룩
    // 7 7은 오른쪽 블랙 룩
    /*
     //흰록,흰말,흰비숍,흰퀸,흰킹,흰비숍,흰말,흰록
     //흰폰 8개 
     //이후4칸 빔
     // 검폰 8개
     //검록,검말,검비숍,검퀸,검킹,검비숍,검말,검록
     Q: 왜 퀸킹 위치 반대임??    
     A: 0,0 이 whiteCamera 기준 하얀 록임. 
     */
    private const int boardSize = 8;
    [SerializeField] private ChessGameController theChessGameController;
    private Piece selectedPiece;

    public class PieceData
    {
       public PieceType pieceType;
       public TeamColor team;
    }

    public PieceData[,] BoardInPiece = new PieceData[8,8]; 
 

    //위치 정보만 담고 있는 PieceType Board 정보 


    private void Start()
    {
        //InisializeBoardinPiece()
    }

    //보드에 기물 위치 정보 초기화 
    private void InitializeBoardInPiece()
    {
        //위치 정보 초기화   
        //n+1 번째 줄 좌 -> 우 순 n+1 번째 칸 
        for (int i = 0; i < boardSize; ++i)
        {
            //흰폰 초기화 
            BoardInPiece[1, i].pieceType = PieceType.Pawn;
            BoardInPiece[1, i].team = TeamColor.White;
            //검폰 초기화
            BoardInPiece[6, i].pieceType = PieceType.Pawn;
            BoardInPiece[6, i].team = TeamColor.Black;
        } // 폰 위치 초기화 
        
        //흰룩 초기화
        BoardInPiece[0, 0].pieceType = PieceType.Rook;
        BoardInPiece[0, 0].team = TeamColor.White;
        BoardInPiece[0, 7].pieceType = PieceType.Rook;
        BoardInPiece[0, 7].team = TeamColor.White;

        //검룩 초기화
        BoardInPiece[7, 0].pieceType = PieceType.Rook;
        BoardInPiece[7, 0].team = TeamColor.Black;
        BoardInPiece[7, 7].pieceType = PieceType.Rook;
        BoardInPiece[7, 7].team = TeamColor.Black;

        //흰나
        BoardInPiece[0, 1].pieceType = PieceType.Knight;
        BoardInPiece[0, 1].team = TeamColor.White;
        BoardInPiece[0, 6].pieceType = PieceType.Knight;
        BoardInPiece[0, 6].team = TeamColor.White;
        //검나
        BoardInPiece[7, 1].pieceType = PieceType.Knight;
        BoardInPiece[7, 1].team = TeamColor.Black;
        BoardInPiece[7, 6].pieceType = PieceType.Knight;
        BoardInPiece[7, 6].team = TeamColor.Black;
        //흰숍
        BoardInPiece[0, 2].pieceType = PieceType.Bishop;
        BoardInPiece[0, 2].team = TeamColor.White;
        BoardInPiece[0, 5].pieceType = PieceType.Bishop;
        BoardInPiece[0, 5].team = TeamColor.White;
        //검숍
        BoardInPiece[7, 2].pieceType = PieceType.Bishop;
        BoardInPiece[7, 2].team = TeamColor.Black;
        BoardInPiece[7, 5].pieceType = PieceType.Bishop;
        BoardInPiece[7, 5].team = TeamColor.Black;
        //흰퀸
        BoardInPiece[0, 3].pieceType = PieceType.Queen;
        BoardInPiece[0, 3].team = TeamColor.White;
        //검퀸
        BoardInPiece[7, 3].pieceType = PieceType.Queen;
        BoardInPiece[7, 3].team = TeamColor.Black;

        //흰킹
        BoardInPiece[0, 4].pieceType = PieceType.King;
        BoardInPiece[0, 4].team = TeamColor.White;
        //검킹
        BoardInPiece[7, 4].pieceType = PieceType.King;
        BoardInPiece[7, 4].team = TeamColor.Black;
    }

    //Vector2Int 좌표를 기물에 적용할 Vector3Int 좌표로 바꾸는 코드
    public Vector3Int CalculateCoordsToPosition(Vector2Int coords)
    {
        return new Vector3Int(coords.x, 0, coords.y);
    }
    //Vector3 또는 Vector3Int 좌표를 Vector2Int 좌표로 바꾸는 코드
    public Vector2Int CalculatePositionToCoords(Vector3 coords)
    {
        return new Vector2Int((int)coords.x, (int)coords.z);
    }
    public int GetBoardSize()
    {
        return boardSize;
    }

    public void SetSelectedPiece(Piece tempPiece)
    {
        selectedPiece = tempPiece;
    }
    public Piece GetSelectedPiece()
    {
        return selectedPiece;
    }

}
