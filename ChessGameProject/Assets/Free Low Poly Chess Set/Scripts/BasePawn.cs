using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TestDebug();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void TestDebug()
    {
        Debug.Log("Hello world");
    }
}
