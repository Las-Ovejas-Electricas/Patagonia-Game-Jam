using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private PlayerInputActions _inputActions;
    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(this);
            return;
        }

        _inputActions = new PlayerInputActions();
        _inputActions.Player.Click.performed += ShootRaycast;

    }

    [SerializeField]
    private RectTransform _selector;

    [SerializeField]
    private GraphicRaycaster _raycaster;
    PointerEventData m_PointerEventData;

    private void ShootRaycast(InputAction.CallbackContext context)
    {
        m_PointerEventData = new(null)
        {
            position = Mouse.current.position.ReadValue()
        };
        List<RaycastResult> results = new();
        _raycaster.Raycast(m_PointerEventData, results);

        if(results.Count > 0)
        {
            RaycastResult result = results[0];
            if(result.gameObject.TryGetComponent<Selectable>(out _))
            {
                RectTransform rect = result.gameObject.GetComponent<RectTransform>();

                _selector.SetParent(rect);
                _selector.anchoredPosition = Vector2.zero;
                _selector.anchorMin = Vector2.zero;
                _selector.anchorMax = Vector2.one;
                _selector.pivot = new Vector2(0.5f, 0.5f);
                _selector.offsetMin = Vector2.zero;
                _selector.offsetMax = Vector2.zero;
                _selector.sizeDelta = Vector2.zero;
            }
        }
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
    }
}
