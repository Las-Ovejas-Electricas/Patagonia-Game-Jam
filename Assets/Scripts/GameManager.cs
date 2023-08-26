using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

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
    private RectTransform _userInputSelector, _dataSelector;

    private RectTransform _currentSelector;

    private Selectable _userInputItem;
    private Selectable _dataItem;

    [SerializeField]
    private GraphicRaycaster _raycaster;

    [SerializeField]
    private TextMeshProUGUI _dialogBoxTMP;
    PointerEventData m_PointerEventData;

    public Transform TrashCan;

    [HideInInspector]
    public List<GameObject> CurrentDocuments;

    [SerializeField]
    private AudioSource _inspectSound, _analyzeSound, _discrepancySound;

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
            if (result.gameObject.TryGetComponent(out Selectable selectable))
            {
                _inspectSound.Play();
                _currentSelector = selectable.Source == Source.UserInput ? _userInputSelector : _dataSelector;
                RectTransform rect = result.gameObject.GetComponent<RectTransform>();

                _currentSelector.gameObject.SetActive(_currentSelector != rect);
                if(_currentSelector == rect) return;

                if (selectable.Source == Source.UserInput) _userInputItem = selectable;
                else _dataItem = selectable;

                _currentSelector.SetParent(rect);
                _currentSelector.anchoredPosition = Vector2.zero;
                _currentSelector.anchorMin = Vector2.zero;
                _currentSelector.anchorMax = Vector2.one;
                _currentSelector.pivot = new Vector2(0.5f, 0.5f);
                _currentSelector.offsetMin = Vector2.zero;
                _currentSelector.offsetMax = Vector2.zero;
                _currentSelector.sizeDelta = Vector2.zero;

                if(_dataItem != null && _userInputItem != null) 
                {
                    StartCoroutine(CompareInformation());
                }
            }
        }
    }

    private IEnumerator CompareInformation()
    {
        _analyzeSound.Play();
        _inputActions.Player.Disable();
        yield return new WaitForSeconds(_analyzeSound.clip.length);
        if (_dataItem.DataType == _userInputItem.DataType)
        {
            if (_dataItem.Value != _userInputItem.Value)
            {
                _discrepancySound.Play();
                _dialogBoxTMP.text = "Ai dispreponcias";
            }else
            {
                _dialogBoxTMP.text = "Ta biem";
            }
        }else
        {
            _dialogBoxTMP.text = "Los tipos de datos no comsiden";
        }
        _dataItem = null;
        _userInputItem = null;
        _dataSelector.gameObject.SetActive(false);
        _userInputSelector.gameObject.SetActive(false);
        _inputActions.Player.Enable();
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
