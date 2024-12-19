using UnityEngine;
using UnityEngine.Events;

public class SCR_EnemyKillCounter : MonoBehaviour
{
    [Header("Attributes")] 
    [SerializeField] private int killsToTriggerEvent = 5; 
    [SerializeField] private float countDownInterval = 2f;
    [SerializeField] private float cooldownTimer = 5f; 

    [Header("Kill Counter Debug")] 
    [SerializeField] private int currentKillCount = 0;
    
    public UnityEvent OnKillStreakAchieved;
    public UnityEvent OnKill;

    private bool isCooldown = false;
    
    public static SCR_EnemyKillCounter Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(CountDownKills), countDownInterval, countDownInterval);
    }
    
    public void RegisterKill(int kills = 1)
    {
        currentKillCount += kills;
        
        if (OnKill != null)
        {
            OnKill?.Invoke();
        }
   
        if (currentKillCount >= killsToTriggerEvent && !isCooldown)
        {
            TriggerKillStreakEvent();
            currentKillCount = 0;
        }
    }
    
    private void TriggerKillStreakEvent()
    {
        OnKillStreakAchieved?.Invoke();
        isCooldown = true;
        Invoke(nameof(EndCooldown), cooldownTimer);
    }
    
    private void EndCooldown()
    {
        isCooldown = false;
        Debug.Log("Kill streak cooldown ended.");
    }
    
    private void CountDownKills()
    {
        if (currentKillCount > 0)
        {
            currentKillCount--;
        }
    }
}