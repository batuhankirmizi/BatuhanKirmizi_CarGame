public interface ISubject<T>
{
    void Register(IObserver<T> o);
    void Unregister(IObserver<T> o);
    void NotifyObservers();
}