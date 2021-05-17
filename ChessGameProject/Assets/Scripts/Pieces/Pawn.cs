using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    PieceType pieceType = PieceType.Pawn;
    //    List<Vector2Int> killDirections = new List<Vector2Int>();  //WhiteCamera 기준 z 음수가 앞, x 음수가 오른쪽 ( 이것은 전용함수로 교정할 예정 ) 양파상도 이것으로 판단함

    ///첫번째 움직임(첫 2칸 전용
    public bool firstMove = true;
    ///양파상
    public bool enpassent = true;

    // index 0은 이동좌표, index 1과 2는 kill 좌표 
    readonly List<Vector2Int> directions = new List<Vector2Int>() {
        new Vector2Int(0,1), 
        new Vector2Int(1, 1) , 
        new Vector2Int(-1, 1) 
    };
    private void Start()
    {
       firstMove = true;
      enpassent = true;
    }
    /// <summary>
    /// Pawn 의 첫 움직임 때 firstMove 는 false 가 되고 enpassent 가 true가 된다. 다음부터는 enpassent 와 firstMove 는 영원히 false 를 반복하게 된다.  양파상이 true 일 경우, 이 Pawn 의 위치는 1칸 움직인 것으로 간주할 수 있다.
    /// </summary>
    public void PawnMove()
    {
        if (firstMove && enpassent)
            enpassent = true;
        else
            enpassent = false;
        firstMove = false;
    }


    public override void SelectedPiece()
    {
        Debug.Log("SelectedPiece 자식 함수 호출됨: ");
    }
    public override PieceType GetPieceType()
    {
        return pieceType;
    }

    //움직일 수 있는 곳을 반환하는 함수 
    public override bool CanMove(Vector2Int coords)
    {
        throw new System.NotImplementedException();
    }

    //실제로 Piece를 움직이게 하는 함수 
    public override void MovePiece(Vector2Int coords)
    {
        throw new System.NotImplementedException();
    }

    public override List<Vector2Int> GetDirections()
    {
        return directions;
    }

}
