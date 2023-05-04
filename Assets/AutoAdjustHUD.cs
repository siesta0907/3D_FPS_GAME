using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAdjustHUD : MonoBehaviour
{
    // Start is called before the first frame update
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void start()
    {
        OnRectTransformDimensionsChange();
    }
    private void OnRectTransformDimensionsChange()
    {
        // �θ� ��ü�� ũ�⿡ ���� HUD ��ġ ����
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = Vector2.zero;
    }
}
