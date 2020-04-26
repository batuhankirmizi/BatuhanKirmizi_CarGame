using UnityEngine;

public abstract class GameStopObserver : MonoBehaviour, IObserver<bool>
{
    private bool gameStopped;

    private static int observerIDTracker = 0;
    
    private int observerID;

    private ISubject<bool> gameStopNotifier;

    public GameStopObserver(GameStopNotifier gameStopNotifier)
    {
        this.gameStopNotifier = gameStopNotifier;
        observerID = ++observerIDTracker;
    }

    protected void Start()
    {
        gameStopNotifier.Register(this);
    }

    /// <summary>
    ///     Notifies the observer when the game stops/continues
    /// </summary>
    /// <param name="gameStopped"></param>
    public void OnNotify(bool gameStopped)
    {
        this.gameStopped = gameStopped;
        if (gameStopped)
            OnGameStopped();
        else
            OnGameRun();
    }

    public abstract void OnGameStopped();
    public abstract void OnGameRun();
}
