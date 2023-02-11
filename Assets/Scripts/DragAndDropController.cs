using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;


public class DragAndDropController : MonoBehaviour
{
    [SerializeField] InputAction clickAction;
    [SerializeField] InputAction holdAction;
    [SerializeField] float _drag = 0.1f;
    [SerializeField] float _leftHorizontalBorder;
    [SerializeField] float _righHorizontalBoarder;
    [SerializeField] float _upVerticalBorder;
    [SerializeField] float _downVerticalBoarder;
    private Camera mainCamera;
    private Vector2 velocity = Vector2.zero;
    private GameObject _toolHolding = null;

    public static Action<GameObject> ToolChoosenEvent;
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        clickAction.Enable();
        holdAction.Enable();
        clickAction.performed += OnClick;
        //GameManager.LevelEndEvent += OnLevelEnded;

    }

/*    private void OnLevelEnded(LevelResult level)
    {
        clickAction.Disable();
        holdAction.Disable();
    }*/

    private void OnDisable()
    {
        clickAction.performed -= OnClick;
        clickAction.Disable();
        holdAction.Disable();
        //GameManager.LevelEndEvent -= OnLevelEnded;

    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if(_toolHolding == null)
        {
            Ray ray = mainCamera.ScreenPointToRay(context.ReadValue<Vector2>());
            Collider2D hitCollider = Physics2D.GetRayIntersection(ray).collider;
            if (hitCollider != null && hitCollider.tag == "Tool")
            {
                _toolHolding = hitCollider.gameObject;
                ToolChoosenEvent.Invoke(hitCollider.gameObject);
                StartCoroutine(DragCoroutine(hitCollider.gameObject));
            }
        }
    }

    private IEnumerator DragCoroutine(GameObject dragable)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        while (holdAction.ReadValue<TouchPhase>() != TouchPhase.Ended)
#elif UNITY_STANDALONE || UNITY_EDITOR
        while (holdAction.ReadValue<float>() != 0)
#endif
        {
            Vector2 PointerPos = mainCamera.ScreenToWorldPoint(clickAction.ReadValue<Vector2>());
            print(PointerPos);

            float horizontalPointerPos = Mathf.Clamp(PointerPos.x, _leftHorizontalBorder, _righHorizontalBoarder);
            float VerticalPointerPos = Mathf.Clamp(PointerPos.y, _downVerticalBoarder, _upVerticalBorder);

            Vector2 currentPos = dragable.transform.position;
            Vector2 targetPos = new Vector2(horizontalPointerPos, VerticalPointerPos);
            //print(targetPos);
            dragable.transform.position = Vector2.SmoothDamp(currentPos, targetPos, ref velocity, _drag);
            yield return null;
        }
        _toolHolding = null;
        ToolChoosenEvent.Invoke(null);
    }

}