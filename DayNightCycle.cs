using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sun;          // the directional light representing sun
    public float dayLength = 120f; // seconds for full 24hr cycle

    private float rotatePerSecond;

    void Start()
    {
        rotatePerSecond = 360f / dayLength;
    }

    void Update()
    {
        // rotate sun around x-axis
        sun.transform.Rotate(Vector3.right, rotatePerSecond * Time.deltaTime);
    }
}