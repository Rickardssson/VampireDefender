using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_AoeAttackAnimation : MonoBehaviour
{
    public float timeUntilDestroy;
    private Vector3 startPos;
    

    void Start()
    {
        startPos = transform.position;
    }
    void Update()
    {
        startPos = new Vector3(transform.position.x, transform.position.y, 0);
        gameObject.transform.position = startPos;
        timeUntilDestroy -= Time.deltaTime;
        if (timeUntilDestroy <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
