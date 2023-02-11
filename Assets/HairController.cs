using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairController : MonoBehaviour
{
    float _currentLength = 1;
    [SerializeField] Transform _hairStart;
    [SerializeField] Transform _hairEnd;
    [SerializeField] float _maxRelativeAngle;
    [SerializeField] GameObject _rotationParent;

    [SerializeField] float test;
    [SerializeField] bool update;

    float originalLocalAngle;
    float currentRotationAngle;

    private void Start()
    {
        originalLocalAngle = _rotationParent.transform.rotation.eulerAngles.z;
        currentRotationAngle = originalLocalAngle;
    }

    public void OnDryerPositionUpdated(float dryerPosition)
    {
        float distance = Mathf.Abs(dryerPosition - _hairStart.position.y);//Vector3.Distance(dryerPosition, _hairStart.position);
        //float normal = Mathf.InverseLerp(0, 10, distance);
        //float bValue = Mathf.Lerp(_maxRelativeAngle, 0, normal);
        float bValue = Remap(distance, 0, 10, _maxRelativeAngle, 0);
        _rotationParent.transform.localRotation = Quaternion.Euler(0, 0, originalLocalAngle + bValue);
       // transform.Rotate(Vector3.forward, bValue);
    }

/*    public void Update()
    {
        if (update)
        {
            update = false;
            OnDryerPositionUpdated(test);
        }
    }*/

    float Remap(float value, float minOriginalVal, float maxOriginalVal, float minRangeVal, float maxRangeVal)
    {
        return minRangeVal + (value - minOriginalVal) * (maxRangeVal - minRangeVal) / (maxOriginalVal - minOriginalVal);
    }



}
