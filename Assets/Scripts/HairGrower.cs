using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairGrower : AbstractTool
{
    protected override void Start()
    {
        base.Start();
        _toolName = Tools.Grower;
    }
}
