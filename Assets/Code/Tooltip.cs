using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// [ExecuteInEditMode]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;
    public LayoutElement layoutElement;
    public int characterWrapLimit;
    public Vector2 padding;

    public RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        Hide();
    }

    public void SetHeader(string newHeader)
    {
        headerField.text = newHeader;
    }

    public void SetContent(string newContent)
    {
        contentField.text = newContent;
    }

    public void AddContent(string newContent)
    {
        contentField.text += System.Environment.NewLine + newContent;
    }

    private void Update()
    {
        int headerLength = headerField.text.Length;
        int contentLength = headerField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;

        rectTransform.pivot = new Vector2(0, 1);

        Vector2 position = Input.mousePosition;

        float tooltipWidth = rectTransform.rect.width;
        float tooltipHeight = rectTransform.rect.height;

        // if (position.x + tooltipWidth + padding.x > Screen.width)
        // {
        //     position.x = Screen.width - tooltipWidth;
        // }

        // if (position.x < 0)
        // {
        //     position.x = 0;
        // }

        // if (position.y - tooltipHeight + padding.y < 0)
        // {
        //     position.y = tooltipHeight - padding.y;
        // }

        // if (position.y + padding.y > Screen.height)
        // {
        //     position.y = Screen.height - padding.y;
        // }

        position += padding;

        transform.position = position;
    }

    public void Show()
    {
        transform.gameObject.SetActive(true);
    }

    public void Hide()
    {
        transform.gameObject.SetActive(false);
    }
}
