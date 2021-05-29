using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어가 기물을 클릭 했을 때, 그 기물의 이동 가능한 위치를 받아 Board 에 표시한다. 
public class SquareGenerator : MonoBehaviour
{
    List<Object> SquareList = new List<Object>();
    [SerializeField] private Object squarePrefab;
    private Board theBoard;
    int flag = 0;
    // Start is called before the first frame update
    void Start()
    {
        theBoard = FindObjectOfType<Board>();
        flag = 0;
    }


    public void GenerateSquare(Piece tempPiece)
    {
        List<Vector2Int> tempDirections;
        Vector3Int calDirection = new Vector3Int();
        PieceType pType = tempPiece.GetPieceType();
        //폰은 킬좌표와 이동좌표가 따로 있음 ( direction[0] 이 이동좌표, direction[1~2] 가 kill 좌표 
        //사각형 만드는 곳에 좌표 검사를 하자. 여기선 그냥 명령만 
        SquareClear();

        if (pType.Equals(PieceType.Pawn))
        {
            tempDirections = tempPiece.GetDirections();
            Debug.Log("tempVector 길이" + tempDirections.Count + "위치: "+tempPiece.transform.position);

            //Piece 의 현재 좌표에서 directions[0] 의 위치에 아무것도 없다면 이동 가능함
            //다른 애들은 팔방 인데 얘만 직선이라 앞뒤 따라 좌표가 다르다. 
            if (tempPiece.team.Equals(TeamColor.Black))
            {
                SquareCreate(calDirection, -tempDirections[0], tempPiece);
                if (!tempPiece.hasMoved)
                {
                    Vector2Int tempDirection = new Vector2Int(-tempDirections[0].x, -(tempDirections[0].y + 1));
                    SquareCreate(calDirection, tempDirection, tempPiece); 
                }
            }
            else
            {
                SquareCreate(calDirection, tempDirections[0], tempPiece);
                if (!tempPiece.hasMoved)
                {
                    Vector2Int tempDirection = new Vector2Int (tempDirections[0].x, tempDirections[0].y + 1);
                    SquareCreate(calDirection, tempDirection, tempPiece);
                }
            }
            //Piece 의 현재 좌표에서 directions[1].[2] 의 위치(kill 위치) 
            for (int i=1; i < tempDirections.Count; ++i)
            {
                if (tempPiece.team.Equals(TeamColor.Black))                
                    SquareCreate(calDirection, -tempDirections[i], tempPiece);
                else
                    SquareCreate(calDirection, tempDirections[i], tempPiece);
            }

        }//end ifpawn
        //킹 역시 킬좌표에 체크검사를 해야 함
        else if (pType.Equals(PieceType.King))
        {
            //임시 
            theBoard.CheckCastling(tempPiece);   //기존과 개별로 사각형 생성
            
            //킹의 경우 개별적인 행동 별 Check 검사가 필요함 
            tempDirections = tempPiece.GetDirections();
            Debug.Log("tempVector 길이" + tempDirections.Count + "위치: " + tempPiece.transform.position);   
        }
        //나이트는 이동 지점이 선이 아니라 포인트임 
        else if (pType.Equals(PieceType.Knight))
        {
            tempDirections = tempPiece.GetDirections();
            Debug.Log("tempVector 길이" + tempDirections.Count + "위치: " + tempPiece.transform.position);

            foreach (Vector2Int tempDirection in tempDirections)
            {
                SquareCreate(calDirection, tempDirection, tempPiece);
            }
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
                
        flag = CheckValidSquare(calDirection, tempPiece);
        switch (flag)
        {
            case 0:
                //유효하지 않음
                break;
            case 1:
            case 3:
                //유효함, 대상 위치에 아무것도 없거나 적 기물이 있음
                SquareList.Add(Instantiate(squarePrefab, calDirection, Quaternion.identity));
                break;
            case 2:
                //유효하지 않음. 대상 위치에 아군 기물이 있음
                break;
            case 4:
                //양파상 움직임일 경우,
                SquareList.Add(Instantiate(squarePrefab, calDirection, Quaternion.identity));
                break;
            default:
                Debug.Log("아무 값이 없음");
                break;
        }        
    }
    /// <summary>
    /// 캐슬링 전용 사각형 함수 2차원 좌표를 받아서 바로 만들어준다.
    /// </summary>
    /// <param name="coords">캐슬링 좌표 (y[z],x)</param>
    public void SquareCreate(Vector3Int coords)
    {
        SquareList.Add(Instantiate(squarePrefab, coords, Quaternion.identity));
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
                    calDirection += new Vector3Int(0, 0, 1);         
                else                
                    calDirection += new Vector3Int(0, 0, -1);
                //endif
            }
            //직선 (옆)
            else if (tempDirection.y == 0)
            {
                if (tempDirection.x < 0)                
                    calDirection += new Vector3Int(1, 0, 0);        
                else                
                    calDirection += new Vector3Int(-1, 0, 0);
                //endif
            }
            //대각선
            else
            {
                if (tempDirection.x < 0 && tempDirection.y < 0)                
                    calDirection += new Vector3Int(1, 0, 1);                
                else if (tempDirection.x > 0 && tempDirection.y < 0)                
                    calDirection += new Vector3Int(-1, 0, 1);                
                else if (tempDirection.x < 0 && tempDirection.y > 0)                
                    calDirection += new Vector3Int(1, 0, -1);                
                else if (tempDirection.x > 0 && tempDirection.y > 0)                
                    calDirection += new Vector3Int(-1, 0, -1);                
            }//end if 
            //유효성 검사 1. 보드 안에서 움직이는 건가? 
            flag = CheckValidSquare(calDirection, tempPiece);
            switch (flag)
            {
                case 0:
                    //유효하지 않음
                    break;
                case 1:
                    //유효함, 대상 위치에 아무것도 없음
                    SquareList.Add(Instantiate(squarePrefab, calDirection, Quaternion.identity));
                    break;
                case 2:
                    //유효하지 않음. 대상 위치에 아군 기물이 있음
                    break;
                case 3:
                    //유효하되, 더 나아갈 수 없음. 대상 위치에 적 기물이 있음. LineCreate 일 경우 더 이상의 반복문을 수행하지 않게 설정 요구. 
                    SquareList.Add(Instantiate(squarePrefab, calDirection, Quaternion.identity));
                    break;
                case 4:
                    //킹일 경우인데, check 검사를 어찌 해야 할지 몰라 더미코드
                    break;
                default:
                    Debug.Log("아무 값이 없음");
                    break;
            }
            if (flag != 1)  //1 제외 break : case 3 은 적 기물에 막혀서 안돼
                break;
        }//endfor
    }//end function 

    private int CheckValidSquare(Vector3Int calDirection, Piece tempPiece)
    {
        if (calDirection.x < 0 || calDirection.x > 7 || calDirection.z < 0 || calDirection.z > 7)
            return 0;
        /*
         *  이동하려는 해당 위치를 기준으로
         *  0. 유효하지 않다. 
         *  1. 아무것도 없다.
         *  2. 아군 기물이 있다.
         *  3. 적군 기물이 있다.
         *  4. 양파상 이동 가능 -> 딱히 구분할 필요는 없는데 어쩌다보니 이렇게 되었음
         *
         *  2-1. Pawn 의 경우 배열이 3개이며, 각 배열엔 기본이동, 좌우 킬좌표  도합 3개의 directions 가 있다. 
         */
        //폰 전용 검사, 양파상
        else if (tempPiece.GetPieceType() == PieceType.Pawn)
        {
            //이동 좌표이면서, 대상 위치에 뭔가 없을 때
            if ((int)tempPiece.transform.position.x == calDirection.x && theBoard.grid[calDirection.z, calDirection.x] == null)
                return 1;
            //이동 좌표가 아니면서, 대상 위치에 뭔가 있을 때, 다른 팀이라면
            else if ((int)tempPiece.transform.position.x != calDirection.x && theBoard.grid[calDirection.z, calDirection.x] != null && !theBoard.TeamCheck(calDirection, tempPiece))
            {
                return 3;
            }
            //이동 좌표가 아니면서, Enpassent 일 경우 
            else if ((int)tempPiece.transform.position.x != calDirection.x  && theBoard.CanEnpassent(calDirection, tempPiece))  //양파상(enpassent) 일 경우
            {
                return 4;   
            }
            else
                return 0;
        }
        //킹 전용 체크검사 
        else if(tempPiece.GetPieceType() == PieceType.King)
        {   //임시함수, 50% 확률로 true 반환 
            if (theBoard.CheckGrid(calDirection))
                return 1;
            else
                return 0;            
        }
        //나머지들의 검사 (이동과 Kill의 위치가 같으며, Check 검사도 필요 없는 Piece 들)
        else
        {
            if (theBoard.grid[calDirection.z, calDirection.x] == null)
                return 1;            
            else if (theBoard.TeamCheck(calDirection,tempPiece))
                return 2;
            else if(!theBoard.TeamCheck(calDirection, tempPiece))  //어떤 다른 경우가 있을까봐      
                return 3;
            
        }
        return 4;   //현재는 킹일 때, 이지만 없음 
    }//endfunction

    

    //그래픽적인 사각형 모두 삭제
    public void SquareClear()
    {
        if (SquareList.Count >= 1)
        {
            foreach (Object n in SquareList)
                Destroy(n);
        }
        SquareList.Clear();
    }
}
