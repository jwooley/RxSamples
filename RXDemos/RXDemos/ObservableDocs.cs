using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Collections;
using System.Threading;

namespace RXDemos
{
    public static class ObservableDocs
    {
        //     Applies an accumulator function over an observable sequence.
        public static TSource Aggregate<TSource>(this IObservable<TSource> source, Func<TSource, TSource, TSource> accumulator);
        //
        // Summary:
        //     Returns the observable sequence that reacts first.
        public static IObservable<TSource> Amb<TSource>(params IObservable<TSource>[] sources);
        public static IObservable<TSource> Amb<TSource>(this IEnumerable<IObservable<TSource>> sources);
        //
        // Summary:
        //     Merges two observable sequences into one observable sequence by using the
        //     selector function whenever one of the observable sequences has a new value.
        public static IObservable<TResult> CombineLatest<TLeft, TRight, TResult>(this IObservable<TLeft> leftSource, IObservable<TRight> rightSource, Func<TLeft, TRight, TResult> selector);
        //
        // Summary:
        //     Concatenates all the observable sequences.
        public static IObservable<TSource> Concat<TSource>(params IObservable<TSource>[] sources);
        //
        // Summary:
        //     Returns the number of values in an observable sequence.
        public static int Count<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Creates an observable sequence from the subscribe implementation.
        public static IObservable<TSource> Create<TSource>(Func<IObserver<TSource>, Action> subscribe);
        //
        // Summary:
        //     Creates an observable sequence from the subscribe implementation.
        public static IObservable<TSource> CreateWithDisposable<TSource>(Func<IObserver<TSource>, IDisposable> subscribe);
        //
        // Summary:
        //     Returns an observable sequence that invokes the observableFactory function
        //     whenever a new observer subscribes.
        public static IObservable<TValue> Defer<TValue>(Func<IObservable<TValue>> observableFactory);
        //
        // Summary:
        //     Time shifts the observable sequence by dueTime.  The relative time intervals
        //     between the values are preserved.
        public static IObservable<TSource> Delay<TSource>(this IObservable<TSource> source, DateTimeOffset dueTime);
        //
        // Summary:
        //     Dematerializes the explicit notification values of an observable sequence
        //     as implicit notifications.
        public static IObservable<TSource> Dematerialize<TSource>(this IObservable<Notification<TSource>> source);
        //
        // Summary:
        //     Invokes the action for its side-effects on each value in the observable sequence.
        public static IObservable<TSource> Do<TSource>(this IObservable<TSource> source, Action<TSource> action);
        //
        // Summary:
        //     Returns an empty observable sequence.
        public static IObservable<TValue> Empty<TValue>();
        //
        // Summary:
        //     Runs all observable sequences in parallel and combines their first values.
        public static IObservable<TSource[]> ForkJoin<TSource>(params IObservable<TSource>[] sources);
        //
        // Summary:
        //     Converts a Begin/End invoke function pair into a asynchronous function.
        public static Func<IObservable<Unit>> FromAsyncPattern(Func<AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end);
        //
        // Summary:
        //     Returns an observable sequence that contains the values of the underlying
        //     .NET event.
        public static IObservable<IEvent<TEventArgs>> FromEvent<TEventArgs>(Action<EventHandler<TEventArgs>> addHandler, Action<EventHandler<TEventArgs>> removeHandler) where TEventArgs : EventArgs;
        //
        // Summary:
        //     Returns an observable sequence that contains the values of the underlying
        //     .NET event.
        public static IObservable<IEvent<TEventArgs>> FromEvent<TEventArgs>(object target, string eventName) where TEventArgs : EventArgs;
        //
        // Summary:
        //     Returns an observable sequence that contains the values of the underlying
        //     .NET event.
        public static IObservable<IEvent<TEventArgs>> FromEvent<TDelegate, TEventArgs>(Func<EventHandler<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler) where TEventArgs : EventArgs;
        //
        // Summary:
        //     Generates an observable sequence by repeatedly calling the function.
        public static IObservable<TValue> Generate<TValue>(this Func<Notification<TValue>> function);
        //
        // Summary:
        //     Generates an observable sequence by repeatedly calling the function.
        public static IObservable<TValue> Generate<TValue>(this Func<TimeInterval<Notification<TValue>>> function);
        //
        // Summary:
        //     Generates an observable sequence by enumerating the sequence.
        public static IObservable<TValue> Generate<TValue>(this IEnumerable<Notification<TValue>> enumerable);
        //
        // Summary:
        //     Generates an observable sequence by enumerating the sequence.
        public static IObservable<TValue> Generate<TValue>(this IEnumerable<TimeInterval<Notification<TValue>>> enumerable);
        //
        // Summary:
        //     Generates an observable sequence by repeatedly calling the function.
        public static IObservable<TValue> Generate<TValue>(this IScheduler scheduler, Func<Notification<TValue>> function);
        //
        // Summary:
        //     Generates an observable sequence by repeatedly calling the function.
        public static IObservable<TValue> Generate<TValue>(this IScheduler scheduler, Func<TimeInterval<Notification<TValue>>> function);
        //
        // Summary:
        //     Generates an observable sequence by enumerating the sequence.
        public static IObservable<TValue> Generate<TValue>(this IScheduler scheduler, IEnumerable<Notification<TValue>> enumerable);
        //
        // Summary:
        //     Generates an observable sequence by enumerating the sequence.
        public static IObservable<TValue> Generate<TValue>(this IScheduler scheduler, IEnumerable<TimeInterval<Notification<TValue>>> enumerable);
        //
        // Summary:
        //     Generates an observable sequence by iterating a state from an initial state
        //     until the condition fails. For each state, a value is generated.
        public static IObservable<TResult> Generate<TState, TResult>(TState initial, Func<TState, IObservable<TResult>> resultSelector, Func<TState, TState> iterate);
        //
        // Summary:
        //     Generates an observable sequence by iterating a state from an initial state
        //     until a completion notification is sent.
        public static IObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, Notification<TResult>> resultSelector, Func<TState, TState> iterate);
        //
        // Summary:
        //     Generates an observable sequence by iterating a state from an initial state
        //     until a completion notification is sent.  For each state, a timestamped notification
        //     is generated at a time interval dependent on the state.
        public static IObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, TimeInterval<Notification<TResult>>> resultSelector, Func<TState, TState> iterate);
        //
        // Summary:
        //     Generates an observable sequence by iterating a state from an initial state
        //     until the condition fails. For each state, a value is generated.
        public static IObservable<TResult> Generate<TState, TResult>(this IScheduler scheduler, TState initial, Func<TState, IObservable<TResult>> resultSelector, Func<TState, TState> iterate);
        //
        // Summary:
        //     Generates an observable sequence by iterating a state from an initial state
        //     until a completion notification is sent.
        public static IObservable<TResult> Generate<TState, TResult>(this IScheduler scheduler, TState initialState, Func<TState, Notification<TResult>> resultSelector, Func<TState, TState> iterate);
        //
        // Summary:
        //     Generates an observable sequence by iterating a state from an initial state
        //     until a completion notification is sent.  For each state, a timestamped notification
        //     is generated at a time interval dependent on the state.
        public static IObservable<TResult> Generate<TState, TResult>(this IScheduler scheduler, TState initialState, Func<TState, TimeInterval<Notification<TResult>>> resultSelector, Func<TState, TState> iterate);
        //
        // Summary:
        //     Generates an observable sequence by iterating a state from an initial state
        //     until the condition fails. For each state, a value is generated.
        public static IObservable<TResult> Generate<TState, TResult>(TState initial, Func<TState, bool> condition, Func<TState, IObservable<TResult>> resultSelector, Func<TState, TState> iterate);
        //
        // Summary:
        //     Generates an observable sequence by iterating a state from an initial state
        //     until the condition fails.
        public static IObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TResult> resultSelector, Func<TState, TState> iterate);
        //
        // Summary:
        //     Generates an observable sequence by iterating a state from an initial state
        //     until the condition fails. For each state, a value is generated.
        public static IObservable<TResult> Generate<TState, TResult>(this IScheduler scheduler, TState initial, Func<TState, bool> condition, Func<TState, IObservable<TResult>> resultSelector, Func<TState, TState> iterate);
        //
        // Summary:
        //     Generates an observable sequence by iterating a state from an initial state
        //     until the condition fails.
        public static IObservable<TResult> Generate<TState, TResult>(this IScheduler scheduler, TState initialState, Func<TState, bool> condition, Func<TState, TResult> resultSelector, Func<TState, TState> iterate);
        //
        // Summary:
        //     Generates an observable sequence by iterating a state from an initial state
        //     until the condition fails. For each state, a value is generated at a time
        //     interval dependent on the state.
        public static IObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TResult> resultSelector, Func<TState, TimeSpan> timeInterval, Func<TState, TState> iterate);
        //
        // Summary:
        //     Generates an observable sequence by iterating a state from an initial state
        //     until the condition fails. For each state, a value is generated at a time
        //     interval dependent on the state.
        public static IObservable<TResult> Generate<TState, TResult>(this IScheduler scheduler, TState initialState, Func<TState, bool> condition, Func<TState, TResult> resultSelector, Func<TState, TimeSpan> timeInterval, Func<TState, TState> iterate);
        //
        // Summary:
        //     Generates an observable sequence by iterating a state from an initial state
        //     until the condition fails. For each state, a value is generated.
        public static IObservable<TResult> GenerateInSequence<TState, TResult>(TState initial, Func<TState, IObservable<TResult>> resultSelector, Func<TState, TState> iterate);
        //
        // Summary:
        //     Generates an observable sequence by iterating a state from an initial state
        //     until the condition fails. For each state, a value is generated.
        public static IObservable<TResult> GenerateInSequence<TState, TResult>(TState initial, Func<TState, bool> condition, Func<TState, IObservable<TResult>> resultSelector, Func<TState, TState> iterate);
        //
        // Summary:
        //     Returns an enumerator that enumerates all values of the observable sequence.
        public static IEnumerator<TSource> GetEnumerator<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Groups the elements of an observable sequence according to a specified key
        //     selector function.
        public static IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector);
        //
        // Summary:
        //     Groups the elements of an observable sequence and selects the resulting elements
        //     by using a specified function.
        public static IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector);
        //
        // Summary:
        //     Groups the elements of an observable sequence according to a specified key
        //     selector function and comparer.
        public static IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer);
        //
        // Summary:
        //     Groups the elements of an observable sequence according to a specified key
        //     selector function and comparer and selects the resulting elements by using
        //     a specified function.
        public static IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer);
        //
        // Summary:
        //     Hides the identity of an observable sequence.
        public static IObservable<TSource> Hide<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Returns an observable sequence that contains only distinct contiguous values.
        public static IObservable<TSource> HoldUntilChanged<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Returns an observable sequence that contains only distinct contiguous values
        //     according to the keySelector.
        public static IObservable<TSource> HoldUntilChanged<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector);
        //
        // Summary:
        //     Returns an observable sequence that contains only distinct contiguous values
        //     according to the comparer.
        public static IObservable<TSource> HoldUntilChanged<TSource>(this IObservable<TSource> source, IEqualityComparer<TSource> comparer);
        //
        // Summary:
        //     Returns an observable sequence that contains only distinct contiguous values
        //     according to the keySelector and comparer.
        public static IObservable<TSource> HoldUntilChanged<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer);
        //
        // Summary:
        //     Returns an observable sequence that produces a value after each duration.
        public static IObservable<long> Interval(TimeSpan duration);
        //
        // Summary:
        //     Returns an observable sequence that produces a value after each duration.
        public static IObservable<long> Interval(this IScheduler scheduler, TimeSpan duration);
        //
        // Summary:
        //     Determins if an observable collection contains values.
        public static bool IsEmpty<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Joins together the results from several patterns.
        public static IObservable<TResult> Join<TResult>(params Plan<TResult>[] plans);
        //
        // Summary:
        //     Joins together the results from several patterns.
        public static IObservable<TResult> Join<TResult>(this IEnumerable<Plan<TResult>> plans);
        //
        // Summary:
        //     Returns the last value of an observable sequence.
        public static TSource Last<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Returns the last value of an observable sequence, or a default value if no
        //     value is found.
        public static TSource LastOrDefault<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Samples the most recent value (buffer of size one with consumption) in an
        //     observable sequence.
        public static IEnumerable<TSource> Latest<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Samples the most recent value (buffer of size one with consumption) in an
        //     observable sequence.
        public static IPropertyGetter<TSource> LatestValue<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Bind the source to the parameter without sharing subscription side-effects.
        public static IObservable<TResult> Let<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> function);
        //
        // Summary:
        //     Bind the source to the parameter so that it can be used multiple times without
        //     duplication of subscription side-effects.
        public static IObservable<TResult> Let<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> function, Func<ISubject<TSource>> subjectFactory);
        //
        // Summary:
        //     Returns an Int64 that represents the total number of values in an observable
        //     sequence.
        public static long LongCount<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Materializes the implicit notifications of an observable sequence as explicit
        //     notification values.
        public static IObservable<Notification<TSource>> Materialize<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Merges all the observable sequences into a single observable sequence.
        public static IObservable<TSource> Merge<TSource>(params IObservable<TSource>[] sources);
        //
        // Summary:
        //     Merges all the observable sequences into a single observable sequence.
        public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources);
        //
        // Summary:
        //     Merges an observable sequence of observable sequences into an observable
        //     sequence.
        public static IObservable<TSource> Merge<TSource>(this IObservable<IObservable<TSource>> source);
        //
        // Summary:
        //     Merges two observable sequences into a single observable sequence.
        public static IObservable<TSource> Merge<TSource>(this IObservable<TSource> leftSource, IObservable<TSource> rightSource);
        //
        // Summary:
        //     Samples the most recent value (buffer of size one without consumption) in
        //     an observable sequence.
        public static IEnumerable<TSource> MostRecent<TSource>(this IObservable<TSource> source, TSource initialValue);
        //
        // Summary:
        //     Samples the most recent value (buffer of size one without consumption) in
        //     an observable sequence.
        public static IPropertyGetter<TSource> MostRecentValue<TSource>(this IObservable<TSource> source, TSource initialValue);
        //
        // Summary:
        //     Returns a non-terminating observable sequence.
        public static IObservable<TValue> Never<TValue>();
        //
        // Summary:
        //     Samples the next value (blocking without buffering) from in an observable
        //     sequence.
        public static IEnumerable<TSource> Next<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Samples the next value (blocking without buffering) from in an observable
        //     sequence.
        public static IPropertyGetter<TSource> NextValue<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Asynchronously notify observers using the scheduler.
        public static IObservable<TSource> ObserveOn<TSource>(this IObservable<TSource> source, ControlScheduler scheduler);
        //
        // Summary:
        //     Asynchronously notify observers using the scheduler.
        public static IObservable<TSource> ObserveOn<TSource>(this IObservable<TSource> source, DispatcherScheduler scheduler);
        //
        // Summary:
        //     Asynchronously notify observers on the synchronization context.
        public static IObservable<TSource> ObserveOn<TSource>(this IObservable<TSource> source, SynchronizationContext synchronizationContext);
        //
        // Summary:
        //     Asynchronously notify observers using the Windows Forms control.
        public static IObservable<TSource> ObserveOn<TSource>(this IObservable<TSource> source, System.Windows.Forms.Control control);
        //
        // Summary:
        //     Asynchronously notify observers using the dispatcher.
        public static IObservable<TSource> ObserveOn<TSource>(this IObservable<TSource> source, System.Windows.Threading.Dispatcher dispatcher);
        //
        // Summary:
        //     Asynchronously notify observers using the current dispatcher.
        public static IObservable<TSource> ObserveOnDispatcher<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Continues an observable sequence that is terminated normally or by an exception
        //     with the next observable sequence.
        public static IObservable<TSource> OnErrorResumeNext<TSource>(params IObservable<TSource>[] sources);
        //
        // Summary:
        //     Continues an observable sequence that is terminated normally or by an exception
        //     with the next observable sequence.
        public static IObservable<TSource> OnErrorResumeNext<TSource>(this IEnumerable<IObservable<TSource>> sources);
        //
        // Summary:
        //     Continues an observable sequence that is terminated normally or by an exception
        //     with the next observable sequence.
        public static IObservable<TSource> OnErrorResumeNext<TSource>(this IEnumerable<IObservable<TSource>> sources, IScheduler scheduler);
        //
        // Summary:
        //     Continues an observable sequence that is terminated normally or by an exception
        //     with the next observable sequence.
        public static IObservable<TSource> OnErrorResumeNext<TSource>(this IObservable<TSource> first, IObservable<TSource> second);
        //
        // Summary:
        //     Continues an observable sequence that is terminated normally or by an exception
        //     with the next observable sequence.
        public static IObservable<TSource> OnErrorResumeNext<TSource>(this IScheduler scheduler, params IObservable<TSource>[] sources);
        //
        // Summary:
        //     Continues an observable sequence that is terminated normally or by an exception
        //     with the next observable sequence.
        public static IObservable<TSource> OnErrorResumeNext<TSource>(this IObservable<TSource> first, IScheduler scheduler, IObservable<TSource> second);
        //
        // Summary:
        //     Publishes the first value of the source observable sequence to a Subject.
        public static AsyncSubject<TSource> Prune<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Replays the first value of source to each use of the bound parameter.
        public static IObservable<TResult> Prune<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> function);
        //
        // Summary:
        //     Publishes the source observable sequence to a Subject.
        public static Subject<TSource> Publish<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Publishes the values of source to each use of the bound parameter.
        public static IObservable<TResult> Publish<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> function);
        //
        // Summary:
        //     Publishes the source observable sequence to a Subject starting with an initial
        //     value.
        public static BehaviorSubject<TSource> Publish<TSource>(this IObservable<TSource> source, TSource initialValue);
        //
        // Summary:
        //     Publishes the values of single to each use of the bound parameter starting
        //     with an intial value.
        public static IObservable<TResult> Publish<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> function, TSource initialValue);
        //
        // Summary:
        //     Generates an observable sequence of integral numbers within a specified range.
        public static IObservable<int> Range(int start, int count);
        //
        // Summary:
        //     Generates an observable sequence of integral numbers within a specified range.
        public static IObservable<int> Range(this IScheduler scheduler, int start, int count);
        //
        // Summary:
        //     Makes an observable sequence remotable.
        public static IObservable<TSource> Remotable<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Removes the timestamp from each value of an observable sequence.
        public static IObservable<TSource> RemoveTimeInterval<TSource>(this IObservable<TimeInterval<TSource>> source);
        //
        // Summary:
        //     Removes the timestamp from each value of an observable sequence.
        public static IObservable<TSource> RemoveTimestamp<TSource>(this IObservable<Timestamped<TSource>> source);
        //
        // Summary:
        //     Repeats the observable sequence indefinitely.
        public static IObservable<TSource> Repeat<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Generates an observable sequence that contains one repeated value.
        public static IObservable<TValue> Repeat<TValue>(TValue value);
        //
        // Summary:
        //     Repeats the observable sequence repeatCount times.
        public static IObservable<TSource> Repeat<TSource>(this IObservable<TSource> source, int repeatCount);
        //
        // Summary:
        //     Repeats the observable sequence indefinitely.
        public static IObservable<TSource> Repeat<TSource>(this IObservable<TSource> source, IScheduler scheduler);
        //
        // Summary:
        //     Generates an observable sequence that contains one repeated value.
        public static IObservable<TValue> Repeat<TValue>(this IScheduler scheduler, TValue value);
        //
        // Summary:
        //     Generates an observable sequence that contains one repeated value.
        public static IObservable<TValue> Repeat<TValue>(TValue value, int repeatCount);
        //
        // Summary:
        //     Repeats the observable sequence repeatCount times.
        public static IObservable<TSource> Repeat<TSource>(this IObservable<TSource> source, IScheduler scheduler, int repeatCount);
        //
        // Summary:
        //     Generates an observable sequence that contains one repeated value.
        public static IObservable<TValue> Repeat<TValue>(this IScheduler scheduler, TValue value, int repeatCount);
        //
        // Summary:
        //     Publishes the source observable sequence to a Subject which can replay all
        //     values.
        public static ReplaySubject<TSource> Replay<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Replays the values of source to each use of the bound parameter.
        public static IObservable<TResult> Replay<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> function);
        //
        // Summary:
        //     Publishes the source observable sequence to a Subject which can replay bufferSize
        //     values.
        public static ReplaySubject<TSource> Replay<TSource>(this IObservable<TSource> source, int bufferSize);
        //
        // Summary:
        //     Publishes the source observable sequence to a Subject which can replay all
        //     values.
        public static ReplaySubject<TSource> Replay<TSource>(this IObservable<TSource> source, TimeSpan window);
        //
        // Summary:
        //     Replays bufferSize values of source to each use of the bound parameter.
        public static IObservable<TResult> Replay<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> function, int bufferSize);
        //
        // Summary:
        //     Replays the values of source within window to each use of the bound parameter.
        public static IObservable<TResult> Replay<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> function, TimeSpan window);
        //
        // Summary:
        //     Publishes the source observable sequence to a Subject which can replay all
        //     values.
        public static ReplaySubject<TSource> Replay<TSource>(this IObservable<TSource> source, int bufferSize, TimeSpan window);
        //
        // Summary:
        //     Repeats the source observable sequence until it successfully terminates.
        public static IObservable<TSource> Retry<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Repeats the source observable sequence the retryCount times or until it successfully
        //     terminates.
        public static IObservable<TSource> Retry<TSource>(this IObservable<TSource> source, int retryCount);
        //
        // Summary:
        //     Repeats the source observable sequence until it successfully terminates.
        public static IObservable<TSource> Retry<TSource>(this IObservable<TSource> source, IScheduler scheduler);
        //
        // Summary:
        //     Repeats the source observable sequence the retryCount times or until it successfully
        //     terminates.
        public static IObservable<TSource> Retry<TSource>(this IObservable<TSource> source, IScheduler scheduler, int retryCount);
        //
        // Summary:
        //     Returns an observable sequence that contains a single value.
        public static IObservable<TValue> Return<TValue>(TValue value);
        //
        // Summary:
        //     Returns an observable sequence that contains a single value.
        public static IObservable<TValue> Return<TValue>(this IScheduler scheduler, TValue value);
        //
        // Summary:
        //     Samples the observable sequence at each interval.
        public static IObservable<TSource> Sample<TSource>(this IObservable<TSource> source, TimeSpan interval);
        //
        // Summary:
        //     Samples the observable sequence at each interval.
        public static IObservable<TSource> Sample<TSource>(this IObservable<TSource> source, IScheduler scheduler, TimeSpan interval);
        //
        // Summary:
        //     Applies an accumulator function over an observable sequence and returns each
        //     intermediate result.
        public static IObservable<TSource> Scan<TSource>(this IObservable<TSource> source, Func<TSource, TSource, TSource> accumulator);
        //
        // Summary:
        //     Applies an accumulator function over an observable sequence and returns each
        //     intermediate result. The specified seed value is used as the initial accumulator
        //     value.
        public static IObservable<TAccumulate> Scan<TSource, TAccumulate>(this IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator);
        //
        // Summary:
        //     Projects each value of an observable sequence into a new form by incorporating
        //     the element's index.
        public static IObservable<TResult> Select<TSource, TResult>(this IObservable<TSource> source, Func<TSource, int, TResult> selector);
        //
        // Summary:
        //     Projects each value of an observable sequence into a new form.
        public static IObservable<TResult> Select<TSource, TResult>(this IObservable<TSource> source, Func<TSource, TResult> selector);
        //
        // Summary:
        //     Projects each value of an observable sequence to an observable sequence and
        //     flattens the resulting observable sequences into one observable sequence.
        public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, IEnumerable<TResult>> selector);
        //
        // Summary:
        //     Projects each value of an observable sequence to an observable sequence and
        //     flattens the resulting observable sequences into one observable sequence.
        public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, IObservable<TResult>> selector);
        //
        // Summary:
        //     Projects each value of an observable sequence to an observable sequence and
        //     flattens the resulting observable sequences into one observable sequence.
        public static IObservable<TOther> SelectMany<TSource, TOther>(this IObservable<TSource> source, IObservable<TOther> other);
        //
        // Summary:
        //     Projects each value of an observable sequence to an observable sequence,
        //     flattens the resulting observable sequences into one observable sequence,
        //     and invokes a result selector function on each value therein.
        public static IObservable<TResult> SelectMany<TSource, TCollection, TResult>(this IObservable<TSource> source, Func<TSource, IObservable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector);
        //
        // Summary:
        //     Projects each value of an observable sequence to an observable sequence and
        //     flattens the resulting observable sequences into one observable sequence.
        public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, IObservable<TResult>> onNext, Func<Exception, IObservable<TResult>> onError, Func<IObservable<TResult>> onCompleted);
        //
        // Summary:
        //     Returns the only value of an observable sequence, and throws an exception
        //     if there is not exactly one value in the observable sequence.
        public static TSource Single<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Returns the only value of an observable sequence, or a default value if the
        //     observable sequence is empty; this method throws an exception if there is
        //     more than one value in the observable sequence.
        public static TSource SingleOrDefault<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Bypasses a specified number of values in an observable sequence and then
        //     returns the remaining values.
        public static IObservable<TSource> Skip<TSource>(this IObservable<TSource> source, int count);
        //
        // Summary:
        //     Returns the values from the source observable sequence only after the other
        //     observable sequence produces a value.
        public static IObservable<TSource> SkipUntil<TSource, TOther>(this IObservable<TSource> source, IObservable<TOther> other);
        //
        // Summary:
        //     Bypasses values in an observable sequence as long as a specified condition
        //     is true and then returns the remaining values.
        public static IObservable<TSource> SkipWhile<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate);
        //
        // Summary:
        //     Invokes the action asynchronously.
        public static IObservable<Unit> Start(Action action);
        //
        // Summary:
        //     Invokes the function asynchronously.
        public static IObservable<TSource> Start<TSource>(Func<TSource> function);
        //
        // Summary:
        //     Prepends a sequence values to an observable sequence.
        public static IObservable<TSource> StartWith<TSource>(this IObservable<TSource> source, params TSource[] values);
        //
        // Summary:
        //     Prepends a sequence values to an observable sequence.
        public static IObservable<TSource> StartWith<TSource>(this IObservable<TSource> source, IScheduler scheduler, params TSource[] values);
        //
        // Summary:
        //     Subscribes an observer to an enumerable sequence. Returns an object that
        //     can be used to unsubscribe the observer from the enumerable.
        public static IDisposable Subscribe<TSource>(this IEnumerable<TSource> source, IObserver<TSource> observer);
        //
        // Summary:
        //     Subscribes an observer to an enumerable sequence. Returns an object that
        //     can be used to unsubscribe the observer from the enumerable.
        public static IDisposable Subscribe<TSource>(this IEnumerable<TSource> source, IScheduler scheduler, IObserver<TSource> observer);
        //
        // Summary:
        //     Asynchronously subscribes and unsubscribes observers using the scheduler.
        public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, ControlScheduler scheduler);
        //
        // Summary:
        //     Asynchronously subscribes and unsubscribes observers using the scheduler.
        public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, DispatcherScheduler scheduler);
        //
        // Summary:
        //     Asynchronously subscribes and unsubscribes observers on the synchronization
        //     context.
        public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, SynchronizationContext context);
        //
        // Summary:
        //     Asynchronously subscribes and unsubscribes observers using the Windows Forms
        //     control.
        public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, System.Windows.Forms.Control control);
        //
        // Summary:
        //     Asynchronously subscribes and unsubscribes observers using the dispatcher.
        public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, System.Windows.Threading.Dispatcher dispatcher);
        //
        // Summary:
        //     Asynchronously subscribes and unsubscribes observers using the current dispatcher.
        public static IObservable<TSource> SubscribeOnDispatcher<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Transforms an observable sequence of observable sequences into an observable
        //     sequence producing values only from the most recent observable sequence.
        public static IObservable<TSource> Switch<TSource>(this IObservable<IObservable<TSource>> source);
        //
        // Summary:
        //     Synchronizes the observable sequence.
        public static IObservable<TSource> Synchronize<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Synchronizes the observable sequence.
        public static IObservable<TSource> Synchronize<TSource>(this IObservable<TSource> source, object gate);
        //
        // Summary:
        //     Returns a specified number of contiguous values from the start of an observable
        //     sequence.
        public static IObservable<TSource> Take<TSource>(this IObservable<TSource> source, int count);
        //
        // Summary:
        //     Returns the values from the source observable sequence until the other observable
        //     sequence produces a value.
        public static IObservable<TSource> TakeUntil<TSource, TOther>(this IObservable<TSource> source, IObservable<TOther> other);
        //
        // Summary:
        //     Returns values from an observable sequence as long as a specified condition
        //     is true, and then skips the remaining values.
        public static IObservable<TSource> TakeWhile<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate);
        //
        // Summary:
        //     Matches when the observable sequence has an available value and projects
        //     the value.
        public static Plan<TResult> Then<TSource, TResult>(this IObservable<TSource> source, Func<TSource, TResult> selector);
        //
        // Summary:
        //     Ignores values from an observable sequence which are followed by another
        //     value before dueTime.
        public static IObservable<TSource> Throttle<TSource>(this IObservable<TSource> source, TimeSpan dueTime);
        //
        // Summary:
        //     Ignores values from an observable sequence which are followed by another
        //     value before dueTime.
        public static IObservable<TSource> Throttle<TSource>(this IObservable<TSource> source, IScheduler scheduler, TimeSpan dueTime);
        //
        // Summary:
        //     Returns an observable sequence that terminates with an exception.
        public static IObservable<TValue> Throw<TValue>(Exception exception);
        //
        // Summary:
        //     Returns an observable sequence that terminates with an exception.
        public static IObservable<TValue> Throw<TValue>(this IScheduler scheduler, Exception exception);
        //
        // Summary:
        //     Records the time interval for each value of an observable sequence.
        public static IObservable<TimeInterval<TSource>> TimeInterval<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Returns either the observable sequence or an TimeoutException if dueTime
        //     elapses.
        public static IObservable<TSource> Timeout<TSource>(this IObservable<TSource> source, DateTimeOffset dueTime);
        //
        // Summary:
        //     Returns either the observable sequence or an TimeoutException if dueTime
        //     elapses.
        public static IObservable<TSource> Timeout<TSource>(this IObservable<TSource> source, TimeSpan dueTime);
        //
        // Summary:
        //     Returns the source observable sequence or the other observable sequence if
        //     dueTime elapses.
        public static IObservable<TSource> Timeout<TSource>(this IObservable<TSource> source, DateTimeOffset dueTime, IObservable<TSource> other);
        //
        // Summary:
        //     Returns either the observable sequence or an TimeoutException if dueTime
        //     elapses.
        public static IObservable<TSource> Timeout<TSource>(this IObservable<TSource> source, IScheduler scheduler, DateTimeOffset dueTime);
        //
        // Summary:
        //     Returns either the observable sequence or an TimeoutException if dueTime
        //     elapses.
        public static IObservable<TSource> Timeout<TSource>(this IObservable<TSource> source, IScheduler scheduler, TimeSpan dueTime);
        //
        // Summary:
        //     Returns the source observable sequence or the other observable sequence if
        //     dueTime elapses.
        public static IObservable<TSource> Timeout<TSource>(this IObservable<TSource> source, TimeSpan dueTime, IObservable<TSource> other);
        //
        // Summary:
        //     Returns the source observable sequence or the other observable sequence if
        //     dueTime elapses.
        public static IObservable<TSource> Timeout<TSource>(this IObservable<TSource> source, IScheduler scheduler, DateTimeOffset dueTime, IObservable<TSource> other);
        //
        // Summary:
        //     Returns the source observable sequence or the other observable sequence if
        //     dueTime elapses.
        public static IObservable<TSource> Timeout<TSource>(this IObservable<TSource> source, IScheduler scheduler, TimeSpan dueTime, IObservable<TSource> other);
        //
        // Summary:
        //     Returns an observable sequence that produces a value at dueTime.
        public static IObservable<long> Timer(DateTimeOffset dueTime);
        //
        // Summary:
        //     Returns an observable sequence that produces a value after the dueTime has
        //     elapsed.
        public static IObservable<long> Timer(TimeSpan dueTime);
        //
        // Summary:
        //     Returns an observable sequence that produces a value at dueTime and then
        //     after each interval.
        public static IObservable<long> Timer(DateTimeOffset dueTime, TimeSpan interval);
        //
        // Summary:
        //     Returns an observable sequence that produces a value at dueTime.
        public static IObservable<long> Timer(this IScheduler scheduler, DateTimeOffset dueTime);
        //
        // Summary:
        //     Returns an observable sequence that produces a value after the dueTime has
        //     elapsed.
        public static IObservable<long> Timer(this IScheduler scheduler, TimeSpan dueTime);
        //
        // Summary:
        //     Returns an observable sequence that produces a value after dueTime has elapsed
        //     and then after each interval.
        public static IObservable<long> Timer(TimeSpan dueTime, TimeSpan interval);
        //
        // Summary:
        //     Returns an observable sequence that produces a value at dueTime and then
        //     after each interval.
        public static IObservable<long> Timer(this IScheduler scheduler, DateTimeOffset dueTime, TimeSpan interval);
        //
        // Summary:
        //     Returns an observable sequence that produces a value after dueTime has elapsed
        //     and then after each interval.
        public static IObservable<long> Timer(this IScheduler scheduler, TimeSpan dueTime, TimeSpan interval);
        //
        // Summary:
        //     Records the timestamp for each value of an observable sequence.
        public static IObservable<Timestamped<TSource>> Timestamp<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Converts the action into an asynchronous function.
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action);
        //
        // Summary:
        //     Converts the action into an asynchronous function.
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action);
        //
        // Summary:
        //     Converts the action into an asynchronous function.
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action);
        //
        // Summary:
        //     Converts the action into an asynchronous function.
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action);
        //
        // Summary:
        //     Converts the action into an asynchronous function.
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action);
        //
        // Summary:
        //     Converts the action into an asynchronous function.
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action);
        //
        // Summary:
        //     Converts the action into an asynchronous function.
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action);
        //
        // Summary:
        //     Converts the action into an asynchronous function.
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action);
        //
        // Summary:
        //     Converts the action into an asynchronous function.
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action);
        //
        // Summary:
        //     Converts the action into an asynchronous function.
        public static Func<T1, T2, T3, T4, T5, T6, T7, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action);
        //
        // Summary:
        //     Converts the action into an asynchronous function.
        public static Func<T1, T2, T3, T4, T5, T6, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action);
        //
        // Summary:
        //     Converts the action into an asynchronous function.
        public static Func<T1, T2, T3, T4, T5, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action);
        //
        // Summary:
        //     Converts the action into an asynchronous function.
        public static Func<T1, T2, T3, T4, IObservable<Unit>> ToAsync<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action);
        //
        // Summary:
        //     Converts the action into an asynchronous function.
        public static Func<T1, T2, T3, IObservable<Unit>> ToAsync<T1, T2, T3>(this Action<T1, T2, T3> action);
        //
        // Summary:
        //     Converts the action into an asynchronous function.
        public static Func<T1, T2, IObservable<Unit>> ToAsync<T1, T2>(this Action<T1, T2> action);
        //
        // Summary:
        //     Converts the action into an asynchronous function.
        public static Func<TSource, IObservable<Unit>> ToAsync<TSource>(this Action<TSource> action);
        //
        // Summary:
        //     Converts the action into an asynchronous function.
        public static Func<IObservable<Unit>> ToAsync(this Action action);
        //
        // Summary:
        //     Converts the function into an asynchronous function.
        public static Func<T, IObservable<TResult>> ToAsync<T, TResult>(this Func<T, TResult> function);
        //
        // Summary:
        //     Converts the function into an asynchronous function.
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> function);
        //
        // Summary:
        //     Converts the function into an asynchronous function.
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> function);
        //
        // Summary:
        //     Converts the function into an asynchronous function.
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> function);
        //
        // Summary:
        //     Converts the function into an asynchronous function.
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> function);
        //
        // Summary:
        //     Converts the function into an asynchronous function.
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function);
        //
        // Summary:
        //     Converts the function into an asynchronous function.
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function);
        //
        // Summary:
        //     Converts the function into an asynchronous function.
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function);
        //
        // Summary:
        //     Converts the function into an asynchronous function.
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function);
        //
        // Summary:
        //     Converts the function into an asynchronous function.
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function);
        //
        // Summary:
        //     Converts the function into an asynchronous function.
        public static Func<T1, T2, T3, T4, T5, T6, T7, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> function);
        //
        // Summary:
        //     Converts the function into an asynchronous function.
        public static Func<T1, T2, T3, T4, T5, T6, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> function);
        //
        // Summary:
        //     Converts the function into an asynchronous function.
        public static Func<T1, T2, T3, T4, T5, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> function);
        //
        // Summary:
        //     Converts the function into an asynchronous function.
        public static Func<T1, T2, T3, T4, IObservable<TResult>> ToAsync<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> function);
        //
        // Summary:
        //     Converts the function into an asynchronous function.
        public static Func<T1, T2, T3, IObservable<TResult>> ToAsync<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> function);
        //
        // Summary:
        //     Converts the function into an asynchronous function.
        public static Func<T1, T2, IObservable<TResult>> ToAsync<T1, T2, TResult>(this Func<T1, T2, TResult> function);
        //
        // Summary:
        //     Converts the function into an asynchronous function.
        public static Func<IObservable<TResult>> ToAsync<TResult>(this Func<TResult> function);
        //
        // Summary:
        //     Converts an observable sequence to an enumerable sequence.
        public static IEnumerable<TSource> ToEnumerable<TSource>(this IObservable<TSource> source);
        //
        // Summary:
        //     Converts an enumerable sequence to an observable sequence.
        public static IObservable<TSource> ToObservable<TSource>(this IEnumerable<TSource> source);
        //
        // Summary:
        //     Converts a property getter to an observable sequence.
        public static IObservable<TValue> ToObservable<TValue>(this IPropertyGetter<TValue> property);
        //
        // Summary:
        //     Returns an observable sequence that contains the values of the underlying
        //     task.
        public static IObservable<TResult> ToObservable<TResult>(this Task<TResult> task);
        //
        // Summary:
        //     Returns an observable sequence that contains the values of the underlying
        //     task.
        public static IObservable<Unit> ToObservable(this Task task);
        //
        // Summary:
        //     Converts an enumerable sequence to an observable sequence.
        public static IObservable<TSource> ToObservable<TSource>(this IEnumerable<TSource> source, IScheduler scheduler);
        //
        // Summary:
        //     Retrieves resource from resourceSelector for use in resourceUsage and disposes
        //     the resource once the resulting observable sequence terminates.
        public static IObservable<TSource> Using<TSource, TResource>(Func<TResource> resourceSelector, Func<TResource, IObservable<TSource>> resourceUsage) where TResource : IDisposable;
        //
        // Summary:
        //     Filters the values of an observable sequence based on a predicate.
        public static IObservable<TSource> Where<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate);
        //
        // Summary:
        //     Filters the values of an observable sequence based on a predicate by incorporating
        //     the element's index.
        public static IObservable<TSource> Where<TSource>(this IObservable<TSource> source, Func<TSource, int, bool> predicate);
        //
        // Summary:
        //     Merges two observable sequences into one observable sequence by using the
        //     selector function.
        public static IObservable<TResult> Zip<TLeft, TRight, TResult>(this IObservable<TLeft> leftSource, IObservable<TRight> rightSource, Func<TLeft, TRight, TResult> selector);
    }
}
