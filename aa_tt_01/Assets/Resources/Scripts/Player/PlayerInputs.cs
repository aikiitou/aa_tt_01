using UnityEngine;

/// <summary>
/// プレイヤーの入力データを保存する構造体
/// ・移動の入力を保存する変数
/// ・視点移動の入力を保存する変数
/// ※PlayerInputsからのみ更新され、外部からは直接変更されない
/// </summary>
public struct PlayerInputData
{
    public Vector2 Move;
    public Vector2 Look;
    public int CameraMode;
}

/// <summary>
/// プレイヤーの入力を管理するスクリプト
/// ・WASDでの移動入力の取得する
/// ・マウスによる視点移動の入力の取得する
/// </summary>
public class PlayerInputs : MonoBehaviour
{
    public PlayerInputData inputData { get; private set; }

    void Update()
    {
        // フレーム単位で取得するためUpdateで処理する
        inputData = new PlayerInputData()
        {
            Move = InputMove(),
            Look = InputLook(),
            CameraMode = InputCameraMode()
        };
    }

    private Vector2 InputMove()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private Vector2 InputLook()
    {
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    private int InputCameraMode()
    {
        for(int i = 0; i <= 9; i++)
        {
            if(Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                return i;
            }
        }

        return -1;
    }
}
