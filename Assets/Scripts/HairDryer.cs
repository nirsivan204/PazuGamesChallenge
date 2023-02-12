using System.Collections;
using UnityEngine;

public class HairDryer : AbstractTool
{
    bool isFlipped = false;
    bool isTaken = false;
    float _originalRotationAngle;

    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] HairManager HairManager;

    protected override void Start()
    {
        base.Start();
        _toolName = Tools.Dryer;
        _originalRotationAngle = transform.rotation.eulerAngles.z;
    }

    IEnumerator UpdateRotation()
    {
        while (isTaken)
        {
            if ((transform.position.x > 0 && !isFlipped) ||
                (transform.position.x < 0 && isFlipped))
            {
                flip();
            }
            LookAt2D(HairManager.transform.position);
            yield return null;
        }
    }

    void LookAt2D(Vector3 position)
    {
        float angle = Vector3.SignedAngle(Vector3.up, position - transform.position,Vector3.forward);
        transform.rotation = Quaternion.Euler(0, isFlipped?180:0, angle* (isFlipped ? -1 : 1));
    }

    void flip()
    {
        isFlipped = !isFlipped;
    }

    public override void Take()
    {
        base.Take();
        isTaken = true;
        StartCoroutine(UpdateRotation());
    }

    public override void Return()
    {
        base.Return();
        isTaken = false;
        transform.rotation = Quaternion.Euler(0, 0, _originalRotationAngle);
    }

}
