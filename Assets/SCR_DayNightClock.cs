using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_DayNightClock : MonoBehaviour
{
    private float rotation;
    private RectTransform rectTransform;

    // Update is called once per frame
    void Update()
    {
        GetRotationComponent();
        rotation += Time.deltaTime * 6;
    }

    private void GetRotationComponent()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0, 0, rotation);
    }
}
