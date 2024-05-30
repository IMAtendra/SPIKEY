using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private bool moveToLeft;

    void Update()
    {
        PingPong();
    }

    // This Code add a Loop Animation of a GameObject
    private void PingPong()
    {
        float x = Mathf.PingPong(Time.time, 2f);
        float y = transform.position.y;
        float z = transform.position.z;

        if (moveToLeft)
        {
            transform.position = new Vector3(x: -(float)x,
                                             y: (float)y,
                                             z: (float)z);
        }
        else
        {
            transform.position = new Vector3(x: (float)x,
                                             y: (float)y,
                                             z: (float)z);

        }
    }
}
