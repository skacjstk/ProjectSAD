using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceDatabase : MonoBehaviour
{
    [System.Serializable]
    private class PieceSetup
    {
       public Vector2Int position;
       public PieceType pieceType;
       public TeamColor teamColor;
    }

    [SerializeField] private PieceSetup[] pieceData;

    public int GetPieceSetupCount()
    {
        return pieceData.Length;
    }
    public Vector2Int GetPieceSetupCoord(int idx)
    {
        return pieceData[idx].position;
    }
    public TeamColor GetPieceSetupTeamColor(int idx)
    {
        return pieceData[idx].teamColor;
    }
    public PieceType GetPieceSetupPieceType(int idx)
    {
        return pieceData[idx].pieceType;
    }

}
