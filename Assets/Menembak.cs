using UnityEngine;

public class Menembak : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        transform.Translate(0f, 0f, speed * Time.deltaTime);
    }
  
}
