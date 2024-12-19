using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class SCR_UI_Fade 
{
    public static void FadeIn(this Graphic g, float duration)
    {
        if (g == null)
        {
            Debug.Log("Graphic is null.");
            return;
        }

        CanvasRenderer canvasRenderer = g.GetComponent<CanvasRenderer>();
        if (canvasRenderer == null)
        {
            Debug.Log("CanvasRenderer component is missing on the Graphic.");
            return;
        }

        canvasRenderer.SetAlpha(0f);
        g.CrossFadeAlpha(1f, duration, false); // Second param is the time
    }

    public static void FadeOut(this Graphic g, float duration)
    {
        if (g == null)
        {
            Debug.Log("Graphic is null.");
            return;
        }

        CanvasRenderer canvasRenderer = g.GetComponent<CanvasRenderer>();
        if (canvasRenderer == null)
        {
            Debug.Log("CanvasRenderer component is missing on the Graphic.");
            return;
        }

        canvasRenderer.SetAlpha(1f);
        g.CrossFadeAlpha(0f, duration, false);
    }
}
