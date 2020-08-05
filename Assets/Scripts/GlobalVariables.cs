using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public int maxDropSize = 2;
    public int currDropSize;
    public int maxHandSize = 6;
    public int currP1HandSize = 0;
    public int currP2HandSize = 0;

    void Awake()
    {
        currDropSize = 0;
        currP1HandSize = 0;
        currP2HandSize = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
