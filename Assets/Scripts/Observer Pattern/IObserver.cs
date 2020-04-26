/// <summary>
///     Observer interface with generic type of T
/// </summary>
/// <typeparam name="T"> Generic type that the observer will be notified of </typeparam>
public interface IObserver<T>
{
    void OnNotify(T t);
}