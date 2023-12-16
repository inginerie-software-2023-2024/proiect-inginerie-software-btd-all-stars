using UnityEngine;

public class CarMovement : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private Animator animator;
    public void InitializeMovement(Vector3 direction, float speedAux)
    {
        this.direction = direction.normalized;
        speed = speedAux;
        animator = GetComponent<Animator>();
        animator.SetFloat("moveX", direction.x);
    }

    private void Update()
    {
        MoveCar();
    }
    private void MoveCar()
    {
        transform.Translate(speed * Time.deltaTime * direction);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "destroyer")
            Destroy(gameObject);

    }
}

