using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

public class NumberScript : MonoBehaviour
{

    public int number;

    UnityEngine.Vector2 pos;

    public TextMeshPro textMesh;

    

    // Start is called before the first frame update
    public NumberScript(UnityEngine.Vector2 pos, int number)
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int CompareTo(NumberScript other)
        {

        if (other == null)
        {
            return 1;

        }

        return 0;
    }
}
