using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System;
using System.Xml.Linq;
using UnityEngine.Windows;
using System.Linq;
using static UnityEngine.InputSystem.UI.VirtualMouseInput;

public class GameManager : MonoBehaviour
{
    private int _score;

    [SerializeField]
    private Texture2D _pointerCursor;

    [SerializeField]
    private GameObject _ending;

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

    public Transform _trashCan;

    private List<GameObject> _currentDocuments = new();

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

    public void SetPointerCursor(bool pointer)
    {
        Cursor.SetCursor(pointer ? _pointerCursor : null, Vector2.zero, UnityEngine.CursorMode.Auto);
    }

    private IEnumerator CompareInformation()
    {
        _analyzeSound.Play();
        _inputActions.Player.Disable();
        yield return new WaitForSeconds(_analyzeSound.clip.length);
        if (_dataItem.DataType == _userInputItem.DataType)
        {
            if(_dataItem.DataType == DataType.Coverage)
            {
                string[] coverage = _dataItem.Value.Split(new[] { ", " }, StringSplitOptions.None);
                if (coverage.Contains(_userInputItem.Value)) OkData();
                else DiscrepanciesFound();
            }
            else
            {
                if (_dataItem.Value != _userInputItem.Value)DiscrepanciesFound();
                else OkData();
            }
        }else
        {
            _dialogBoxTMP.text = "Los tipos de datos no coinciden";
        }
        _dataItem = null;
        _userInputItem = null;
        _dataSelector.gameObject.SetActive(false);
        _userInputSelector.gameObject.SetActive(false);
        _inputActions.Player.Enable();
    }

    private void DiscrepanciesFound()
    {
        _discrepancySound.Play();
        _dialogBoxTMP.text = "Se han encontrado discrepancias";
        StartCoroutine(DestroyDocuments());
    }

    private void OkData()
    {
        _dialogBoxTMP.text = "Datos coincidientes";
    }

    #region Client Sequence
    [Serializable]
    public class Client
    {
        public GameObject[] Documentation;
        public PolicySO Policy;
    }

    [Space][Header("Client Sequence")]
    [SerializeField]
    private List<Client> _clients;

    [SerializeField]
    private InsuranceFile _policy;
    [SerializeField]
    private Webcam _webcam;

    [SerializeField]
    private Transform _spawnPosition, _targetPosition;
    public void StartClientSequence()
    {
        if(_clients.Count == 0)
        {
            _ending.SetActive(true);
            return;
        }
        StartCoroutine(SpawnDocuments(_clients[0]));
    }

    private IEnumerator SpawnDocuments(Client client)
    {
        _policy.PolicySO = client.Policy;
        _webcam.PolicySO = client.Policy;
        foreach (GameObject document in client.Documentation)
        {
            GameObject doc = Instantiate(document, _spawnPosition.position, Quaternion.identity, transform);
            _currentDocuments.Add(doc);
            LeanTween.move(doc, _targetPosition.position, 0.25f).setEaseOutCubic();
            yield return new WaitForSeconds(0.2f);
        }
        _policy.SetWindowState(true);
        _webcam.SetWindowState(true);
        _clients.RemoveAt(0);
    }

    private IEnumerator DestroyDocuments()
    {
        _inputActions.Player.Disable();
        yield return new WaitForSeconds(1.5f);
        _userInputSelector.SetParent(null);
        _dataSelector.SetParent(null);
        foreach (GameObject document in _currentDocuments)
        {
            if(document.TryGetComponent(out Draggable draggable)){
                draggable.enabled = false;
            }
            LeanTween.move(document, _trashCan.position, 0.5f).setEaseOutCubic();
            LeanTween.scale(document, Vector3.zero, 0.5f).setEaseOutCubic().setOnComplete(() =>
            {
                Destroy(document);
            });
            yield return new WaitForSeconds(0.1f);
        }
        _policy.SetWindowState(false);
        _webcam.SetWindowState(false);
        _currentDocuments.Clear();
        _inputActions.Player.Enable();
        _dialogBoxTMP.text = "";
        Invoke(nameof(StartClientSequence), 1f);
    }
    #endregion

    private void OnEnable()
    {
        _inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
    }
}
