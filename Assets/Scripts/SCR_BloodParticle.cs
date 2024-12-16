using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_BloodParticles : MonoBehaviour
{
    public static SCR_BloodParticles Instance { get; private set; }
    private SCR_MeshParticleSystem meshParticleSystem;
    private List<SingleParticle> singleParticleList;
    
    [SerializeField] private int particleCount = 5;
    [SerializeField] private float particleSpread = 15f;
    [SerializeField] private float minTravelDistance = 5f;
    [SerializeField] private float maxTravelDistance = 50f;
    [SerializeField] private float minSpeed = 10f;
    [SerializeField] private float maxSpeed = 20f;
    
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            /*Debug.LogError("Multiple SCR_BloodParticles instances detected! This can result in duplicate blood particles.");*/
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        singleParticleList = new List<SingleParticle>();
        
        meshParticleSystem = FindObjectOfType<SCR_MeshParticleSystem>();
        /*if (meshParticleSystem == null)
        {
            Debug.LogError("SCR_MeshParticleSystem is not in your scene.");
        }*/

        SCR_EnemyHealth[] enemies = FindObjectsOfType<SCR_EnemyHealth>();
        foreach (SCR_EnemyHealth enemy in enemies)
        {
            enemy.DamageEvent += OnEnemyDamaged;
        }
       
    }

    public void RegisterEnemy(SCR_EnemyHealth enemyHealth)
    {
        if (enemyHealth != null)
        {
            /*Debug.Log($"Registering DamageEvent for enemy {enemyHealth.name}");*/
            enemyHealth.DamageEvent += OnEnemyDamaged;
        }
        /*else
        {
            Debug.LogWarning("null enemy health component in enemy");
        }*/
    }

    private void OnDestroy()
    {
        SCR_EnemyHealth[] enemies = FindObjectsOfType<SCR_EnemyHealth>();
        foreach (SCR_EnemyHealth enemy in enemies)
        {
            enemy.DamageEvent -= OnEnemyDamaged;
        }
    }

    public void UnregisterEnemy(SCR_EnemyHealth enemyHealth)
    {
        if (enemyHealth != null)
        {
            /*Debug.Log($"Unregistering DamageEvent for enemy {enemyHealth.name}");*/
            enemyHealth.DamageEvent -= OnEnemyDamaged;
        }
    }

    private void OnEnemyDamaged(Vector2 hitPosition, Vector2 hitDirection)
    {
        /*Debug.Log($"OnEnemyDamaged triggered at position {hitPosition} with direction {hitDirection}.");*/
        BloodParticle(hitPosition, hitDirection);
    }
    
    private void Update()
    {
        for (int i = 0; i < singleParticleList.Count; i++)
        {
            SingleParticle singleParticle = singleParticleList[i];
            singleParticle.Update();
            
            if (singleParticle.IsMovementComplete())
            {
                singleParticleList.RemoveAt(i);
                /*Debug.Log($"Particle removed. Total particles after removal: {singleParticleList.Count}");*/
                i--;
            }
        }
        
        /*singleParticleList.RemoveAll(p => p.isExpired());*/
        meshParticleSystem.RecalculateMeshBounds();
    }
    
    public void BloodParticle(Vector3 position, Vector3 direction)
    {
        for (int i = 0; i < particleCount; i++)
        {
            Vector3 randomDirection = Quaternion.Euler(
                Random.Range(-particleSpread, particleSpread), 
                Random.Range(-particleSpread, particleSpread), 
                Random.Range(-particleSpread, particleSpread)
            ) * direction;
            
            float randomDistance = Random.Range(minTravelDistance, maxTravelDistance);
            float randomSpeed = Random.Range(minSpeed, maxSpeed);
            
            singleParticleList.Add(new SingleParticle(
                position, 
                randomDirection.normalized, 
                randomDistance, 
                randomSpeed,
                meshParticleSystem
            ));
            
            Debug.Log($"Particle created, distance: {randomDistance}, speed: {randomSpeed}");
        }
    }
    
    private void OnDrawGizmos()
    {
        if (singleParticleList != null)
        {
            foreach (var particle in singleParticleList)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(particle.position, 0.1f);
            }
        }
    }

    private class SingleParticle
    {
        private SCR_MeshParticleSystem meshParticleSystem;
        public Vector3 position;
        private Vector3 direction;
        private Vector3 quadSize;
        private Vector3 startPosition;
        private int quadIndex;
        private float rotation;
        
        private float lifeTime = 5f;
        private float moveSpeed;
        private float maxTravelDistance;
        private float deceleration = 50f;

        public SingleParticle(Vector3 position, Vector3 direction, float maxTravelDistance, float moveSpeed, 
            SCR_MeshParticleSystem meshParticleSystem)
        {
            this.position = position;
            this.moveSpeed = moveSpeed;
            this.startPosition = position;
            this.direction = direction.normalized;
            this.maxTravelDistance = maxTravelDistance;
            this.meshParticleSystem = meshParticleSystem;

            quadSize = new Vector3(0.5f, 1f);
            rotation = Random.Range(0f, 360f);

            quadIndex = meshParticleSystem.AddQuad(position, direction,rotation, quadSize,  0);
        }

        public void Update()
        {
            lifeTime -= Time.deltaTime;
            
            moveSpeed = Mathf.Max(0f, moveSpeed - (deceleration * Time.deltaTime));

            if (moveSpeed > 0.1f && Vector3.Distance(position, startPosition) < maxTravelDistance)
            {
                position += direction * (moveSpeed * Time.deltaTime);
            }
            /*else
            {
                Debug.Log($"Movement complete for quadIndex: {quadIndex}, Final position: {position}");
            }*/
            
            if (Vector3.Distance(position, startPosition) >= maxTravelDistance)
            {
                Debug.Log($"Particle reached its max travel distance: {maxTravelDistance}");
            }
            
            rotation += 360f * (moveSpeed / 10f) * Time.deltaTime;

            meshParticleSystem.SetQuadPosition(quadIndex, position);
            meshParticleSystem.UpdateQuad(quadIndex, position, rotation, quadSize,  0);
        }

        public bool IsMovementComplete()
        {
            return moveSpeed < 0.1f || lifeTime <= 0f || Vector3.Distance(position, startPosition) >= maxTravelDistance;
        }
    }
}
