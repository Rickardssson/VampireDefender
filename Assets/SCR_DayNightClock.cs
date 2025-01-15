using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_DayNightClock : MonoBehaviour
{
    [SerializeField] private float StartRotation = 0;
    private float rotation;
    private RectTransform rectTransform;

    private void Start()
    {
        rotation = StartRotation;
        GetRotationComponent();
    }

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
