using UnityEngine;

public class PacmanMove : MonoBehaviour
{
    public float speed = 0.35f;
    private Vector2 dest = Vector2.zero;

    private void Start()
    {
        dest = this.transform.position;
    }
}
