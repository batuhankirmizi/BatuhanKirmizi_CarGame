using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    private LinkedList<GameObject> cars;

    [Header("Variables")]
    public int totalCarCount = 8;

    [Header("Prefabs")]
    public GameObject carPrefab;

    #region Singleton
    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    private void Start()
    {
        cars = new LinkedList<GameObject>();
        for (int i = 0; i < totalCarCount; i++)
            cars.AddLast(InstantiateCar(i));
        
        GameManager.Instance.NextPoint();
    }

    private GameObject InstantiateCar(int i)
    {
        Transform entrancePoint = GameManager.Instance.entrancePoints.transform.GetChild(i).transform;
        Transform targetPoint = GameManager.Instance.targetPoints.transform.GetChild(i).transform;

        GameObject car = Instantiate(carPrefab, entrancePoint.position, entrancePoint.rotation);
        car.GetComponent<Car>().entrancePoint = entrancePoint.gameObject;
        car.GetComponent<Car>().targetPoint = targetPoint.gameObject;
        car.SetActive(false);

        return car;
    }

    public GameObject SpawnCar(int i)
    {
        GameObject carToBeSpawned = GetCar(i);
        carToBeSpawned.SetActive(true);
        return carToBeSpawned;
    }

    public LinkedList<GameObject> GetCars()
    {
        return cars;
    }

    public GameObject GetCar(int i)
    {
        return cars.ElementAt(i);
    }

    public bool HasActiveCar()
    {
        foreach (GameObject car in cars)
            if (car.GetComponent<Car>().isActive)
                return true;
        return false;
    }
}
