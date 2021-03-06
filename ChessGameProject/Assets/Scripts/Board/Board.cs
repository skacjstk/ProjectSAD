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



    //위치 정보만 담고 있는 grid 정보 
    public Piece[,] grid = new Piece[boardSize, boardSize];



    private void Start()
    {
        //ChessGameController.StartNewGame() 이후에 동작 할까...?
    }

    //보드에 기물 위치 정보 초기화    
    //인스턴스 접근을 잘못해서 다시 짜야 함.
    //클래스를 만들 때는 생성자를 이용해야지 직접 접근으로 생성할 수 없음.
    public void InitializeGrid(Vector3 coords, GameObject tempObject)
    {
        //tempObject 로 coords 새로 불러오면, 여기서 new 해야 된다. 자원낭비임 
        grid[(int)coords.z, (int)coords.x] = tempObject.GetComponent<Piece>();
        //위치 정보 초기화   
        //n+1 번째 줄 좌 -> 우 순 n+1 번째 칸 
        Debug.Log("Piece:"+grid[(int)coords.z, (int)coords.x].GetPieceType()+"\tcoords: "+coords);    
    }

    public bool CheckGrid(Vector3Int coords)
    {
        //임시
        return Random.value > 0.5f; //Random.value 는 0.0~ 1.0 을 랜덤으로 반환
    }

    //{get; set;}
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
