using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// フィールドを生成するオブジェクトのスクリプト
/// 生成方法はInspectorにアタッチされたPrefabを並べるだけ
/// 担当者:粟田
/// </summary>
public class MapCreator : MonoBehaviour
{
    private enum MapPrefabType
    {
        Type1,
        Type2,
    }

    [SerializeField]
    private List<GameObject> mapPrefabs = new();
    [SerializeField]
    private Vector2 mapSize;

    // Start is called before the first frame update
    void Start()
    {
        CreateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateMap()
    {
        GameObject MapParent = new GameObject("Map");

        for (int i = 0; i < mapSize.x; i++)
        {
            for(int j = 0; j < mapSize.y; j++)
            {
                // Prefabをランダムに選択して配置
                int prefabIndex = UnityEngine.Random.Range(0, mapPrefabs.Count);
                GameObject prefab = mapPrefabs[prefabIndex];
                Vector3 position = new Vector3(i * prefab.transform.localScale.x, 0, j * prefab.transform.localScale.z);
                Instantiate(prefab, position, Quaternion.identity, transform).transform.parent = MapParent.transform;
            }
        }
    }
}
