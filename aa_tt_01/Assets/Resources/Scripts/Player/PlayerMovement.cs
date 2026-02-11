using System;
using UnityEngine;

/// <summary>
/// プレイヤーの移動を管理するスクリプト
/// ・移動の入力を受け取る
/// ・カメラ基準で移動方向を計算する
/// ・Rigidbodyに力を加えて移動させる
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerInputs playerInputs;
    [SerializeField]
    private Transform cameraTransform;
    
    [Header("MovementSettings")]
    [SerializeField]
    private float moveAcceleration = 0.0f;
    [SerializeField]
    private float limitMoveSpeed = 0.0f;

    private Rigidbody rb = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Rigidbodyを操作するため、物理更新タイミングで処理する
        ApplyMovement();
        LimitHorizontalSpeed();

    }

    private void ApplyMovement()
    {
        Vector2 inputVec = playerInputs.inputData.Move;

        Vector3 currentVel = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);

        if(inputVec.sqrMagnitude < 0.01f)
        {
            // 入力がない場合、現在速度を打ち消す力を加えて自然に停止させる
            Vector3 stopForce  = -currentVel * moveAcceleration;
            rb.AddForce(stopForce,ForceMode.Acceleration);
            return;
        }

        // カメラの向きを基準に移動方向を計算する
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // Y成分は無視する
        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;

        Vector3 direction = cameraForward.normalized * inputVec.y + cameraRight.normalized * inputVec.x;
        Vector3 targetVel = direction.normalized * limitMoveSpeed;
        Vector3 velocityDiff = targetVel - currentVel;

        rb.AddForce(velocityDiff * moveAcceleration, ForceMode.Acceleration);
    }

    private void LimitHorizontalSpeed()
    {
        Vector3 horizontalVel = new Vector3(rb.velocity.x,0.0f,rb.velocity.z);

        if(horizontalVel.magnitude > limitMoveSpeed)
        {
            Vector3 limited = horizontalVel.normalized * limitMoveSpeed;
            rb.velocity = new Vector3(limited.x,rb.velocity.y,limited.z);
        }
    }
}
