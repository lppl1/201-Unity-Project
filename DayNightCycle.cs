using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Transform pivot;
    public Light sun;          // the directional light representing sun
    public float dayLength = 120f; // seconds for full 24hr cycle

    private float rotatePerSecond;

    void Start(){
        rotatePerSecond = 360f / dayLength;
    }

    void Update(){
         // rotate pivot continuously
        pivot.Rotate(Vector3.right * rotatePerSecond * Time.deltaTime, Space.World);

        // use pivot local forward vector for smoother intensity transition
        float intensity = (Vector3.Dot(pivot.forward, Vector3.down) + 1f) * 0.5f;

        sun.intensity = intensity;
        RenderSettings.skybox.SetFloat("_Exposure", intensity);
    }
}