using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 視点移動を管理するスクリプト
/// ・マウス入力による視点移動を行う
/// ・上下回転(Pitch)に制限をかける
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerInputs playerInputs;
    [SerializeField]
    private Volume glovalVolume;
    [SerializeField]
    private List<VolumeProfile> cameraMode;

    [Header("LookSettings")]
    [SerializeField]
    private float mouseSensitivity = 0.0f;
    [SerializeField]
    private float limitPitch = 0.0f;

    private float pitch = 0.0f;
    private float yaw = 0.0f;


    void Start()
    {
        // 視点操作中にカーソルが画面外に出ないようにする
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // フレーム単位で実行するためUpdateで処理する
        ApplyLook();
        ChangeCameraMode();
    }

    private void ApplyLook()
    {
        Vector2 lookInput = playerInputs.inputData.Look;

        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        yaw += mouseX;

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch,-limitPitch, limitPitch);

        transform.localRotation = Quaternion.Euler(pitch, yaw, 0);
    }

    private void ChangeCameraMode()
    {
        int cameraModeInput = playerInputs.inputData.CameraMode;
        if (cameraModeInput == -1) return;

        Debug.Log(cameraModeInput);

        if (cameraMode[cameraModeInput] == null)
        {
            //glovalVolume.profile = VolumeProfile.
        }
        glovalVolume.profile = cameraMode[cameraModeInput];
    }
}
