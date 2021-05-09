using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어가 기물을 클릭 했을 때, 그 기물의 이동 가능한 위치를 받아 Board 에 표시한다. 
public class SquareGenerator : MonoBehaviour
{
    List<Object> SquareList = new List<Object>();
    [SerializeField] private Object squarePrefab;
    private Board theBoard;
    // Start is called before the first frame update
    void Start()
    {
        theBoard = FindObjectOfType<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateSquare(Piece tempPiece)
    {
        List<Vector2Int> tempDirections;
        Vector3Int calDirection = new Vector3Int();
        PieceType pType = tempPiece.GetPieceType();
        //폰은 킬좌표와 이동좌표가 따로 있음 ( direction[0] 이 이동좌표, direction[1~2] 가 kill 좌표 
        //삼각형 만드는 곳에 좌표 검사를 하자. 여기선 그냥 명령만 
        SquareClear();

        //당연하겠지만 폰 검사와 유효 바닥 검사 따로 해야함
        //여기서 검사하고, 유효한 Square 만 생성하는 식으로 하자 
        if (pType.Equals(PieceType.Pawn))
        {
            tempDirections = tempPiece.GetDirections();
            Debug.Log("tempVector 길이" + tempDirections.Count + "위치: "+tempPiece.transform.position);

            //Piece 의 현재 좌표에서 directions[0] 의 위치에 아무것도 없다면 이동 가능함

            //다른 애들은 팔방 인데 얘만 직선이라 앞뒤 따라 좌표가 다르다. 
            if (tempPiece.team.Equals(TeamColor.Black))
                SquareCreate(calDirection, -tempDirections[0], tempPiece);
            else
                SquareCreate(calDirection, tempDirections[0], tempPiece);
            //Piece 의 현재 좌표에서 directions[1].[2] 의 위치에 
            for (int i=1; i < tempDirections.Count; ++i)
            {
                if (tempPiece.team.Equals(TeamColor.Black))                
                    SquareCreate(calDirection, -tempDirections[i], tempPiece);
                else
                    SquareCreate(calDirection, tempDirections[i], tempPiece);
            }
        }
        //킹 역시 킬좌표에 체크검사를 해야 함
        else if (pType.Equals(PieceType.King))
        {

        }
        //나이트는 이동 지점이 선이 아니라 포인트임 
        else if (pType.Equals(PieceType.Knight))
        {

        }
        else //나머지는 킬좌표와 이동좌표가 같음 
        {
            tempDirections = tempPiece.GetDirections();
            Debug.Log("tempVector 길이" + tempDirections.Count + "위치: " + tempPiece.transform.position);

            //Piece 의 현재 좌표에서 directions[0] 의 위치에 아무것도 없다면 이동 가능함
            foreach(Vector2Int tempDirection in tempDirections)
            {
                    SquareLineCreate(calDirection, tempDirection, tempPiece);
            }
        } //endif
    } //endfunction

    private void SquareCreate(Vector3Int calDirection, Vector2Int tempDirection, Piece tempPiece)
    {
        //현재 위치를 calDirection 에 담고 
        calDirection = new Vector3Int(Mathf.RoundToInt(tempPiece.transform.position.x), 0, Mathf.RoundToInt(tempPiece.transform.position.z));
        //calDIrection 에 사각형 위치를 더하고
         calDirection += theBoard.CalculateCoordsToPosition(tempDirection);

        //객체를 만들기 (이걸 어디에 담아서 파괴시켜야 한다)
        SquareList.Add(Instantiate(squarePrefab, calDirection, Quaternion.identity));
    }
    private void SquareLineCreate(Vector3Int calDirection, Vector2Int tempDirection, Piece tempPiece)
    {
        //현재 위치를 calDirection 에 담고 
        calDirection = new Vector3Int(Mathf.RoundToInt(tempPiece.transform.position.x), 0, Mathf.RoundToInt(tempPiece.transform.position.z));

        //  calDirection += theBoard.CalculateCoordsToPosition(tempDirection);
        //calDIrection 에 사각형 위치를 더하고
        //내 위치와 목표 위치 사이에 삼각형을 만든다. 
        for (int i = 0; i < theBoard.GetBoardSize(); ++i)
        {            
            //직선 (앞)
            if (tempDirection.x == 0)
            {
                if (tempDirection.y < 0)
                {
                    calDirection += new Vector3Int(0, 0, 1);
                    SquareList.Add(Instantiate(squarePrefab, calDirection, Quaternion.identity));
                }

                else
                {
                    calDirection += new Vector3Int(0, 0, -1);
                    SquareList.Add(Instantiate(squarePrefab, calDirection, Quaternion.identity));
                }//endif
            }

            //직선 (옆)
            else if (tempDirection.y == 0)
            {
                if (tempDirection.x < 0)
                {
                    calDirection += new Vector3Int(1, 0, 0);
                    SquareList.Add(Instantiate(squarePrefab, calDirection, Quaternion.identity));
                }

                else
                {
                    calDirection += new Vector3Int(-1, 0, 0);
                    SquareList.Add(Instantiate(squarePrefab, calDirection, Quaternion.identity));
                }//endif
            }

            //대각선
            else
            {
                if (tempDirection.x < 0 && tempDirection.y < 0)
                {
                    calDirection += new Vector3Int(1, 0, 1);
                    SquareList.Add(Instantiate(squarePrefab, calDirection, Quaternion.identity));
                }
                else if (tempDirection.x > 0 && tempDirection.y < 0)
                {
                    calDirection += new Vector3Int(-1, 0, 1);
                    SquareList.Add(Instantiate(squarePrefab, calDirection, Quaternion.identity));
                }
                else if (tempDirection.x < 0 && tempDirection.y > 0)
                {
                    calDirection += new Vector3Int(1, 0, -1);
                    SquareList.Add(Instantiate(squarePrefab, calDirection, Quaternion.identity));
                }
                else if (tempDirection.x > 0 && tempDirection.y > 0)
                {
                    calDirection += new Vector3Int(-1, 0, -1);
                    SquareList.Add(Instantiate(squarePrefab, calDirection, Quaternion.identity));
                }
            }
        }
    }//end function 

    private void SquareClear()
    {
        if (SquareList.Count >= 1)
        {
            foreach (Object n in SquareList)
                Destroy(n);
        }
        SquareList.Clear();
    }
}
