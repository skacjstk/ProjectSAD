using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    PieceType pieceType = PieceType.King;
    //    List<Vector2Int> killDirections = new List<Vector2Int>();  //WhiteCamera 기준 z 음수가 앞, x 음수가 오른쪽 ( 이것은 전용함수로 교정할 예정 ) 양파상도 이것으로 판단함

    // Rook 은 직선방향 
    readonly List<Vector2Int> directions = new List<Vector2Int>() {
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0),
        //대각선 4방 
       new Vector2Int(1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, 1),
        new Vector2Int(-1, -1),
    };



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
