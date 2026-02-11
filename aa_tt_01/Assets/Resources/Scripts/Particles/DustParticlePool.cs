using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 埃用パーティクルのObjectPoolを管理するスクリプト
/// ・Poolの作成・取得・返却を管理する
/// </summary>
public class DustParticlePool : MonoBehaviour
{

    [Header("ObjectPoolSettings")]
    [SerializeField]
    private GameObject dustParticlePrefab = null;
    [SerializeField]
    private int createNum = 0;

    private Queue<GameObject> objectPool = new Queue<GameObject>();

    private void Awake()
    {
        // 外部のStartで取得するためにAwakeで生成を実行
        for(int i = 0; i < createNum; i++)
        {
            GameObject createObject = Instantiate(dustParticlePrefab);
            createObject.SetActive(false);
            createObject.transform.parent = transform;
            objectPool.Enqueue(createObject);
        }
    }

    /// <summary>
    /// Poolからオブジェクトを一つ取得する
    /// </summary>
    /// <param name="get_object">取得したオブジェクト</param>
    /// <returns>取得成功時 true</returns>
    public bool TryGetPoolObject(out GameObject get_object)
    {
        if(objectPool.Count == 0)
        {
            get_object = null;
            return false;
        }
        get_object = objectPool.Dequeue();
        return true;
    }

    /// <summary>
    /// オブジェクトをPoolに返却する
    /// </summary>
    /// <param name="return_object">返却するオブジェクト</param>
    public void ReturnPoolObject(GameObject return_object)
    {
        return_object.SetActive(false);
        return_object.transform.parent = transform;
        objectPool.Enqueue(return_object);
    }
}
