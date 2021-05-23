using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour

{
    public TeamColor team;
   // [SerializeField] private Board board;
    //행동 완료 검사 (필요 한가?)
    public bool hasMoved = false;
    //현재 움직일 수 있는 위치 
    public List<Vector2Int> availableMoves;


    public abstract void SelectedPiece();


    public abstract PieceType GetPieceType();

    public abstract bool CanMove(Vector2Int coords);

    public abstract bool MovePiece( );


    public abstract List<Vector2Int> GetDirections();



}
