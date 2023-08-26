using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class RedButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject[] _documents;

    [SerializeField]
    private Transform _spawnPosition, _targetPosition;
    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(SpawnDocuments());
    }

    private IEnumerator SpawnDocuments()
    {
        foreach (GameObject document in _documents)
        {
            GameObject doc = Instantiate(document, _spawnPosition.position, Quaternion.identity, transform.parent);
            GameManager.instance.CurrentDocuments.Add(doc);
            LeanTween.move(doc, _targetPosition.position, 0.25f).setEaseOutCubic();
            yield return new WaitForSeconds(0.2f);
        }
    }
}
