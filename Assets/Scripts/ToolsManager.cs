using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tools
{
    Dryer,
    Grower,
    Scissors
}

[Serializable]
struct ToolsDisctionaryEntry
{
    public Tools toolName;
    public AbstractTool toolScript;
}


public class ToolsManager : MonoBehaviour
{
    [SerializeField] List<ToolsDisctionaryEntry> _toolsList= new List<ToolsDisctionaryEntry>();
    AbstractTool _currentTool;
    private void OnEnable()
    {
        DragAndDropController.ToolChoosenEvent += OnToolChosen;
    }

    private void OnDisable()
    {
        DragAndDropController.ToolChoosenEvent -= OnToolChosen;
    }

    private void OnToolChosen(GameObject obj)
    {
        if(obj == null)
        {
            _currentTool.Return();
            _currentTool = null;
            return;
        }
        _currentTool = obj.GetComponent<AbstractTool>();
        if(_currentTool == null)
        {
            throw new Exception("Error in tools manager: Not a tool");
        }
        _currentTool.Take();
    }
}
