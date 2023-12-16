using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    public CameraMovement mainCamera;
    public Vector2 maxPosition;
    public Vector2 minPosition;
    public Vector3 moveOffset;
    public EnemyHandler enemyHandler;

    //configures the camera bounds for the new room
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            mainCamera.maxPosition = maxPosition;
            mainCamera.minPosition = minPosition;

            //we need to teleport the player forward a bit to bypass the other transition
            collision.transform.position += moveOffset;
            if(enemyHandler != null )
                enemyHandler.WakeUpEnemies();
        }

    }
}
