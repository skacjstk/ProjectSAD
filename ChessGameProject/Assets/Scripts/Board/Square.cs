using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    //이거 근데 안써, 걍 tr.position 하면 받아와지다보니까 
    public Vector3 GetSquarePosition()
    {
        return this.transform.position;
    }
}
