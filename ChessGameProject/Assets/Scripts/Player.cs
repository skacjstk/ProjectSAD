using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//카메라 좌표
// 블랙: 3.5, 8 ,8  yRotation 180
// 화이트: 3.5, 8, -1, yRotation 0
public class Player : ColliderHandler
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SelectPiece();
        }
    }



}
