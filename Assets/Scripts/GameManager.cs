using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : GameStopNotifier
{
    [Header("General Game Variables")]
    [SerializeField] private bool isLastScene = false;

    //  Entrance and target point game objects
    public GameObject entrancePoints;
    public GameObject targetPoints;

    // For handling right and left button clicking
    public bool rightBClicked = false;
    public bool leftBClicked = false;

    #region Singleton
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    internal void RestartGame()
    {
        // Reset variables
        currentPoint = 0;
        SetGameStopped(true);

        // Set cars inactive
        ResetAllCars();
        int i = 0;
        foreach (GameObject car in ObjectPooler.Instance.GetCars()) {
            car.SetActive(false);
            car.name = "Car - Active";
            car.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            car.GetComponent<Car>().isActive = true;
            if(car.GetComponent<Car>().path != null)
                car.GetComponent<Car>().path.Reset();
            i++;
        }

        // Restarts from point 0
        NextPoint();
    }

    [Header("Debugging")]
    [SerializeField] public bool gameOver = false;
    public float nextSceneIn = 4f;
    [Range(0, 7)] public int currentPoint = 0;
    private void Update()
    {
        #region Input Handling
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && !gameOver && GetGameStopped())
            SetGameStopped(false);

        if (Input.GetKeyDown(KeyCode.LeftArrow) && !GetGameStopped())
            leftBClicked = true;
        else if (Input.GetKeyDown(KeyCode.RightArrow) && !GetGameStopped())
            rightBClicked = true;
        
        if (Input.GetKeyUp(KeyCode.RightArrow))
            rightBClicked = false;
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
            leftBClicked = false;
        #endregion

        // Game over
        if (gameOver)
            GameOver();
    }

    public void NextPoint()
    {
        SetGameStopped(true);

        ObjectPooler.Instance.SpawnCar(currentPoint);
        UpdateUI();

        currentPoint = ObjectPooler.Instance.HasActiveCar() ? currentPoint + 1 : currentPoint;
    }

    private void UpdateUI() {
        GameObject.Find("Text - Entrance").GetComponent<RectTransform>().anchoredPosition = TransformToUIPos(entrancePoints.transform.GetChild(currentPoint).gameObject);
        GameObject.Find("Text - Target").GetComponent<RectTransform>().anchoredPosition = TransformToUIPos(targetPoints.transform.GetChild(currentPoint).gameObject);
    }

    private Vector2 TransformToUIPos(GameObject go) {
        RectTransform CanvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
        Vector2 viewportPos = Camera.main.WorldToViewportPoint(go.transform.position);
        Vector2 goPos = new Vector2((viewportPos.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f),
                                                          (viewportPos.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f));
        return goPos;
    }

    /// <summary>
    ///     Executes the 'Reset()' function of each instantiated car
    /// </summary>
    public void ResetAllCars()
    {
        int i = 0;
        foreach (GameObject car in ObjectPooler.Instance.GetCars()) {
            if(car.activeSelf)
                car.GetComponent<Car>().Reset();
            i++;
        }
    }

    /// <summary>
    ///     Stops all the cars, enables level end text
    /// </summary>
    private void GameOver()
    {
        StopAllCars();

        SetGameStopped(true);
        if(isLastScene)
            GameObject.Find("Text - End").GetComponent<Text>().text = "Game Over";
        GameObject.Find("Panel - End").GetComponent<Image>().enabled = true;
        GameObject.Find("Text - End").GetComponent<Text>().enabled = true;
        GameObject.Find("Text - End Counter").GetComponent<Text>().enabled = !isLastScene;
        
        nextSceneIn -= Time.deltaTime;
        GameObject.Find("Text - End Counter").GetComponent<Text>().text = ((int)nextSceneIn).ToString();
        if (nextSceneIn <= 0f)
            NextScene();

        gameOver = true;
    }

    private void StopAllCars()
    {
        foreach (GameObject car in ObjectPooler.Instance.GetCars())
            if(car.activeSelf)
                car.GetComponent<Car>().Stop();
    }

    private void NextScene()
    {
        if(!IsLastScene())
            SceneHandler.NextScene();
    }

    private bool IsLastScene()
    {
        return isLastScene;
    }
}
