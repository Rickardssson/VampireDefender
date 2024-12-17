using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class SCR_DayNightCycle : MonoBehaviour
{
    public TextMeshProUGUI timeDisplay; 
    public TextMeshProUGUI dayDisplay; 
    public Volume ppv; 
    
    private float timeAccumulator; 
    private int totalSeconds; 
    private int seconds; 
    private int mins;
    public int hours; 
    private int days;

    [SerializeField] private float dayLengthInMinutes = 2f;
    [SerializeField] private float lightIntensity = 30f;

    public float timeScale;
    public bool activateLights;
    public List<GameObject> lights = new List<GameObject>();

    private void Start()
    {
        timeScale = 86400 / (dayLengthInMinutes * 60);
        
        if (ppv == null)
        {
            // Try to find the Volume in the entire scene
            ppv = FindObjectOfType<Volume>();

            if (ppv == null)
            {
                Debug.LogError("No Volume component found in the scene!");
            }
        }
        
        InvokeRepeating(nameof(DetectAndAddLights), 0.1f, 0.05f);
        InvokeRepeating(nameof(CleanupLights), 2f, 2f);
    }

    private void Update()
    {
        timeAccumulator += Time.deltaTime * timeScale;
        
        if (timeAccumulator >= 1f)
        {
            totalSeconds += Mathf.FloorToInt(timeAccumulator); 
            timeAccumulator %= 1f;
        }
        
        CalcTime();
        DisplayTime();
        ControlPPV();
    }

    public void AddLight(GameObject light)
    {
        if (!lights.Contains(light))
        {
            lights.Add(light);
            /*Debug.Log($"Added light: {light.name}");*/
            
            Light2D light2D = light.GetComponent<Light2D>();
            if (light2D == null) return;
            
            if(hours >= 6 && hours < 21)
            {
               light.SetActive(true);
               StartCoroutine(FadeLight(light, 0f, 0.1f));
            }
            else
            {
                light.SetActive(true);
                StartCoroutine(FadeLight(light, lightIntensity, 0.1f));
            }
        }
    }

    private IEnumerator FadeLight(GameObject lightObject, float targetIntensity, float duration)
    {
        if (lightObject == null) yield break;
        
        Light2D light2D = lightObject.GetComponent<Light2D>();
        if (light2D == null) yield break;
        
        float startIntensity = light2D.intensity;
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            light2D.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / duration);
            yield return null;
        }
        
        light2D.intensity = targetIntensity;

        if (targetIntensity == 0f)
        {
            lightObject.SetActive(false);
        }
    }
    
    private void CleanupLights()
    {
        lights.RemoveAll(light => light == null);
    }
    
    private void DetectAndAddLights()
    {
        Light2D[] allLights = FindObjectsOfType<Light2D>();
        foreach (Light2D light in allLights)
        {
            GameObject parentObject = light.gameObject;
            
            if (parentObject.CompareTag("Light"))
            {
                AddLight(parentObject);
            }
        }
        
        /*Debug.Log($"Lights added: {lights.Count}");*/
    }

    public void CalcTime()
    {
        seconds = totalSeconds % 60;
        mins = totalSeconds / 60 % 60;
        hours = totalSeconds / 3600 % 24;
        days = totalSeconds / 86400;
        
        /*Debug.Log(
         $"Total Seconds: {totalSeconds}, Hours: {hours}, Minutes: {mins}, Seconds: {seconds}, Days: {days}");*/
    }

    public void ControlPPV()
{
    /*if (lights != null && lights.Count > 0)
    {
        foreach (var light in lights)
        {
            if (light != null)
                light.SetActive(true);
        }
    }
    else
    {
        Debug.LogWarning("Lights list is not assigned or is empty!");
    }*/
    
    if (ppv == null)
    {
        Debug.LogWarning("Post-Processing Volume (ppv) is not assigned or missing!");
        return;
    }

    // Handle night effects between 21:00 and 22:00
    if (hours >= 21 && hours < 22)
    {
        ppv.weight = (float)mins / 60;

        if (!activateLights && mins > 30)
        {
            if (lights != null)
            {
                foreach (var light in lights)
                {
                    light.SetActive(true);
                    StartCoroutine(FadeLight(light, lightIntensity, 2f)); // fades in over seconds
                }
                activateLights = true;
            }
            else
            {
                Debug.LogWarning("Lights array is not assigned or is empty!");
            }
        }
    }

    // Handle morning effects between 6:00 and 7:00, similar checks
    if (hours >= 6 && hours < 7)
    {
        ppv.weight = 1 - (float)mins / 60;

        if (activateLights && mins > 30)
        {
            if (lights != null)
            {
                foreach (var light in lights)
                {
                    StartCoroutine(FadeLight(light, 0f, 2f)); // fades out over seconds
                }
                activateLights = false;
            }
        }
    }
}

    public void DisplayTime()
    {
        timeDisplay.text = string.Format("{0:00}:{1:00}", hours, mins);
        dayDisplay.text = string.Format("Day:" + " " + "{0}", days);
    }
}