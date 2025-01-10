using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_BloodMagicAnimation : MonoBehaviour
{
    
    private Vector2 PointerPosition { get; set; }

    private float destroyTime = 0.40f;
    // Start is called before the first frame update
    void Start()
    {
                
        // Update the pointer position
        PointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the direction to the pointer
        Vector2 direction = PointerPosition - (Vector2)transform.position;

        // Calculate the angle between the weapon and the pointer
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the weapon to face the pointer
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        
        // Rotate Attack animation to have the correct rotation
        gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // Update is called once per frame
    void Update()
    {
        destroyTime -= Time.deltaTime;
        if (destroyTime <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
