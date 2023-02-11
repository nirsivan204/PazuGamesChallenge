using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scissors : AbstractTool
{
    [SerializeField] Animator _anim;
    protected override void Start()
    {
        base.Start();
        _toolName = Tools.Scissors;
    }

    public override void Take()
    {
        _anim.SetTrigger("ToggleScissors");
        transform.rotation = Quaternion.Euler(0, 0, 90);
    }

    public override void Return()
    {
        base.Return();
        _anim.SetTrigger("ToggleScissors");
        transform.rotation = Quaternion.identity;
    }

}
