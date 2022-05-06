using UnityEngine;

public class Laser : MonoBehaviour
{
    // Settings
    [SerializeField]
    private int _speed;

    void Update()
    {
        LaserMovement();
        CheckInBounds();
    }

    void LaserMovement()
    {
        transform.Translate(new Vector3(0, _speed, 0) * Time.deltaTime);
    }

    void CheckInBounds() 
    {
        if (transform.position.y >= 7 || transform.position.y <= -6)
        {
            if (transform.parent)
            {
                Destroy(transform.parent.gameObject);
                Destroy(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
