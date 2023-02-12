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

    /// <summary>
    /// I am not using the mask for now, but I left it here to show that it can be done in a different way
    /// </summary>
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
        _originalHairLength = Vector3.Distance(_hairStart.position, _hairEnd.position);
        _originalScale = transform.lossyScale.x;
        //_maskOriginalYposition = _maskParent.transform.position.y;

    }
    /// <summary>
    /// This function will use the distance along the y axis between the hair root and the dryer, to rotate the hair between 0 degrees to _maxRelativeAngle
    /// </summary>
    /// <param name="dryerPosition"></param>
    public void OnDryerPositionUpdated(float dryerPosition)
    {
        float distance = Mathf.Abs(dryerPosition - _hairStart.position.y);
        float bValue = Remap(distance, 0, 10, _maxRelativeAngle, 0);
        _rotationParent.transform.localRotation = Quaternion.Euler(0, 0, originalLocalAngle + bValue);
    }

    /// <summary>
    /// This function will remap a value between minOriginalval to maxOriginalval, to a range between minRangeVal and maxRangeVal
    /// </summary>
    /// <param name="value"></param>
    /// <param name="minOriginalVal"></param>
    /// <param name="maxOriginalVal"></param>
    /// <param name="minRangeVal"></param>
    /// <param name="maxRangeVal"></param>
    /// <returns></returns>
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
    /// <summary>
    /// This function will grow a hair in _growRate rate, to the maximum original scale
    ///  (The commented lines were used when I implemented it using masks, but eventually I decided to implement it the same way as the example)   
    /// </summary>
    public void Grow()
    {
        /*        if (_maskParent.transform.position.y < _maskOriginalYposition)
                {
                    _maskParent.transform.position += Vector3.up*_growRate;
                }*/
        float newScale = Mathf.Min(transform.localScale.x + _growRate,_originalScale);
        transform.localScale = new Vector3(newScale, _originalScale, _originalScale);

    }
    /// <summary>
    ///   This function will cut a hair.
    ///  (The commented lines were used when I implemented it using masks, but eventually I decided to implement it the same way as the example)   
    /// </summary>
    /// <param name="scissorsPosition"></param>
    public void Cut(Vector3 scissorsPosition)
    {
        /*        if(_maskParent.transform.position.y > scissorsYPosition)
                {
                    _maskParent.transform.localPosition = new Vector3(0, scissorsYPosition, 0);
                }*/
        //find closest point to scissors, and cut there
        float distanceToCutPoint = FindDistanceToNearestPointOnLine(_hairStart.position, _hairEnd.position - _hairStart.position, scissorsPosition);
        //calculate new scale using remapping the distance to a scaling value 
        float newScale = Remap(distanceToCutPoint, 0, _originalHairLength, 0.1f, _originalScale);
        //rescale, but check that the new scale is smaller then the current scale
        transform.localScale = new Vector3(Mathf.Min(newScale, transform.localScale.x), _originalScale, _originalScale);
    }

    /// <summary>
    /// This function will find the distance between the origin of a line , to a point along its direction which is the closest to a given point.
    /// This function makes sure that the closest point is on the line (between start and end of the hair)
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="direction"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    public float FindDistanceToNearestPointOnLine(Vector3 origin, Vector3 direction, Vector3 point)
    {
        direction.Normalize();
        Vector3 lhs = point - origin;
        return Mathf.Clamp(Vector3.Dot(lhs, direction), 0, Vector3.Distance(_hairStart.position, _hairEnd.position));
    }

}
