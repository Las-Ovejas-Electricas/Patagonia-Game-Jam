using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Draggable : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public float speed = 1.0f;

    private bool onTable = false;
    private bool dragging = false;
    private bool pointerDown = false;
    private Vector2 previousMousePosition;
    private Vector2 mousePosition;

    private Vector2 previousPosition;
    private bool isMoving;

    private void Update()
    {
        isMoving = previousPosition != (Vector2)transform.position;
        previousPosition = transform.position;
        mousePosition = Mouse.current.position.ReadValue();

        if (dragging) OnDrag();
        if (Vector2.Distance(previousMousePosition, mousePosition) > 15)
        {
            if (pointerDown)
            {
                dragging = true;
                ScaleCard(1.1f);
                onTable = !onTable;
            }
        }
        if(!pointerDown)
        {
            previousMousePosition = mousePosition;
        }
    }

    private void Awake()
    {
        mousePosition = transform.position;
    }

    public void OnDrag()
    {
        float distance = Vector2.Distance(transform.position, mousePosition);
        if (distance > 0.1f)
        {
            float currentSpeed = speed * distance;
            transform.position = Vector2.MoveTowards(transform.position, mousePosition, currentSpeed * Time.deltaTime);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ScaleCard(1f, true);
        dragging = false;
        pointerDown = false;
        onTable = !onTable;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        LeanTween.cancel(gameObject);
        pointerDown = true;
    }

    private void ScaleCard(float size, bool cancelTweens = false, float time = 0.3f)
    {
        if (cancelTweens) LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one * size, time).setEaseOutCubic();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //HoverCard(1.05f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //HoverCard(1f);
    }

    //private void HoverCard(float size)
    //{
    //    if (!onTable && !dragging)
    //    {
    //        if (!isMoving) LeanTween.cancel(gameObject);
    //        ScaleCard(size);
    //    }
    //}
}
