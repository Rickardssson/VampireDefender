using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_KillPickupAnimation : MonoBehaviour
{
    private float timer = 0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.35f)
        {
            Destroy(gameObject);
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
