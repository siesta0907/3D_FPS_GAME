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
        // 부모 객체의 크기에 맞춰 HUD 위치 조정
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = Vector2.zero;
    }
}
