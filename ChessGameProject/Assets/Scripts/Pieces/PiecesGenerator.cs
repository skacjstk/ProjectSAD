using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesGenerator : MonoBehaviour
{
    ChessGameController theChessGameController;
    [SerializeField] private GameObject[] piecesPrefabs;
    private PieceDatabase thePieceDatabase;
    private Dictionary<PieceType, GameObject> nameToPieceDict = new Dictionary<PieceType,GameObject>();
    [SerializeField] private Material[] WhiteMaterials;
    [SerializeField] private Material[] BlackMaterials;
    Renderer meshRenderer;

    //사전에 Piece 정보 초기화 
    private void Awake()
    {
        foreach (var piece in piecesPrefabs)
        {
            nameToPieceDict.Add(piece.GetComponent<Piece>().GetPieceType(), piece);
        }
        thePieceDatabase = transform.GetChild(0).GetComponent<PieceDatabase>();
        theChessGameController = FindObjectOfType<ChessGameController>();
    }

    public GameObject CreatePiece(PieceType type)
    {
        Debug.Log("기물창조 시작");
        GameObject go_piece;
        go_piece = nameToPieceDict[type];
        if (go_piece)    //받아오기 성공 시 
        {
            GameObject newPiece = Instantiate(go_piece);
            return newPiece;
        }
        else
        {
            Debug.LogError("팀 오류: TeamColor.White 와 TeamColor.Black 만 가능합니다");
        }
        return null;
    }
    public void InitializePieces()
    {
        //thePieceDatabase 의 pieceData 를 기반으로
        //객체 CreatePiece 하고, 좌표를 고치기 
        GameObject tempObject;
        Debug.Log("반복 갯수" + thePieceDatabase.GetPieceSetupCount());
        for(int i=0;i < thePieceDatabase.GetPieceSetupCount(); ++i)
        {
            tempObject = CreatePiece(thePieceDatabase.GetPieceSetupPieceType(i));
            SetMaterial(thePieceDatabase.GetPieceSetupPieceType(i), thePieceDatabase.GetPieceSetupTeamColor(i), ref tempObject);
            tempObject.transform.position = CalculateCoordsToPosition(thePieceDatabase.GetPieceSetupCoord(i));
            //gird[y][x] 에  객체의 Piece 보내기 
            theChessGameController.InitializeGrid(tempObject.transform.position, tempObject);
        }        
    }

    public void SetMaterial(PieceType type, TeamColor team, ref GameObject tempObject)
    {
        meshRenderer = tempObject.GetComponent<Renderer>();

        if(team == TeamColor.White) { 
            meshRenderer.material = WhiteMaterials[(int)type];
            tempObject.gameObject.GetComponent<Piece>().team = TeamColor.White;
        }
        else if(team == TeamColor.Black) {
            meshRenderer.material = BlackMaterials[(int)type];
            tempObject.gameObject.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
            tempObject.gameObject.GetComponent<Piece>().team = TeamColor.Black;
        }
    }

    //Board 에 있는거 그냥 뜯어온 거 
    public Vector3Int CalculateCoordsToPosition(Vector2Int coords)
    {
        return new Vector3Int(coords.x, 0, coords.y);
    }
    //Vector3 또는 Vector3Int 좌표를 Vector2Int 좌표로 바꾸는 코드
    public Vector2Int CalculatePositionToCoords(Vector3 coords)
    {
        return new Vector2Int((int)coords.x, (int)coords.z);
    }
}
