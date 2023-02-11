using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairController : MonoBehaviour
{
    [SerializeField] Transform _hairStart;
    [SerializeField] Transform _hairEnd;
    [SerializeField] float _maxRelativeAngle;
    [SerializeField] GameObject _rotationParent;
    //[SerializeField] GameObject _maskParent;

    float _growRate = 0.04f;
    float originalLocalAngle;
    float currentRotationAngle;
    float _originalHairLength;
    float _originalScale;
    //float _maskOriginalYposition;

    [SerializeField] HairManager _manager;

    private void Start()
    {
        originalLocalAngle = _rotationParent.transform.rotation.eulerAngles.z;
        currentRotationAngle = originalLocalAngle;
        print("distance " + Vector3.Distance(_hairStart.position, _hairEnd.position));
        _originalHairLength = Vector3.Distance(_hairStart.position, _hairEnd.position);
        _originalScale = transform.lossyScale.x;
        //_maskOriginalYposition = _maskParent.transform.position.y;

    }

    public void OnDryerPositionUpdated(float dryerPosition)
    {
        float distance = Mathf.Abs(dryerPosition - _hairStart.position.y);
        float bValue = Remap(distance, 0, 10, _maxRelativeAngle, 0);
        _rotationParent.transform.localRotation = Quaternion.Euler(0, 0, originalLocalAngle + bValue);
    }

    float Remap(float value, float minOriginalVal, float maxOriginalVal, float minRangeVal, float maxRangeVal)
    {
        return minRangeVal + (value - minOriginalVal) * (maxRangeVal - minRangeVal) / (maxOriginalVal - minOriginalVal);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_manager.CurrentToolType == Tools.Scissors)
        {
            Cut( collision.transform.position);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_manager.CurrentToolType == Tools.Grower)
        {
            Grow();
        }
    }

    public void Grow()
    {
        /*        if (_maskParent.transform.position.y < _maskOriginalYposition)
                {
                    _maskParent.transform.position += Vector3.up*_growRate;
                }*/
        float newScale = Mathf.Min(transform.localScale.x + _growRate,_originalScale);
        transform.localScale = new Vector3(newScale, _originalScale, _originalScale);

    }

    public void Cut(Vector3 scissorsYPosition)
    {
        /*        if(_maskParent.transform.position.y > scissorsYPosition)
                {
                    _maskParent.transform.localPosition = new Vector3(0, scissorsYPosition, 0);
                }*/
        float distanceToCutPoint = FindDistanceToNearestPointOnLine(_hairStart.position, _hairEnd.position - _hairStart.position, scissorsYPosition);
        print(distanceToCutPoint);
        float newScale = Remap(distanceToCutPoint, 0, _originalHairLength, 0.1f, _originalScale);
        transform.localScale = new Vector3(Mathf.Min(newScale, transform.localScale.x), _originalScale, _originalScale);
    }

    public float FindDistanceToNearestPointOnLine(Vector3 origin, Vector3 direction, Vector3 point)
    {
        direction.Normalize();
        Vector3 lhs = point - origin;
        return Mathf.Clamp(Vector3.Dot(lhs, direction), 0, Vector3.Distance(_hairStart.position, _hairEnd.position));
    }

}
