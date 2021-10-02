using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float seconds = 1f;

    void Start()
    {
        Invoke("DestroyNow", seconds);
    }

    void DestroyNow()
    {
        Destroy(gameObject);
    }
}
