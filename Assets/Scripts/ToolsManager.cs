using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tools
{
    Dryer,
    Grower,
    Scissors,
    None,
}


public class ToolsManager : MonoBehaviour
{
    AbstractTool _currentTool;
    public static Action<Tools, GameObject> ToolChoosenEvent; 
    private void OnEnable()
    {
        DragAndDropController.DragedObjectUpdateEvent += OnToolChosen;
    }

    private void OnDisable()
    {
        DragAndDropController.DragedObjectUpdateEvent -= OnToolChosen;
    }

    private void OnToolChosen(GameObject obj)
    {
        if(obj == null)
        {
            _currentTool.Return();
            _currentTool = null;
            ToolChoosenEvent.Invoke(Tools.None, null);
            return;
        }
        _currentTool = obj.GetComponent<AbstractTool>();
        if(_currentTool == null)
        {
            throw new Exception("Error in tools manager: Not a tool");
        }
        _currentTool.Take();
        ToolChoosenEvent.Invoke(_currentTool.ToolName, _currentTool.gameObject);
    }
}
