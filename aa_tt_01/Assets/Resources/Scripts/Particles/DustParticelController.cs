using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの移動に応じて埃用パーティクルを管理するスクリプト
/// ・ObjectPoolを利用した生成・返却
/// ・プレイヤーとの距離に応じた出現・消滅制御
/// </summary>
public class DustParticelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform playerTransform = null;
    [SerializeField]
    private DustParticlePool dustParticlePool = null;

    [Header("SpawnSettings")]
    [SerializeField]
    private float spawnIntervalDistance = 0.0f; // 次の埃用パーティクルを生成するまでに必要なプレイヤーとの距離
    [SerializeField]
    private float spawnDistnace = 0.0f; // 生成時に配置するOffset距離
    [SerializeField]
    private float despawnDistance = 0.0f; // プレイヤーからこの距離以上離れたら返却する

    private Queue<GameObject> spawnParticles = new Queue<GameObject>();

    void Start()
    {
        // 事前にObjectPoolを用意してもらわないといけないのでStartで実行
        if (spawnParticles.Count <= 0)
        {
            GameObject spawnParticle = null;
            if(dustParticlePool.TryGetPoolObject(out spawnParticle))
            {
                spawnParticle.SetActive(true);
                spawnParticle.transform.parent = transform;
                spawnParticles.Enqueue(spawnParticle);
            }
        }
    }

    void Update()
    {
        // 毎フレーム確認したいのでUpdateで実行
        if(IsSpawnNextParticle())
        {
            SpawnDustParticle();
        }

        if(IsDespawnParticle())
        {
            DespawnDustParticle();
        }
    }

    private bool IsSpawnNextParticle()
    {
        float distance = GetFirstParticlefromPlayerDistance();
        if (distance < spawnIntervalDistance || spawnParticles.Count >= 2)
        {
            return false;
        }
        return true;
    }

    private bool IsDespawnParticle()
    {
        float distance = GetFirstParticlefromPlayerDistance();
        if (distance < despawnDistance)
        {
            return false;
        }

        return true;
    }

    private float GetFirstParticlefromPlayerDistance()
    {
        return Vector3.Distance(GetFirstParticleHorizontalPos(), GetPlayerHorizontalPos());
    }

    private Vector3 GetFirstParticlefromPlayerHorizontalVec()
    {
        return GetPlayerHorizontalPos() - GetFirstParticleHorizontalPos();
    }

    private Vector3 GetPlayerHorizontalPos()
    {
        Vector3 playerHorizontalPosition = new Vector3(playerTransform.position.x, 0.0f, playerTransform.position.z);
        return playerHorizontalPosition;
    }
    
    private Vector3 GetFirstParticleHorizontalPos()
    {
        GameObject firstParticle = spawnParticles.Peek();
        Vector3 firstParticleHorizontalPosition = new Vector3(firstParticle.transform.position.x, 0.0f, firstParticle.transform.position.z);
        return firstParticleHorizontalPosition;
    }

    private void SpawnDustParticle()
    {
        GameObject getPoolObject = null;
        dustParticlePool.TryGetPoolObject(out getPoolObject);
        getPoolObject.SetActive(true);
        getPoolObject.transform.parent = transform;
        Vector3 spawnVec = GetFirstParticlefromPlayerHorizontalVec().normalized;
        Vector3 spawnOffset = spawnVec * spawnDistnace;
        getPoolObject.transform.position = spawnParticles.Peek().transform.position + spawnOffset;
        spawnParticles.Enqueue(getPoolObject);
    }

    private void DespawnDustParticle()
    {
        dustParticlePool.ReturnPoolObject(spawnParticles.Dequeue());
    }
}
