using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    [SerializeField]
    ScrollRect scrollRect;

    [SerializeField]
    RectTransform contentPanel;

    public static UIController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        contentPanel.anchoredPosition =
            (Vector2)scrollRect.transform.InverseTransformPoint(new Vector2(contentPanel.position.x, contentPanel.position.y))
            - (Vector2)scrollRect.transform.InverseTransformPoint(new Vector2(contentPanel.position.x, target.position.y));
    }
}
