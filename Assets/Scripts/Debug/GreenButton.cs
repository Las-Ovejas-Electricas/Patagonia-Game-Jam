using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GreenButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(DestroyDocuments());
    }

    private IEnumerator DestroyDocuments()
    {
        foreach (GameObject document in GameManager.instance.CurrentDocuments)
        {
            LeanTween.move(document, GameManager.instance.TrashCan.position, 0.5f).setEaseOutCubic();
            LeanTween.scale(document, Vector3.zero, 0.5f).setEaseOutCubic().setOnComplete(() =>
            {
                Destroy(document);
            });
            yield return new WaitForSeconds(0.1f);
        }
        GameManager.instance.CurrentDocuments.Clear();
    }
}
