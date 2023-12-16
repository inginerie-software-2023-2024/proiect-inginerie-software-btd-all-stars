using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs;
    public Vector3 direction;
    public float speed;

    private void Start()
    {
        InvokeRepeating("SpawnCar", 0f, Random.Range(2f,4f));
    }

    private void SpawnCar()
    {
        int randomIndex = Random.Range(0, carPrefabs.Length);
        GameObject newCar = Instantiate(carPrefabs[randomIndex], transform.position, Quaternion.identity);
        CarMovement carMovement = newCar.GetComponent<CarMovement>();
        carMovement.InitializeMovement(direction, speed);
    }
}
