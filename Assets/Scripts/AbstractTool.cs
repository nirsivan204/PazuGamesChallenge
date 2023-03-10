using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractTool : MonoBehaviour
{
    Vector3 _toolnitialPlace;
    protected Tools _toolName;

    public Tools ToolName { get => _toolName;  }

    protected virtual void Start()
    {
        _toolnitialPlace = transform.position;
    }

    public virtual void Take() 
    {

    }

    public virtual void Return()
    {
        transform.position = _toolnitialPlace;
    }


}
