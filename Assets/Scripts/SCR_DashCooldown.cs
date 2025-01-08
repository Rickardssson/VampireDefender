using UnityEngine;
using UnityEngine.UI;

public class SCR_DashCooldown : MonoBehaviour
{
    public TopDownMovement topDownMovement;
    public Image dash_cooldown_bar;
    
    private void Start()
    {
        if (topDownMovement == null)
        {
            topDownMovement = GetComponent<TopDownMovement>();
        }
        
        dash_cooldown_bar.fillAmount = 1f;
    }

    private void Update()
    {
        if (topDownMovement != null)
        {
            float maxCooldown = topDownMovement.dashCooldownDuration;
            float currentCooldown = topDownMovement.CurrentDashCooldown;
            dash_cooldown_bar.fillAmount -= 1f / maxCooldown * Time.deltaTime;

            if (topDownMovement.isDashing)
            {
                dash_cooldown_bar.fillAmount = 0f;
            }
            else
            {
                dash_cooldown_bar.fillAmount = 1f - currentCooldown / maxCooldown;
            }
        }
    }
}
