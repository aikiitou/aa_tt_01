using UnityEngine;

public class SppotLightmanager : MonoBehaviour
{
    [SerializeField] private Material spotLightColliderMaterial = null;

    private Light spotLight = null;

    private readonly int propLightPos = Shader.PropertyToID("_LightPos");
    private readonly int propLightDirection = Shader.PropertyToID("_LightDir");
    private readonly int propLightAngleCos = Shader.PropertyToID("_LightAngleCos");

    private void Awake()
    {
        spotLight = GetComponent<Light>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spotLightColliderMaterial == null) return;

        spotLightColliderMaterial.SetVector(propLightPos, spotLight.transform.position);
        spotLightColliderMaterial.SetVector(propLightDirection, spotLight.transform.forward);
        float halfAngleRad = spotLight.spotAngle * 0.5f * Mathf.Deg2Rad;
        float cosAngle = Mathf.Cos(halfAngleRad);
        spotLightColliderMaterial.SetFloat(propLightAngleCos, cosAngle);
    }
}
