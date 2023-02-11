using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scissors : AbstractTool
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _toolName = Tools.Scissors;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
