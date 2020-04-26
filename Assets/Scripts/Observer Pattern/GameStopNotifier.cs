using System.Collections.Generic;
using UnityEngine;

public abstract class GameStopNotifier : MonoBehaviour, ISubject<bool>
{
    protected LinkedList<IObserver<bool>> observers;
    [SerializeField] private bool gameStopped;

    public bool GetGameStopped()
    {
        return gameStopped;
    }

    public void SetGameStopped(bool value)
    {
        gameStopped = value;
        NotifyObservers();
    }

    public GameStopNotifier()
    {
        observers = new LinkedList<IObserver<bool>>();
    }

    /// <summary>
    ///     Notifies all the registered observers
    /// </summary>
    public void NotifyObservers()
    {
        foreach (IObserver<bool> observer in observers)
            observer.OnNotify(GetGameStopped());
    }

    /// <summary>
    ///     Registers a new observer
    /// </summary>
    /// <param name="o"> Observer object </param>
    public void Register(IObserver<bool> o)
    {
        observers.AddLast(o);
    }

    /// <summary>
    ///     Unregisters the registered observer object
    /// </summary>
    /// <param name="o"> observer object to be unregistered </param>
    public void Unregister(IObserver<bool> o)
    {
        if (observers.Contains(o))
            observers.Remove(o);
        else
            Debug.Log("Does not contain an observer " + o);
    }


}