using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairManager : MonoBehaviour
{
    [SerializeField] List<HairController> _hairsList;
    Tools _currentToolType = Tools.None;
    GameObject _currentTool;

    public Tools CurrentToolType { get => _currentToolType; }

    private void OnEnable()
    {
        ToolsManager.ToolChoosenEvent += OnToolChosen;
    }

    private void OnToolChosen(Tools toolName, GameObject tool)
    {
        _currentToolType = toolName;
        _currentTool = tool;
        switch (_currentToolType)
        {
            case Tools.Dryer:
                break;
            case Tools.Grower:
                break;
            case Tools.Scissors:
                break;
            case Tools.None:

                break;
            default:
                break;
        }
    }

    private void OnDisable()
    {
        ToolsManager.ToolChoosenEvent -= OnToolChosen;
    }

    void Update()
    {
        if(_currentToolType == Tools.Dryer)
        {
            Vector3 dryerPos = _currentTool.transform.position;
            foreach ( HairController hair in _hairsList)
            {
                hair.OnDryerPositionUpdated(dryerPos.y);
            }
            float angle = Vector3.SignedAngle(Vector3.down, dryerPos-transform.position,Vector3.forward);
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
