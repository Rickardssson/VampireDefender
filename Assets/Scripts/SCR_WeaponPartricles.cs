using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_WeaponParticles : MonoBehaviour
{
    public static SCR_WeaponParticles Instance { get; private set; }
    private SCR_MeshParticleSystem meshParticleSystem;
    private SCR_PlayerWeapon playerWeapon;
    private List<SingleParticle> singleParticleList;
    
    public void Awake()
    {
        Instance = this;
        singleParticleList = new List<SingleParticle>();
        
        meshParticleSystem = FindObjectOfType<SCR_MeshParticleSystem>();
        if (meshParticleSystem == null)
        {
            Debug.LogError("SCR_MeshParticleSystem is not in your scene.");
        }

        playerWeapon = FindObjectOfType<SCR_PlayerWeapon>();
        if (playerWeapon != null)
        {
            playerWeapon.AttackEvent += OnPlayerAttack;
        }
        else
        {
            Debug.LogError("SCR_PlayerWeapon is not in your scene.");
        }
    }

    private void OnDestroy()
    {
        if (playerWeapon != null)
        {
            playerWeapon.AttackEvent -= OnPlayerAttack;
        }
    }

    private void OnPlayerAttack(Vector2 attackPosition, Vector2 attackDirection)
    {
        SpawnWeaponParticle(attackPosition, attackDirection);
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
                i--;
            }
        }
        
        singleParticleList.RemoveAll(p => p.isExpired());
        meshParticleSystem.RecalculateMeshBounds();
    }
    
    public void SpawnWeaponParticle(Vector3 position, Vector3 direction)
    {
        /*Debug.Log($"Spawn particle at {position}, direction {direction}, caller: {System.Environment.StackTrace}");*/
        singleParticleList.Add(new SingleParticle(position, direction.normalized, meshParticleSystem));
    }

    private class SingleParticle
    {
        private SCR_MeshParticleSystem meshParticleSystem;
        private Vector3 position;
        private Vector3 direction;
        private Vector3 quadSize;
        private Vector3 startPosition;
        private int quadIndex;
        private float rotation;
        private float lifeTime = 5f;
        private float moveSpeed;
        private float maxTravelDistance = 10f;

        public bool isExpired()
        {
            return lifeTime <= 0f || Vector3.Distance(position, startPosition) > maxTravelDistance;
        }

        public SingleParticle(Vector3 position, Vector3 direction, SCR_MeshParticleSystem meshParticleSystem)
        {
            this.position = position;
            this.startPosition = position;
            this.direction = direction.normalized;
            this.meshParticleSystem = meshParticleSystem;

            quadSize = new Vector3(0.5f, 1f);
            rotation = Random.Range(0f, 360f);
            moveSpeed = 30f;

            quadIndex = meshParticleSystem.AddQuad(position, rotation, quadSize, true, 0);
        }

        public void Update()
        {
            lifeTime -= Time.deltaTime;
            position += direction * moveSpeed * Time.deltaTime;
            rotation += 360f * (moveSpeed / 10f) * Time.deltaTime;

            meshParticleSystem.UpdateQuad(quadIndex, position, rotation, quadSize, true, 0);

            float slowDownParticle = 3.5f;
            moveSpeed -= moveSpeed * slowDownParticle * Time.deltaTime;
        }

        public bool IsMovementComplete()
        {
            return moveSpeed < 0.1f;
        }
    }
}
