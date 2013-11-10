public interface IEnumerable<T> 
{
    IEnumerator<T> GetEnumerator();
}

public interface IEnumerator<T> 
{
    T Current { get; } // but can throw an exception
    bool MoveNext();
    void Reset(); // not used
}

public interface IObserverDraft1<T> 
{
    SetCurrent(T value); // but need to handle an exception
    MoveNext(bool can) ;
}

interface IObserverDraft2<T>
{
    OnNext(T value); 
    OnException(Exception ex);
    MoveNext(bool can) ; //but called every time!
}

public interface IObserver<T> // But what about IDisposable ?
{
    OnNext(T value); 
    OnException(Exception ex);
    OnCompleted(); // only called once
}


interface IEnumerable<T> 
{
    IEnumerator<T> GetEnumerator();
}

public interface IObservableDraft<T> 
{
    SetObserver(IObserver<T> obs); // But how do I stop observing?
}

interface IObservableDraft1<T> 
{
    Subscribe(IObserver<T> obs);
    Unsubscribe(IObserver<T> obs); // can I be more composable?
}

interface IObservable<T> 
{
    IDisposable Subscribe(IObserver<T> obs);
}

