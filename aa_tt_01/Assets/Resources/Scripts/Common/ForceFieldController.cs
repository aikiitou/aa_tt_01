using UnityEngine;

/// <summary>
/// MoveStateNotifierの通知に応じて
/// ParticleSystemForceFieldのON/OFFを制御する
/// </summary>
public class ForceFieldController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private MoveStateNotifier moveStateNotifier;

    private ParticleSystemForceField forceField;

    private void Awake()
    {
        // 同一オブジェクト上のコンポーネント参照のため、他オブジェクトの初期化を待たずにAwakeで実行
        forceField = GetComponent<ParticleSystemForceField>();
        if (forceField == null)
        {
            Debug.LogError("ParticleSystemForceFieldがアタッチされていません", this);
        }
    }

    private void Start()
    {
        // 外部オブジェクトコンポーネント参照のため、Startでチェック
        if (moveStateNotifier == null)
        {
            Debug.LogError("MoveStateNotifierがアタッチされていません", this);
        }

    }

    private void OnEnable()
    {
        if (moveStateNotifier == null) return;
        moveStateNotifier.OnMoveStateChanged += OnMoveStateChanged;
    }

    private void OnDisable()
    {
        if (moveStateNotifier == null) return;
        moveStateNotifier.OnMoveStateChanged -= OnMoveStateChanged;
    }

    private void OnMoveStateChanged(bool isMoving)
    {
        if (forceField == null) return;
        forceField.enabled = isMoving;
    }
}
