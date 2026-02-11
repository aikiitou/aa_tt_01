using System;
using UnityEngine;

/// <summary>
/// プレイヤーの移動状態を通知するスクリプト
/// </summary>

public class MoveStateNotifier : MonoBehaviour
{
    public event Action<bool> OnMoveStateChanged;

    [Header("ForceFieldSettings")]
    [SerializeField]
    private float moveThreshold = 0.0f;

    private Rigidbody rb;
    private bool isMoving = true;

    private void Awake()
    {
        // 同一オブジェクトコンポーネント参照なのでAwakeで実行
        rb = GetComponent<Rigidbody>();
        if(rb == null)
        {
            Debug.LogError("Rigidbodyがアタッチされていません",this);
            enabled = false;
        }
    }

    private void FixedUpdate()
    {
        // Rigidbodyを使用しているためFixedUpdateで実行
        TryOnMoveStateChanged();
    }

    private void TryOnMoveStateChanged()
    {
        bool movingNow = rb.velocity.sqrMagnitude > moveThreshold * moveThreshold;

        if (isMoving != movingNow)
        {
            isMoving = movingNow;
            OnMoveStateChanged?.Invoke(isMoving);
        }
    }

}
