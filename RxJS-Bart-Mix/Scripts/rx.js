(function ()
{
    var _undefined;
    var root;
    var global = this;
    var outOfRange = "Index out of range";
    if (typeof ProvideCustomRxRootObject == "undefined")
    {
        root = global.Rx = {};
    }
    else
    {
        root = ProvideCustomRxRootObject();
    }

    var _nothing = function () { };

    var defaultNow = function () { return new Date().getTime(); };

    var defaultComparer = function (a, b) { return a === b };
    var identity = function (x) { return x; }

    var disposableCreate = function (action)
    {
        return { Dispose: action };
    };

    var disposableEmpty = { Dispose: _nothing };

    root.Disposable = { Create: disposableCreate, Empty: disposableEmpty };

    var booleanDisposable = root.BooleanDisposable = function ()
    {
        var _isDisposed = false;
        this.GetIsDisposed = function () { return _isDisposed; };
        this.Dispose = function () { _isDisposed = true; };
    };

    var innerDisposable = function (disposable)
    {
        var innerIsDisposed = false;
        disposable._count++;

        this.Dispose = function ()
        {
            var shouldDispose = false;

            if (!disposable._isUnderlyingDisposed)
            {
                if (!this._isInnerDisposed)
                {
                    this._isInnerDisposed = true;
                    disposable._count--;
                    if (disposable._count == 0 && disposable._isPrimaryDisposed)
                    {
                        disposable._isUnderlyingDisposed = true;
                        shouldDispose = true;
                    }
                }
            }
            if (shouldDispose)
                disposable._underlyingDisposable.Dispose();
        };
    };

    var refCountDisposable = root.RefCountDisposable = function (disposable)
    {
        this._isPrimaryDisposed = false;
        this._isUnderlyingDisposed = false;
        this._underlyingDisposable = disposable;
        this._count = 0;

        this.Dispose = function ()
        {
            var shouldDispose = false;
            if (!this._isUnderlyingDisposed)
            {
                if (!this._isPrimaryDisposed)
                {
                    this._isPrimaryDisposed = true;
                    if (this._count == 0)
                    {
                        this._isUnderlyingDisposed = true;
                        shouldDispose = true;
                    }
                }
            }
            if (shouldDispose)
                this._underlyingDisposable.Dispose();
        };
        this.GetDisposable = function ()
        {
            if (this._isUnderlyingDisposed)
                return disposableEmpty;
            else
                return new innerDisposable(this);
        };
    };

    var compositeDisposable = root.CompositeDisposable = function ()
    {
        var _disposables = new list();
        for (var i = 0; i < arguments.length; i++)
        {
            _disposables.Add(arguments[i]);
        }
        var _isDisposed = false;

        this.GetCount = function () { return _disposables.GetCount(); };
        this.Add = function (item)
        {
            if (!_isDisposed)
                _disposables.Add(item);
            else
                item.Dispose();
        };
        this.Remove = function (item, keepAlive)
        {
            if (!_isDisposed)
            {
                var shouldDispose = _disposables.Remove(item);
                if (!keepAlive & shouldDispose)
                    item.Dispose();
            }
        };
        this.Dispose = function ()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                this.Clear();
            }
        };
        this.Clear = function ()
        {
            for (var i = 0; i < _disposables.GetCount(); i++)
            {
                _disposables.GetItem(i).Dispose();
            }
            _disposables.Clear();
        };
    };

    var mutableDisposable = root.MutableDisposable = function ()
    {
        var _isDisposed = false;
        var _current;

        this.Get = function () { return _current; },
        this.Replace = function (disposable)
        {
            if (_isDisposed && disposable !== _undefined)
                disposable.Dispose();
            else
            {
                if (_current !== _undefined)
                {
                    _current.Dispose();
                }
                _current = disposable;
            }
        };
        this.Dispose = function ()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (_current !== _undefined)
                    _current.Dispose();
            }
        };
    };

    var cloneArray = function (arr)
    {
        var duplicate = [];
        for (var i = 0; i < arr.length; i++)
        {
            duplicate.push(arr[i]);
        }
        return duplicate;
    };

    var list = root.List = function (comparer)
    {
        var _items = [];
        var _count = 0;
        var _comparer = comparer !== _undefined ? comparer : defaultComparer;
        this.Add = function (item)
        {
            _items[_count] = item;
            _count++;
        };
        this.RemoveAt = function (index)
        {
            if (index < 0 || index >= _count)
            {
                throw outOfRange;
            }
            if (index == 0)
            {
                _items.shift();
                _count--;
            }
            else
            {
                _items.splice(index, 1);
                _count--;
            }
        };
        this.IndexOf = function (item)
        {
            for (var i = 0; i < _count; i++)
            {
                if (_comparer(item, _items[i]))
                    return i;
            }
            return -1;
        };
        this.Remove = function (item)
        {
            var index = this.IndexOf(item);
            if (index == -1)
                return false;
            this.RemoveAt(index);
            return true;
        };
        this.Clear = function ()
        {
            _items = [];
            _count = 0;
        };
        this.GetCount = function ()
        {
            return _count;
        };
        this.GetItem = function (index)
        {
            if (index < 0 || index >= _count)
            {
                throw outOfRange;
            }
            return _items[index];
        };
        this.SetItem = function (index, item)
        {
            if (index < 0 || index >= _count)
            {
                throw outOfRange;
            }
            _items[index] = item;
        };
        this.ToArray = function ()
        {
            var results = [];
            for (var i = 0; i < this.GetCount(); i++)
            {
                results.push(this.GetItem(i));
            }
            return results;
        };
    };

    var priorityQueue = function (comparer)
    {
        if (comparer === null)
        {
            comparer = defaultComparer;
        }
        this._comparer = comparer;
        var defaultSize = 4;
        this._items = new Array(defaultSize);
        this._size = 0;
    };

    priorityQueue.prototype._isHigherPriority = function (left, right)
    {
        return this._comparer(this._items[left], this._items[right]) < 0;
    };

    priorityQueue.prototype._percolate = function (index)
    {
        if (index >= this._size || index < 0)
            return;
        var parent = (index - 1) >> 1;
        if (parent < 0 || parent == index)
            return;
        if (this._isHigherPriority(index, parent))
        {
            var temp = this._items[index];
            this._items[index] = this._items[parent];
            this._items[parent] = temp;
            this._percolate(parent);
        }
    };

    priorityQueue.prototype._heapify = function (index)
    {
        if (index === _undefined)
            index = 0;

        var left = 2 * index + 1;
        var right = 2 * index + 2;
        var first = index;

        if (left < this._size && this._isHigherPriority(left, first))
            first = left;

        if (right < this._size && this._isHigherPriority(right, first))
            first = right;

        if (first != index)
        {
            var temp = this._items[index];
            this._items[index] = this._items[first];
            this._items[first] = temp;
            this._heapify(first);
        }
    };

    priorityQueue.prototype.GetCount = function ()
    {
        return this._size;
    };

    priorityQueue.prototype.Peek = function ()
    {
        if (this._size == 0)
            throw "Heap is empty.";

        return this._items[0];
    };

    priorityQueue.prototype.Dequeue = function ()
    {
        var result = this.Peek();
        this._items[0] = this._items[--this._size];
        delete this._items[this._size];
        this._heapify();
        return result;
    };

    priorityQueue.prototype.Enqueue = function (item)
    {
        var index = this._size++;
        this._items[index] = item;
        this._percolate(index);
    };

    var schedulerBase = root.Scheduler = function (schedule, scheduleWithTime, now)
    {
        this.Schedule = schedule;
        this.ScheduleWithTime = scheduleWithTime;
        this.Now = now;
        this.ScheduleRecursive = function (action)
        {
            var scheduler = this;
            var group = new compositeDisposable();
            var recursiveAction;
            recursiveAction = function ()
            {
                action(function ()
                {
                    var isAdded = false;
                    var isDone = false;
                    var d;
                    d = scheduler.Schedule(function ()
                    {
                        recursiveAction();
                        if (isAdded)
                            group.Remove(d);
                        else
                            isDone = true;
                    });
                    if (!isDone)
                    {
                        group.Add(d);
                        isAdded = true;
                    }
                });
            };
            group.Add(scheduler.Schedule(recursiveAction));
            return group;
        };
        this.ScheduleRecursiveWithTime = function (action, dueTime)
        {
            var scheduler = this;
            var group = new compositeDisposable();
            var recursiveAction;
            recursiveAction = function ()
            {
                action(function (dt)
                {
                    var isAdded = false;
                    var isDone = false;
                    var d;
                    d = scheduler.ScheduleWithTime(function ()
                    {
                        recursiveAction();
                        if (isAdded)
                            group.Remove(d);
                        else
                            isDone = true;
                    }, dt);
                    if (!isDone)
                    {
                        group.Add(d);
                        isAdded = true;
                    }
                });
            };
            group.Add(scheduler.ScheduleWithTime(recursiveAction, dueTime));
            return group;
        };
    };

    var virtualScheduler = root.VirtualScheduler = function (comparer, increment, toDate, fromTimeSpan)
    {
        var base = new schedulerBase(function (action)
        {
            return this.ScheduleWithTime(action, 0)
        },
        function (action, dueTime)
        {
            return this.ScheduleVirtual(action, fromTimeSpan(dueTime));
        },
        function ()
        {
            return toDate(this._ticks);
        });
        base.ScheduleVirtual = function (action, dueTime)
        {
            var disposable = new booleanDisposable();

            var runAt = increment(this._ticks, dueTime);

            var run = function ()
            {
                if (!disposable.IsDisposed)
                    action();
            };

            var si = new scheduledItem(run, runAt);

            this._queue.Enqueue(si);

            return disposable;
        };
        base.Run = function ()
        {
            while (this._queue.GetCount() > 0)
            {
                var item = this._queue.Dequeue();
                this._ticks = item._dueTime;
                item._action();
            }
        }
        base.RunTo = function (time)
        {
            while (this._queue.GetCount() > 0 && this._comparer(this._queue.Peek()._dueTime, time) <= 0)
            {
                var item = this._queue.Dequeue();
                this._ticks = item._dueTime;
                item._action();
            }
        }
        base.GetTicks = function ()
        {
            return this._ticks;
        }
        base._ticks = 0;
        base._queue = new priorityQueue(function (a, b) { return comparer(a._dueTime, b._dueTime); });
        base._comparer = comparer;
        return base;
    }

    var testScheduler = root.TestScheduler = function ()
    {
        var base = new virtualScheduler(function (a, b) { return a - b; },
            function (a, b) { return a + b; },
            function (absolute) { return new Date(absolute); },
            function (span) { if (span <= 0) return 1; return span; });
        return base;
    };

    var currentThreadScheduler = new schedulerBase(function (action)
    {
        return this.ScheduleWithTime(action, 0);
    }, function (action, time)
    {
        var dt = this.Now() + time;
        var si = new scheduledItem(action, dt);
        if (this._queue === _undefined)
        {
            var t = new trampoline();
            try
            {
                this._queue.Enqueue(si);
                t._run();
            }
            finally
            {
                t._dispose();
            }
        }
        else
        {
            this._queue.Enqueue(si);
        }
        return si._getDisposable();
    }, defaultNow);

    currentThreadScheduler._ensureTrampoline = function (action)
    {
        if (this._queue === _undefined)
        {
            var t = new trampoline();
            try
            {
                action();
                t._run();
            }
            finally
            {
                t._dispose();
            }
        }
        else
            action();
    };

    schedulerBase.CurrentThread = currentThreadScheduler;

    var trampoline = function ()
    {
        currentThreadScheduler._queue = new priorityQueue(function (a, b) { try { return a._dueTime - b._dueTime; } catch (e) { debugger; } });
        this._dispose = function ()
        {
            currentThreadScheduler._queue = _undefined;
        }

        this._run = function ()
        {
            while (currentThreadScheduler._queue.GetCount() > 0)
            {
                var item = currentThreadScheduler._queue.Dequeue();
                if (!item._getIsCanceled())
                {
                    while (item._dueTime - currentThreadScheduler.Now() > 0)
                    {
                    }
                    if (!item._getIsCanceled())
                    {
                        item._action();
                    }
                }
            }
        }
    }

    var scheduledItemId = 0;
    var scheduledItem = function (action, dueTime)
    {
        this._id = scheduledItemId++;
        this._action = action;
        this._dueTime = dueTime;
        this._disposable = new booleanDisposable();
        this._getIsCanceled = function ()
        {
            return this._disposable.GetIsDisposed();
        };
        this._getDisposable = function ()
        {
            return this._disposable;
        };

    }

    var immediateScheduler = new schedulerBase(function (action)
    {
        action();
        return disposableEmpty;
    },
    function (action, time)
    {
        while (this.Now < time)
        {
        }
        action();
    }, defaultNow);

    schedulerBase.Immediate = immediateScheduler;

    var timeoutScheduler = new schedulerBase(
    function (action)
    {
        var id = global.setTimeout(action, 0);
        return disposableCreate(function () { global.clearTimeout(id); });
    },
    function (action, dueTime)
    {
        var id = global.setTimeout(action, dueTime);
        return disposableCreate(function () { global.clearTimeout(id); });
    }, defaultNow);

    schedulerBase.Timeout = timeoutScheduler;

    var observer = root.Observer = function (onNext, onError, onCompleted)
    {
        this.OnNext = onNext === _undefined ? _nothing : onNext;
        this.OnError = onError === _undefined ? function (e) { throw e; } : onError;
        this.OnCompleted = onCompleted === _undefined ? _nothing : onCompleted;

        this.AsObserver = function ()
        {
            var parent = this;
            return new observer(function (value)
            {
                parent.OnNext(value);
            },
            function (exception)
            {
                parent.OnError(exception);
            },
            function ()
            {
                parent.OnCompleted();
            });
        }
    };

    var observerCreate = observer.Create = function (onNext, onError, onCompleted)
    {
        return new observer(onNext, onError, onCompleted);
    }

    var observable = root.Observable = function (subscribe)
    {
        this._subscribe = subscribe;
    };

    var observableCreateWithDisposable = observable.CreateWithDisposable = function (subscribe)
    {
        return new observable(subscribe);
    };

    var observableCreate = observable.Create = function (subscribe)
    {
        return observableCreateWithDisposable(function (observer) { return disposableCreate(subscribe(observer)); });
    };

    var removeValue = function ()
    {
        return this.Select(function (value)
        {
            return value.Value;
        });
    };
    observable.prototype =
    {
        Subscribe: function (observerOrOnNext, onError, onCompleted)
        {
            var subscriber;
            if (arguments.length == 0 || arguments.length > 1 || typeof observerOrOnNext == "function")
            {
                subscriber = new observer(observerOrOnNext, onError, onCompleted);
            }
            else
            {
                subscriber = observerOrOnNext;
            }
            return this._SubscribeObserver(subscriber);
        },
        _SubscribeObserver: function (subscriber)
        {
            var isStopped = false;
            var subscription = new mutableDisposable();
            var parent = this;
            currentThreadScheduler._ensureTrampoline(function ()
            {
                var nested = new observer(
                function (value)
                {
                    if (!isStopped)
                        subscriber.OnNext(value);
                },
                function (error)
                {
                    if (!isStopped)
                    {
                        isStopped = true;
                        subscription.Dispose();
                        subscriber.OnError(error);
                    }
                },
                function ()
                {
                    if (!isStopped)
                    {
                        isStopped = true;
                        subscription.Dispose();
                        subscriber.OnCompleted();
                    }
                });
                subscription.Replace(parent._subscribe(nested));
            });
            return new compositeDisposable(subscription, disposableCreate(function () { isStopped = true; }));
        },
        Select: function (selector)
        {
            var parent = this;
            return observableCreateWithDisposable(function (subscriber)
            {
                var count = 0;
                return parent.Subscribe(new observer(
                function (value)
                {
                    var newValue;
                    try
                    {
                        newValue = selector(value, count++);
                    }
                    catch (e)
                    {
                        subscriber.OnError(e);
                        return;
                    }
                    subscriber.OnNext(newValue);
                },
                function (e) { subscriber.OnError(e); },
                function () { subscriber.OnCompleted(); }));
            });
        },
        Let: function (func, subjectFactory)
        {
            if (subjectFactory === _undefined)
                return func(this);

            var parent = this;

            return observableCreateWithDisposable(function (observer)
            {
                var subject = subjectFactory();
                var boundObservable;
                try
                {
                    boundObservable = func(subject);
                }
                catch (e)
                {
                    return observableThrow(e).Subscribe(observer);
                }

                var derivedSubscription = new mutableDisposable();
                var sourceSubscription = new mutableDisposable();
                var group = new compositeDisposable(sourceSubscription, derivedSubscription);

                derivedSubscription.Replace(
                    boundObservable.Subscribe(function (value)
                    {
                        observer.OnNext(value);
                    },
                    function (exception)
                    {
                        observer.OnError(exception);
                        group.Dispose();
                    },
                    function ()
                    {
                        observer.OnCompleted();
                        group.Dispose();
                    }));

                sourceSubscription.Replace(parent.Subscribe(subject));

                return group;
            });
        },
        MergeObservable: function ()
        {
            var parent = this;

            return observableCreateWithDisposable(function (observer)
            {
                var isStopped = false;
                var group = new compositeDisposable();
                var outerSubscription = new mutableDisposable();
                group.Add(outerSubscription);

                outerSubscription.Replace(parent.Subscribe(
                    function (innerSource)
                    {
                        var innerSubscription = new mutableDisposable();
                        group.Add(innerSubscription);
                        innerSubscription.Replace(innerSource.Subscribe(
                        function (x)
                        {
                            observer.OnNext(x);
                        },
                        function (exception)
                        {
                            observer.OnError(exception);
                        },
                        function ()
                        {
                            group.Remove(innerSubscription)
                            if (group.GetCount() == 1 && isStopped)
                                observer.OnCompleted();
                        }));
                    },
                    function (exception)
                    {
                        observer.OnError(exception);
                    },
                    function ()
                    {
                        isStopped = true;
                        if (group.GetCount() == 1)
                            observer.OnCompleted();
                    }
                    ));

                return group;
            });
        },
        _Shift: function (func, args)
        {
            var items = cloneArray(args);
            items.unshift(this);
            return func(items);
        },
        Concat: function ()
        {
            return this._Shift(observableConcat, arguments);
        },
        Merge: function ()
        {
            return this._Shift(observableMerge, arguments);
        },
        Catch: function ()
        {
            return this._Shift(observableCatch, arguments);
        },
        OnErrorResumeNext: function ()
        {
            return this._Shift(observableOnErrorResumeNext, arguments);
        },
        Zip: function (right, selector)
        {
            var left = this;
            return observableCreateWithDisposable(function (observer)
            {
                var done = false;
                var qLeft = [];
                var qRight = [];
                var leftCompleted = false;
                var rightCompleted = false;
                var group = new compositeDisposable();
                var onError = function (error)
                {
                    group.Dispose();
                    qLeft = _undefined;
                    qRight = _undefined;
                    observer.OnError(error);
                };
                group.Add(left.Subscribe(function (value)
                {
                    if (rightCompleted)
                    {
                        observer.OnCompleted();
                        return;
                    }
                    if (qRight.length > 0)
                    {
                        var rightValue = qRight.shift();
                        var newValue;
                        try
                        {
                            newValue = selector(value, rightValue);
                        }
                        catch (e)
                        {
                            group.Dispose();
                            observer.OnError(e);
                            return;
                        }
                        observer.OnNext(newValue);
                    }
                    else
                    {
                        qLeft.push(value);
                    }
                },
                onError,
                function ()
                {
                    if (rightCompleted)
                    {
                        observer.OnCompleted();
                        return;
                    }
                    leftCompleted = true;
                }));
                group.Add(right.Subscribe(function (value)
                {

                    if (leftCompleted)
                    {
                        observer.OnCompleted();
                        return;
                    }

                    if (qLeft.length > 0)
                    {
                        var leftValue = qLeft.shift();
                        var newValue;
                        try
                        {
                            newValue = selector(leftValue, value);
                        }
                        catch (e)
                        {
                            group.Dispose();
                            observer.OnError(e);
                            return;
                        }
                        observer.OnNext(newValue);
                    }
                    else
                    {
                        qRight.push(value);
                    }
                },
                onError,
                 function ()
                 {
                     if (leftCompleted)
                     {
                         observer.OnCompleted();
                         return;
                     }
                     rightCompleted = true;
                 }));
                return group;
            });
        },
        CombineLatest: function (right, selector)
        {
            var left = this;
            return observableCreateWithDisposable(function (observer)
            {
                var done = false;
                var hasLeft = false;
                var hasRight = false;
                var leftValue;
                var rightValue;
                var leftCompleted = false;
                var rightCompleted = false;
                var group = new compositeDisposable();
                var onError = function (error)
                {
                    group.Dispose();
                    observer.OnError(error);
                };

                group.Add(left.Subscribe(function (value)
                {
                    if (rightCompleted)
                    {
                        observer.OnCompleted();
                        return;
                    }

                    if (hasRight)
                    {
                        var newValue;
                        try
                        {
                            newValue = selector(value, rightValue);
                        }
                        catch (e)
                        {
                            group.Dispose();
                            observer.OnError(e);
                            return;
                        }
                        observer.OnNext(newValue);
                    }
                    leftValue = value;
                    hasLeft = true;
                }, onError, function ()
                {
                    if (rightCompleted)
                    {
                        observer.OnCompleted();
                        return;
                    }
                    leftCompleted = true;
                }));
                group.Add(right.Subscribe(function (value)
                {
                    if (leftCompleted)
                    {
                        observer.OnCompleted();
                        return;
                    }

                    if (hasLeft)
                    {
                        var newValue;
                        try
                        {
                            newValue = selector(leftValue, value);
                        }
                        catch (e)
                        {
                            group.Dispose();
                            observer.OnError(e);
                            return;
                        }
                        observer.OnNext(newValue);
                    }
                    rightValue = value;
                    hasRight = true;
                },
                onError, function ()
                {
                    if (leftCompleted)
                    {
                        observer.OnCompleted();
                        return;
                    }
                    rightCompleted = true;
                }));
            });
        },
        Switch: function ()
        {
            var parent = this;
            return observableCreateWithDisposable(function (observer)
            {
                var isStopped = false;
                var innerSubscription = new mutableDisposable();
                var subscription = new mutableDisposable();

                subscription.Replace(parent.Subscribe(
                function (innerSource)
                {
                    if (!isStopped)
                    {
                        var d = new mutableDisposable();
                        d.Replace(innerSource.Subscribe(
                            function (value)
                            {
                                observer.OnNext(value);
                            },
                            function (exception)
                            {
                                subscription.Dispose();
                                innerSubscription.Dispose();
                                observer.OnError(exception);
                            },
                            function ()
                            {
                                innerSubscription.Replace(_undefined);
                                if (isStopped)
                                    observer.OnCompleted();
                            }
                        ));
                        innerSubscription.Replace(d);
                    }
                },
                function (exception)
                {
                    innerSubscription.Dispose();
                    observer.OnError(exception);
                },
                function ()
                {
                    isStopped = true;
                    if (innerSubscription.Get() === _undefined)
                        observer.OnCompleted();
                }));
                return new compositeDisposable(subscription, innerSubscription);
            });
        },
        TakeUntil: function (other)
        {
            var parent = this;
            return observableCreateWithDisposable(function (observer)
            {
                var group = new compositeDisposable();
                group.Add(other.Subscribe(
                function () { observer.OnCompleted(); group.Dispose(); },
                function (e) { observer.OnError(e); },
                function () { }));

                group.Add(parent.Subscribe(observer));

                return group;
            });
        },
        SkipUntil: function (other)
        {
            var parent = this;
            return observableCreateWithDisposable(function (subscriber)
            {
                var skipping = true;
                var group = new compositeDisposable();

                group.Add(other.Subscribe(
            function () { skipping = false; },
            function (e) { subscriber.OnError(e); },
            _nothing));

                group.Add(parent.Subscribe(new observer(
                function (v)
                {
                    if (!skipping)
                    {
                        subscriber.OnNext(v);
                    }
                },
                function (e)
                {
                    subscriber.OnError(e);
                },
                function ()
                {
                    if (!skipping)
                    {
                        subscriber.OnCompleted();
                    }
                }
            )));

                return group;
            });
        },
        Scan1: function (accumulator)
        {
            var parent = this;
            return observableDefer(function ()
            {
                var accumulation;
                var hasAccumulation = false;
                return parent.Select(function (x)
                {
                    if (hasAccumulation)
                        accumulation = accumulator(accumulation, x);
                    else
                    {
                        accumulation = x;
                        hasAccumulation = true;
                    }
                    return accumulation;
                });
            });
        },
        Scan: function (seed, accumulator)
        {
            var parent = this;
            return observableDefer(function ()
            {
                var accumulation;
                var hasAccumulation = false;
                return parent.Select(function (x)
                {
                    if (hasAccumulation)
                        accumulation = accumulator(accumulation, x);
                    else
                    {
                        accumulation = accumulator(seed, x);
                        hasAccumulation = true;
                    }
                    return accumulation;
                });
            });
        },
        Scan0: function (seed, accumulator)
        {
            var parent = this;
            return observableCreateWithDisposable(function (subscriber)
            {
                var current = seed;
                var atBegin = true;
                return parent.Subscribe(function (value)
                {
                    if (atBegin)
                    {
                        atBegin = false;
                        subscriber.OnNext(current);
                    }
                    try
                    {
                        current = accumulator(current, value);
                    }
                    catch (e)
                    {
                        subscriber.OnError(e);
                        return;
                    }
                    subscriber.OnNext(current);
                },
                function (exception)
                {
                    if (atBegin)
                    {
                        subscriber.OnNext(current);
                    }
                    subscriber.OnError(exception);
                },
                function ()
                {
                    if (atBegin)
                    {
                        subscriber.OnNext(current);
                    }
                    subscriber.OnCompleted();
                });
            });
        },
        Finally: function (finallyAction)
        {
            var parent = this;
            return observableCreate(function (observer)
            {
                var subscription = parent.Subscribe(observer);
                return function ()
                {
                    try
                    {
                        subscription.Dispose();
                        finallyAction();
                    }
                    catch (e)
                    {
                        finallyAction();
                        throw e;
                    }
                };
            });
        },
        Do: function (observerOrOnNext, onError, onCompleted)
        {
            var sideEffector;
            if (arguments.length == 0 || arguments.length > 1 || typeof observerOrOnNext == "function")
            {
                sideEffector = new observer(observerOrOnNext, onError !== _undefined ? onError : _nothing, onCompleted);
            }
            else
            {
                sideEffector = observerOrOnNext;
            }
            var parent = this;
            return observableCreateWithDisposable(function (subscriber)
            {
                return parent.Subscribe(new observer(
                function (value)
                {
                    try
                    {
                        sideEffector.OnNext(value);
                    }
                    catch (ex)
                    {
                        subscriber.OnError(ex);
                        return;
                    }
                    subscriber.OnNext(value);
                },
                function (exception)
                {
                    if (onError !== _undefined)
                    {
                        try
                        {
                            sideEffector.OnError(exception);
                        }
                        catch (ex)
                        {
                            subscriber.OnError(ex);
                            return;
                        }
                    }
                    subscriber.OnError(exception);
                },
                function ()
                {
                    if (onCompleted !== _undefined)
                    {
                        try
                        {
                            sideEffector.OnCompleted();
                        }
                        catch (ex)
                        {
                            subscriber.OnError(ex);
                            return;
                        }
                    }
                    subscriber.OnCompleted();
                }));
            });
        },
        Where: function (predicate)
        {
            var parent = this;
            return observableCreateWithDisposable(function (subscriber)
            {
                var count = 0;
                return parent.Subscribe(new observer(
            function (value)
            {
                var shouldRun = false;
                try
                {
                    shouldRun = predicate(value, count++);
                }
                catch (e)
                {
                    subscriber.OnError(e);
                    return;
                }
                if (shouldRun)
                    subscriber.OnNext(value);
            },
            function (e) { subscriber.OnError(e); }, function () { subscriber.OnCompleted(); }));
            });
        },
        Take: function (count, scheduler)
        {
            if (scheduler === _undefined)
                scheduler = immediateScheduler;
            var parent = this;
            return observableCreateWithDisposable(function (subscriber)
            {
                if (count <= 0)
                {
                    parent.Subscribe().Dispose();
                    return observableEmpty(scheduler).Subscribe(subscriber);
                }

                var remaining = count;

                return parent.Subscribe(new observer(
            function (value)
            {
                if (remaining-- > 0)
                {
                    subscriber.OnNext(value);
                    if (remaining == 0)
                        subscriber.OnCompleted();
                }
            },
            function (e) { subscriber.OnError(e); }, function () { subscriber.OnCompleted(); }));
            });
        },
        GroupBy: function (keySelector, elementSelector, keySerializer)
        {
            if (keySelector === _undefined)
                keySelector = identity;

            if (elementSelector === _undefined)
                elementSelector = identity;

            if (keySerializer === _undefined)
                keySerializer = function (x) { return x.toString(); }

            var parent = this;

            return observableCreateWithDisposable(function (observer)
            {
                var map = {};
                var subscription = new mutableDisposable();
                var refCount = new refCountDisposable(subscription);
                subscription.Replace(parent.Subscribe(function (x)
                {
                    var key;
                    try
                    {
                        key = keySelector(x);
                    }
                    catch (exception)
                    {
                        for (var k in map)
                        {
                            map[k].OnError(exception);
                        }
                        observer.OnError(exception);
                        return;
                    }
                    var fireNewMapEntry = false;
                    var writer;
                    try
                    {
                        var serialized = keySerializer(key);
                        if (map[serialized] === _undefined)
                        {
                            writer = new subject();
                            map[serialized] = writer;
                            fireNewMapEntry = true;
                        }
                        else
                        {
                            writer = map[serialized];
                        }
                    }
                    catch (exception)
                    {
                        for (var k in map)
                        {
                            map[k].OnError(exception);
                        }
                        observer.OnError(exception);
                        return;
                    }
                    if (fireNewMapEntry)
                    {
                        var inner = observableCreateWithDisposable(function (innerObserver)
                        {
                            return new compositeDisposable(refCount.GetDisposable(), writer.Subscribe(innerObserver));
                        });
                        inner.Key = key;
                        observer.OnNext(inner);
                    }
                    var element;
                    try
                    {
                        element = elementSelector(x);
                    }
                    catch (exception)
                    {
                        for (var k in map)
                        {
                            map[k].OnError(exception);
                        }
                        observer.OnError(exception);
                        return;
                    }
                    writer.OnNext(element);
                },
            function (exception)
            {
                for (var k in map)
                {
                    map[k].OnError(exception);
                }
                observer.OnError(exception);
            },
            function ()
            {
                for (var k in map)
                {
                    map[k].OnCompleted();
                }
                observer.OnCompleted();
            }));
                return refCount;
            });
        },
        TakeWhile: function (predicate)
        {
            var parent = this;
            return observableCreateWithDisposable(function (subscriber)
            {
                var running = true;
                return parent.Subscribe(new observer(
            function (value)
            {
                if (running)
                {
                    try
                    {
                        running = predicate(value);
                    }
                    catch (e)
                    {
                        subscriber.OnError(e);
                        return;
                    }
                    if (running)
                    {
                        subscriber.OnNext(value);
                    }
                    else
                    {
                        subscriber.OnCompleted();
                    }
                }
            },
            function (e) { subscriber.OnError(e); }, function () { subscriber.OnCompleted(); }));
            });
        },
        SkipWhile: function (predicate)
        {
            var parent = this;
            return observableCreateWithDisposable(function (subscriber)
            {
                var running = false;
                return parent.Subscribe(new observer(
            function (value)
            {
                if (!running)
                {
                    try
                    {
                        running = !predicate(value);
                    }
                    catch (e)
                    {
                        subscriber.OnError(e);
                        return;
                    }
                }
                if (running)
                {
                    subscriber.OnNext(value);
                }
            },
            function (e) { subscriber.OnError(e); }, function () { subscriber.OnCompleted(); }));
            });
        },
        Skip: function (count)
        {
            var parent = this;
            return observableCreateWithDisposable(function (subscriber)
            {
                var remaining = count;

                return parent.Subscribe(new observer(
            function (value)
            {
                if (remaining-- <= 0)
                {
                    subscriber.OnNext(value);
                }
            },
            function (e) { subscriber.OnError(e); }, function () { subscriber.OnCompleted(); }));
            });
        },
        SelectMany: function (selector)
        {
            return this.Select(selector).MergeObservable();
        },
        TimeInterval: function (scheduler)
        {
            if (scheduler === _undefined)
                scheduler = immediateScheduler;

            var parent = this;
            return observableDefer(function ()
            {
                var last = scheduler.Now();
                return parent.Select(function (value)
                {
                    var now = scheduler.Now();
                    var span = now - last;
                    last = now;
                    return { Interval: span, Value: value };
                });
            });
        },
        RemoveInterval: removeValue,
        Timestamp: function (scheduler)
        {
            if (scheduler === _undefined)
                scheduler = immediateScheduler;

            return this.Select(function (value)
            {
                return { Timestamp: scheduler.Now(), Value: value };
            });
        },
        RemoveTimestamp: removeValue,
        Materialize: function ()
        {
            var parent = this;
            return observableCreateWithDisposable(function (subscriber)
            {
                return parent.Subscribe(new observer(
            function (value)
            {
                subscriber.OnNext(new notification("N", value));
            },
            function (error)
            {
                subscriber.OnNext(new notification("E", error));
                subscriber.OnCompleted();
            },
            function ()
            {
                subscriber.OnNext(new notification("C"));
                subscriber.OnCompleted();
            }));
            });
        },
        Dematerialize: function ()
        {
            return this.SelectMany(function (notification) { return notification; });
        },
        AsObservable: function ()
        {
            var parent = this;
            return observableCreateWithDisposable(function (observer)
            {
                return parent.Subscribe(observer);
            });
        },
        Delay: function (dueTime, scheduler)
        {
            if (scheduler === _undefined)
                scheduler = timeoutScheduler;

            var parent = this;

            return observableCreateWithDisposable(function (observer)
            {
                var q = [];
                var active = false;
                var cancelable = new mutableDisposable();

                var subscription = parent.Materialize().Timestamp().Subscribe(function (notification)
                {
                    if (notification.Value.Kind == "E")
                    {
                        observer.OnError(notification.Value.Value);
                        q = [];
                        if (active)
                            cancelable.Dispose();
                        return;
                    }
                    q.push({ Timestamp: scheduler.Now() + dueTime, Value: notification.Value });
                    if (!active)
                    {
                        cancelable.Replace(scheduler.ScheduleRecursiveWithTime(
                function (start)
                {
                    var result;
                    do
                    {
                        result = _undefined;
                        if (q.length > 0 && q[0].Timestamp <= scheduler.Now())
                            result = q.shift().Value
                        if (result !== _undefined)
                        {
                            result.Accept(observer);
                        }
                    }
                    while (result !== _undefined);
                    if (q.length > 0)
                    {
                        start(Math.max(0, q[0].Timestamp - scheduler.Now()));
                        active = true;
                    }
                    else
                    {
                        active = false;
                    }
                }, dueTime));
                        active = true;
                    }
                });
                return new compositeDisposable(subscription, cancelable);
            });
        },
        Throttle: function (dueTime, scheduler)
        {
            if (scheduler === _undefined)
            {
                scheduler = timeoutScheduler;
            }
            var parent = this;
            return observableCreateWithDisposable(function (observer)
            {
                var value;
                var hasValue = false;
                var cancelable = new mutableDisposable();
                var id = 0;

                var subscription = parent.Subscribe(function (x)
                {
                    hasValue = true;
                    value = x;
                    id++;
                    var currentId = id;
                    cancelable.Replace(scheduler.ScheduleWithTime(function ()
                    {
                        if (hasValue && id == currentId)
                        {
                            observer.OnNext(value);
                        }
                        hasValue = false;
                    }, dueTime));
                },
            function (exception)
            {
                cancelable.Dispose();
                observer.OnError(exception);
                hasValue = false;
                id++;
            },
            function ()
            {
                cancelable.Dispose();
                if (hasValue)
                    observer.OnNext(value);
                observer.OnCompleted();
                hasValue = false;
                id++;
            });

                return new compositeDisposable(subscription, cancelable);
            });
        },
        Timeout: function (dueTime, other, scheduler)
        {
            if (scheduler === _undefined)
            {
                scheduler = timeoutScheduler;
            }
            if (other === _undefined)
            {
                other = observableThrow("Timeout", scheduler);
            }
            var parent = this;

            return observableCreateWithDisposable(function (subscriber)
            {
                var subscription = new mutableDisposable();
                var timer = new mutableDisposable();
                var id = 0;
                var initial = id;
                var switched = false;

                timer.Replace(scheduler.ScheduleWithTime(function ()
                {
                    switched = id == initial;
                    if (switched)
                        subscription.Replace(other.Subscribe(subscriber));
                }, dueTime));

                subscription.Replace(parent.Subscribe(
                    function (x)
                    {
                        var value = 0;
                        if (!switched)
                        {
                            id++;
                            value = id;
                            subscriber.OnNext(x);
                            timer.Replace(scheduler.ScheduleWithTime(
                                function ()
                                {
                                    switched = id == value;
                                    if (switched)
                                    {
                                        subscription.Replace(other.Subscribe(subscriber));
                                    }
                                }, dueTime));
                        }
                    },
                    function (exception)
                    {
                        if (!switched)
                        {
                            id++;
                            subscriber.OnError(exception);
                        }
                    },
                    function ()
                    {
                        if (!switched)
                        {
                            id++;
                            subscriber.OnCompleted();
                        }
                    }));

                return new compositeDisposable(subscription, timer);
            });
        },
        Sample: function (interval, scheduler)
        {
            if (scheduler === _undefined)
            {
                scheduler = timeoutScheduler;
            }
            var parent = this;
            return observableCreateWithDisposable(function (observer)
            {
                var hasCurrentValue = false;
                var currentValue;
                var isAtEnd = false;
                var group = new compositeDisposable();
                group.Add(observableInterval(interval, scheduler).Subscribe(
                function (value)
                {
                    if (hasCurrentValue)
                    {
                        observer.OnNext(currentValue);
                        hasCurrentValue = false;
                    }
                    if (isAtEnd)
                    {
                        observer.OnCompleted();
                    }
                },
                function (exception)
                {
                    observer.OnError(exception);
                },
                function ()
                {
                    observer.OnCompleted();
                }));
                group.Add(parent.Subscribe(function (value)
                {
                    hasCurrentValue = true;
                    currentValue = value;
                },
                function (exception)
                {
                    observer.OnError(exception);
                    group.Dispose();
                },
                function ()
                {
                    isAtEnd = true;
                }));
                return group;
            });
        },
        Repeat: function (repeatCount, scheduler)
        {
            var parent = this;

            if (scheduler === _undefined) scheduler = immediateScheduler;

            if (repeatCount === _undefined)
                repeatCount = -1;

            return observableCreateWithDisposable(function (observer)
            {
                var left = repeatCount;
                var mutable = new mutableDisposable();
                var group = new compositeDisposable(mutable);
                var attach = function (self)
                {
                    mutable.Replace(parent.Subscribe(function (value)
                    {
                        observer.OnNext(value);
                    },
                    function (exception)
                    {
                        observer.OnError(exception);
                    },
                    function ()
                    {
                        if (left > 0)
                        {
                            left--;
                            if (left == 0)
                            {
                                observer.OnCompleted();
                                return;
                            }
                        }
                        self();
                    }));
                };
                group.Add(scheduler.ScheduleRecursive(attach));
                return group;
            });
        },
        Retry: function (retryCount, scheduler)
        {
            var parent = this;

            if (scheduler === _undefined) scheduler = immediateScheduler;

            if (retryCount === _undefined)
                retryCount = -1;

            return observableCreateWithDisposable(function (observer)
            {
                var left = retryCount;
                var mutable = new mutableDisposable();
                var group = new compositeDisposable(mutable);
                var attach = function (self)
                {
                    mutable.Replace(parent.Subscribe(function (value)
                    {
                        observer.OnNext(value);
                    },
                function (exception)
                {
                    if (left > 0)
                    {
                        left--;
                        if (left == 0)
                        {
                            observer.OnError(exception);
                            return;
                        }
                    }
                    self();
                },
                function ()
                {
                    observer.OnCompleted();
                }));
                };
                group.Add(scheduler.ScheduleRecursive(attach));
                return group;
            });
        },
        BufferWithTime: function (timeSpan, timeShift, scheduler)
        {
            if (scheduler === _undefined)
                scheduler = timeoutScheduler;

            if (timeShift === _undefined)
                timeShift = timeSpan;


            var parent = this;

            return observableCreateWithDisposable(function (observer)
            {

                var items = new list();
                var currentWindowStart = scheduler.Now();

                var getCurrentWindow = function ()
                {
                    var window = [];
                    for (var i = 0; i < items.GetCount(); i++)
                    {
                        var item = items.GetItem(i);
                        if (item.Timestamp - currentWindowStart >= 0)
                        {
                            window.push(item.Value);
                        }
                    }
                    return window;
                };

                var group = new compositeDisposable();
                var onError = function (exception)
                {
                    observer.OnError(exception);
                };
                var onCompleted = function ()
                {
                    observer.OnNext(getCurrentWindow());
                    observer.OnCompleted();
                }
                group.Add(parent.Subscribe(
                function (value)
                {
                    items.Add({ Value: value, Timestamp: scheduler.Now() });
                },
                onError,
                onCompleted));

                group.Add(observableTimer(timeSpan, timeShift, scheduler).Subscribe(
                function (value)
                {
                    var result = getCurrentWindow();
                    var nextWindowStart = scheduler.Now() + timeShift - timeSpan;

                    while (items.GetCount() > 0 && items.GetItem(0).Timestamp - nextWindowStart <= 0)
                    {
                        items.RemoveAt(0);
                    }
                    observer.OnNext(result);
                    currentWindowStart = nextWindowStart;
                },
                onError,
                onCompleted));

                return group;
            });
        },
        BufferWithTimeOrCount: function (timeSpan, count, scheduler)
        {
            if (scheduler === _undefined)
                scheduler = timeoutScheduler;

            var parent = this;

            return observableCreateWithDisposable(function (subscriber)
            {
                var bufferId = 0;
                var data = new list();

                var flushBuffer = function ()
                {
                    subscriber.OnNext(data.ToArray());
                    data.Clear();
                    bufferId++;
                };

                var timer = new mutableDisposable();
                var startTimer;
                startTimer = function (myId)
                {
                    var workItem = scheduler.ScheduleWithTime(function ()
                    {
                        var shouldRecurse = false;
                        var nextId = 0;
                        if (myId == bufferId)
                        {
                            flushBuffer();
                            nextId = bufferId;
                            shouldRecurse = true;
                        }
                        if (shouldRecurse)
                        {
                            startTimer(nextId);
                        }
                    }, timeSpan);
                    timer.Replace(workItem);
                };
                startTimer(bufferId);

                var subscription = parent.Subscribe(
                function (value)
                {
                    var shouldStartNewTimer = false;
                    var nextId = 0;
                    data.Add(value);
                    if (data.GetCount() == count)
                    {
                        flushBuffer();
                        nextId = bufferId;
                        shouldStartNewTimer = true;
                    }
                    if (shouldStartNewTimer)
                    {
                        startTimer(nextId);
                    }
                },
                function (exception)
                {
                    subscriber.OnError(exception);
                    data.Clear();
                },
                function ()
                {
                    subscriber.OnNext(data.ToArray());
                    bufferId++;
                    subscriber.OnCompleted();
                    data.Clear();
                });
                return new compositeDisposable(subscription, timer);
            });
        },
        BufferWithCount: function (count, skip)
        {
            if (skip === _undefined)
                skip = count;

            var parent = this;

            return observableCreateWithDisposable(function (observer)
            {
                var list = [];
                var n = 0;
                return parent.Subscribe(
                function (value)
                {
                    if (n == 0)
                        list.push(value);
                    else
                        n--;

                    var currentCount = list.length;
                    if (currentCount == count)
                    {
                        var result = list;
                        list = [];
                        var skipHead = Math.min(skip, currentCount);
                        for (var i = skipHead; i < currentCount; i++)
                        {
                            list.push(result[i]);
                        }
                        n = Math.max(0, skip - count);
                        observer.OnNext(result);
                    }
                },
                function (exception)
                {
                    observer.OnError(exception);
                },
                function ()
                {
                    if (list.length > 0)
                        observer.OnNext(list);

                    observer.OnCompleted();
                });
            });
        },
        StartWith: function (values, scheduler)
        {
            if (!(values instanceof Array))
            {
                values = [values];
            }

            if (scheduler === _undefined)
                scheduler = immediateScheduler;

            var parent = this;
            return observableCreateWithDisposable(function (observer)
            {
                var group = new compositeDisposable();
                var pos = 0;
                group.Add(scheduler.ScheduleRecursive(function (self)
                {
                    if (pos < values.length)
                    {
                        observer.OnNext(values[pos]);
                        pos++;
                        self();
                    }
                    else
                    {
                        group.Add(parent.Subscribe(observer));
                    }
                }));
                return group;
            });
        },
        DistinctUntilChanged: function (keySelector, comparer)
        {
            if (keySelector === _undefined)
                keySelector = identity;
            if (comparer === _undefined)
                comparer = defaultComparer;


            var parent = this;

            return observableCreateWithDisposable(function (observer)
            {
                var currentKey;
                var hasCurrentKey = false;
                return parent.Subscribe(
                function (value)
                {
                    var key;
                    try
                    {
                        key = keySelector(value);
                    }
                    catch (e)
                    {
                        observer.OnError(e);
                        return;
                    }
                    var comparerEquals = false;
                    if (hasCurrentKey)
                    {
                        try
                        {
                            comparerEquals = comparer(currentKey, key);
                        }
                        catch (e)
                        {
                            observer.OnError(e);
                            return;
                        }
                    }
                    if (!hasCurrentKey || !comparerEquals)
                    {
                        hasCurrentKey = true;
                        currentKey = key;
                        observer.OnNext(value);
                    }
                },
                function (exception)
                {
                    observer.OnError(exception);
                },
                function ()
                {
                    observer.OnCompleted();
                })
            });
        },
        Publish: function (selector)
        {
            if (selector === _undefined)
                return new connectableObservable(this, new subject());

            var parent = this;
            return observableCreateWithDisposable(function (subscriber)
            {
                var connectable = new connectableObservable(parent, new subject());
                return new compositeDisposable(selector(connectable).Subscribe(observer),
                connectable.Connect());
            });
        },
        Prune: function (selector, scheduler)
        {
            if (scheduler === _undefined)
                scheduler = immediateScheduler;

            if (selector === _undefined)
                return new connectableObservable(this, new asyncSubject(scheduler));

            var parent = this;
            return observableCreateWithDisposable(function (subscriber)
            {
                var connectable = new connectableObservable(parent, new asyncSubject(scheduler));
                return new compositeDisposable(selector(connectable).Subscribe(observer),
                connectable.Connect());
            });
        },
        Replay: function (selector, bufferSize, window, scheduler)
        {
            if (scheduler === _undefined)
                scheduler = currentThreadScheduler;

            if (selector === _undefined)
                return new connectableObservable(this, new replaySubject(bufferSize, window, scheduler));

            var parent = this;
            return observableCreateWithDisposable(function (subscriber)
            {
                var connectable = new connectableObservable(parent, new replaySubject(bufferSize, window, scheduler));
                return new compositeDisposable(selector(connectable).Subscribe(observer),
                connectable.Connect());
            });
        },
        SkipLast: function (count)
        {
            var parent = this;
            return observableCreateWithDisposable(function (subscriber)
            {
                var q = [];
                return parent.Subscribe(function (value)
                {
                    q.push(value);
                    if (q.length > count)
                    {
                        subscriber.OnNext(q.shift());
                    }
                },
                function (e)
                {
                    subscriber.OnError(e);
                },
                function ()
                {
                    subscriber.OnCompleted();
                });
            });
        },
        TakeLast: function (count)
        {
            var parent = this;
            return observableCreateWithDisposable(function (subscriber)
            {
                var q = [];
                return parent.Subscribe(function (value)
                {
                    q.push(value);
                    if (q.length > count)
                    {
                        q.shift();
                    }
                },
                function (e)
                {
                    subscriber.OnError(e);
                },
                function ()
                {
                    while (q.length > 0)
                        subscriber.OnNext(q.shift());
                    subscriber.OnCompleted();
                });
            });
        }
    };

    var observableMerge = observable.Merge = function (items, scheduler)
    {
        if (scheduler === _undefined)
            scheduler = immediateScheduler;

        return observableFromArray(items, scheduler).MergeObservable();
    }

    var observableConcat = observable.Concat = function (items, scheduler)
    {
        if (scheduler === _undefined)
            scheduler = immediateScheduler;

        return observableCreateWithDisposable(function (observer)
        {
            var subscription = new mutableDisposable();
            var pos = 0;
            var cancelable = scheduler.ScheduleRecursive(function (self)
            {
                if (pos < items.length)
                {
                    var current = items[pos];
                    pos++;
                    var d = new mutableDisposable();
                    subscription.Replace(d);
                    d.Replace(current.Subscribe(
                        function (value)
                        {
                            observer.OnNext(value);
                        },
                        function (exception)
                        {
                            observer.OnError(exception);
                        },
                        self));
                }
                else
                {
                    observer.OnCompleted();
                }
            });

            return new compositeDisposable(subscription, cancelable);
        });
    };

    var observableFromArray = observable.FromArray = function (values, scheduler)
    {
        if (scheduler === _undefined) scheduler = immediateScheduler;

        return observableCreateWithDisposable(function (subscriber)
        {
            var count = 0;
            return scheduler.ScheduleRecursive(function (self)
            {
                if (count < values.length)
                {
                    subscriber.OnNext(values[count++]);
                    self();
                }
                else
                {
                    subscriber.OnCompleted();
                }
            });
        });
    }

    var observableReturn = observable.Return = function (value, scheduler)
    {
        if (scheduler === _undefined) scheduler = immediateScheduler;

        return observableCreateWithDisposable(function (observer)
        {
            return scheduler.Schedule(function ()
            {
                observer.OnNext(value);
                observer.OnCompleted();
            });
        });
    };

    var observableThrow = observable.Throw = function (error, scheduler)
    {
        if (scheduler === _undefined) scheduler = immediateScheduler;
        return observableCreateWithDisposable(function (observer)
        {
            return scheduler.Schedule(function ()
            {
                observer.OnError(error);
            });
        });
    };

    var observableNever = observable.Never = function ()
    {
        return observableCreateWithDisposable(function (observer)
        {
            return disposableEmpty;
        });
    };

    var observableEmpty = observable.Empty = function (scheduler)
    {
        if (scheduler === _undefined) scheduler = immediateScheduler;
        return observableCreateWithDisposable(function (observer)
        {
            return scheduler.Schedule(function ()
            {
                observer.OnCompleted();
            });
        });
    };

    var observableDefer = observable.Defer = function (observableFactory)
    {
        return observableCreateWithDisposable(function (observer)
        {
            var result;
            try
            {
                result = observableFactory();
            }
            catch (e)
            {
                observer.OnError(e);
                return disposableEmpty;
            }
            return result.Subscribe(observer);
        });
    }


    var observableCatch = observable.Catch = function (items, scheduler)
    {
        if (scheduler === _undefined)
            scheduler = immediateScheduler;

        return observableCreateWithDisposable(function (observer)
        {
            var subscription = new mutableDisposable();
            var pos = 0;
            var cancelable = scheduler.ScheduleRecursive(function (self)
            {
                var current = items[pos];
                pos++;
                var d = new mutableDisposable();
                subscription.Replace(d);
                d.Replace(current.Subscribe(
                function (value)
                {
                    observer.OnNext(value);
                },
                function (e)
                {
                    if (pos < items.length)
                        self();
                    else
                        observer.OnError(e);
                },
                function ()
                {
                    observer.OnCompleted();
                }));
            });

            return new compositeDisposable(subscription, cancelable);
        });
    };

    var observableUsing = observable.Using = function (resourceSelector, resourceUsage)
    {
        return observableCreateWithDisposable(function (observer)
        {
            var source;
            var disposable = disposableEmpty;
            try
            {
                var resource = resourceSelector();
                if (resource !== _undefined)
                    disposable = resource;
                source = resourceUsage(resource);
            }
            catch (e)
            {
                return new compositeDisposable(Throw(e).Subscribe(observer), disposable);
            }
            return new compositeDisposable(source.Subscribe(observer), disposable);
        });
    };

    var observableRange = observable.Range = function (start, count, scheduler)
    {
        if (scheduler === _undefined)
            scheduler = immediateScheduler;

        var max = start + count - 1
        return observableGenerate(start
        , function (x) { return x <= max; }
        , function (x) { return x + 1; }
        , identity, scheduler);
    }

    var observableRepeat = observable.Repeat = function (value, repeatCount, scheduler)
    {
        if (scheduler === _undefined)
            scheduler = immediateScheduler;

        if (repeatCount === _undefined)
            repeatCount = -1;

        var left = repeatCount;

        return observableCreateWithDisposable(function (observer)
        {
            return scheduler.ScheduleRecursive(function (self)
            {
                observer.OnNext(value);
                if (left > 0)
                {
                    left--;
                    if (left == 0)
                    {
                        observer.OnCompleted();
                        return;
                    }
                }
                self();
            });
        });
    };

    var observableGenerate = observable.Generate = function (initialState, condition, iterate, resultSelector, scheduler)
    {
        if (scheduler === _undefined)
            scheduler = immediateScheduler;

        return observableCreateWithDisposable(function (observer)
        {
            var state = initialState;
            var first = true;

            return scheduler.ScheduleRecursive(function (self)
            {
                var hasResult = false;
                var result;
                try
                {
                    if (first)
                        first = false;
                    else
                        state = iterate(state);
                    hasResult = condition(state);
                    if (hasResult)
                        result = resultSelector(state);
                }
                catch (e)
                {
                    observer.OnError(e);
                    return;
                }
                if (hasResult)
                {
                    observer.OnNext(result);
                    self();
                }
                else
                    observer.OnCompleted();
            });
        });

    };

    var observableGenerateWithTime = observable.GenerateWithTime = function (initialState, condition, iterate, resultSelector, timeSelector, scheduler)
    {
        if (scheduler === _undefined)
            scheduler = timeoutScheduler;

        return new observableCreateWithDisposable(function (subscriber)
        {
            var state = initialState;
            var first = true;
            var hasResult = false;
            var result;
            var time;
            return scheduler.ScheduleRecursiveWithTime(function (self)
            {
                if (hasResult)
                    subscriber.OnNext(result);
                try
                {
                    if (first)
                        first = false;
                    else
                        state = iterate(state);
                    hasResult = condition(state);
                    if (hasResult)
                    {
                        result = resultSelector(state);
                        time = timeSelector(state);
                    }
                }
                catch (exception)
                {
                    subscriber.OnError(exception);
                    return;
                }

                if (hasResult)
                    self(time);
                else
                    subscriber.OnCompleted();
            }, 0);
        });
    };

    var observableOnErrorResumeNext = observable.OnErrorResumeNext = function (items, scheduler)
    {
        if (scheduler === _undefined)
            scheduler = immediateScheduler;

        return observableCreateWithDisposable(function (observer)
        {
            var subscription = new mutableDisposable();
            var pos = 0;
            var cancelable = scheduler.ScheduleRecursive(function (self)
            {
                if (pos < items.length)
                {
                    var current = items[pos];
                    pos++;
                    var d = new mutableDisposable();
                    subscription.Replace(d);
                    d.Replace(current.Subscribe(
                function (value)
                {
                    observer.OnNext(value);
                },
                self,
                self));
                }
                else
                {
                    observer.OnCompleted();
                }
            });

            return new compositeDisposable(subscription, cancelable);
        });
    };

    var observableAmb = observable.Amb = function ()
    {
        var items = arguments;

        return observableCreateWithDisposable(function (subscriber)
        {
            var group = new compositeDisposable();
            var outerDisposable = new mutableDisposable();
            outerDisposable.Replace(group);
            var winnerSelected = false;
            for (var i = 0; i < items.length; i++)
            {
                var item = items[i];
                var innerSubscription = new mutableDisposable();
                var innerObserver = new observer(function (v)
                {
                    if (!winnerSelected)
                    {
                        group.Remove(this._innerSubscription, true);
                        group.Dispose();
                        outerDisposable.Replace(this._innerSubscription);
                        winnerSelected = true;
                    }
                    subscriber.OnNext(v);
                },
                function (e)
                {
                    subscriber.OnError(e);
                    outerDisposable.Dispose();
                },
                function ()
                {
                    subscriber.OnCompleted();
                    outerDisposable.Dispose();
                });
                innerObserver._innerSubscription = innerSubscription;
                innerSubscription.Replace(item.Subscribe(innerObserver));
                group.Add(innerSubscription);
            }
            return outerDisposable;
        });
    };

    var observableForkJoin = observable.ForkJoin = function ()
    {
        var items = arguments;
        return observableCreateWithDisposable(function (subscriber)
        {
            var hasResults = [];
            var hasCompleted = [];
            var results = [];
            var group = new compositeDisposable();
            for (var i = 0; i < items.length; i++)
            {
                (function (i)
                {
                    group.Add(items[i].Subscribe(function (value)
                    {
                        hasResults[i] = true;
                        results[i] = value;
                    },
                    function (error)
                    {
                        subscriber.OnError(error);
                    },
                    function (onCompleted)
                    {
                        if (!hasResults[i])
                        {
                            subscriber.OnCompleted();
                            results = _undefined;
                            hasResults = _undefined;
                            return;
                        }
                        hasCompleted[i] = true;
                        var done = true;
                        for (var j = 0; j < items.length; j++)
                        {
                            if (!hasCompleted[j])
                                done = false;
                        }
                        if (done)
                        {
                            subscriber.OnNext(results);
                            subscriber.OnCompleted();
                            results = _undefined;
                            hasCompleted = _undefined;
                            hasResults = _undefined;
                        }
                    }));
                })(i);
            }
            return group;
        });
    }

    var observableInterval = observable.Interval = function (period, scheduler)
    {
        return observableTimer(period, period, scheduler);
    };

    var _normalizeTimespan = function (timespan)
    {
        return Math.max(0, timespan);
    };

    var observableTimer = observable.Timer = function (dueTime, period, scheduler)
    {
        if (scheduler === _undefined)
            scheduler = timeoutScheduler;

        if (dueTime === _undefined)
            return observableNever();

        if (dueTime instanceof Date)
        {
            return observableDefer(function ()
            {
                return observable.Timer(dueTime - new Date(), period, scheduler);
            });
        }
        var d = _normalizeTimespan(dueTime);

        if (period === _undefined)
        {

            return observableCreateWithDisposable(function (observer)
            {
                return scheduler.ScheduleWithTime(function ()
                {
                    observer.OnNext(0);
                    observer.OnCompleted();
                }, d);
            });
        }

        var p = _normalizeTimespan(period);

        return observableCreateWithDisposable(function (observer)
        {
            var count = 0;
            return scheduler.ScheduleRecursiveWithTime(function (self)
            {
                observer.OnNext(count++);
                self(p);
            }, d);
        });

    };

    var observableWhile = observable.While = function (condition, source)
    {
        return observableCreateWithDisposable(function (subscriber)
        {
            var subscription = new mutableDisposable();
            var group = new compositeDisposable(subscription);
            group.Add(immediateScheduler.ScheduleRecursive(function (self)
            {
                var startNext;
                try
                {
                    startNext = condition();
                }
                catch (e)
                {
                    subscriber.OnError(e);
                    return;
                }
                if (startNext)
                {
                    subscription.Replace(source.Subscribe(
                        function (value)
                        {
                            subscriber.OnNext(value);
                        },
                        function (exception)
                        {
                            subscriber.OnError(exception);
                        },
                        function ()
                        {
                            self();
                        }
                    ));
                }
                else
                {
                    subscriber.OnCompleted();
                }
            }));
            return group;
        });
    }

    var observableIf = observable.If = function (condition, thenSource, elseSource)
    {
        if (elseSource === _undefined)
            elseSource = observableEmpty();

        return observableDefer(function () { return condition() ? thenSource : elseSource; });
    }

    var observableDoWhile = observable.DoWhile = function (source, condition)
    {
        return observableConcat([source, observableWhile(condition, source)]);
    }

    var observableCase = observable.Case = function (selector, sources, defaultSource, scheduler)
    {
        if (scheduler === _undefined)
            scheduler = immediateScheduler;
        if (defaultSource === _undefined)
            defaultSource = observableEmpty(scheduler);

        return observableDefer(function ()
        {
            var result = sources[selector()]
            if (result === _undefined)
            {
                result = defaultSource;
            }
            return result;
        });
    }

    var observableFor = observable.For = function (source, resultSelector)
    {
        return observableCreateWithDisposable(function (subscriber)
        {
            var group = new compositeDisposable();
            var count = 0;
            group.Add(immediateScheduler.ScheduleRecursive(function (self)
            {
                if (count < source.length)
                {
                    var result;
                    try
                    {
                        result = resultSelector(source[count]);
                    }
                    catch (e)
                    {
                        subscriber.OnError(e);
                        return;
                    }
                    group.Add(result.Subscribe(
                        function (value)
                        {
                            subscriber.OnNext(value);
                        },
                        function (exception)
                        {
                            subscriber.OnError(exception);
                        },
                        function ()
                        {
                            count++;
                            self();
                        }
                    ));
                }
                else
                {
                    subscriber.OnCompleted();
                }
            }));
            return group;
        });
    }

    var observableLet = observable.Let = function (value, selector)
    {
        return observableDefer(function () { return selector(value); });
    }

    var notification = root.Notification = function (kind, value)
    {
        this.Kind = kind;
        this.Value = value;
        this.toString = function ()
        {
            return this.Kind + ": " + this.Value;
        };

        this.Accept = function (observer)
        {
            switch (this.Kind)
            {
                case "N":
                    observer.OnNext(this.Value);
                    break;
                case "E":
                    observer.OnError(this.Value);
                    break;
                case "C":
                    observer.OnCompleted();
                    break;
            }
            return disposableEmpty;
        };

        this._subscribe = function (observer)
        {
            var subscription = this.Accept(observer);
            if (kind == "N")
                observer.OnCompleted();
            return subscription;
        };
    };

    notification.prototype = new observable;

    var subject = root.Subject = function ()
    {
        var observers = new list();
        var isStopped = false;

        this.OnNext = function (value)
        {
            if (!isStopped)
            {
                var currentObservers = observers.ToArray();
                for (var i = 0; i < currentObservers.length; i++)
                {
                    var observer = currentObservers[i];
                    observer.OnNext(value);
                }
            }
        };
        this.OnError = function (exception)
        {
            if (!isStopped)
            {
                var currentObservers = observers.ToArray();
                for (var i = 0; i < currentObservers.length; i++)
                {
                    var observer = currentObservers[i];
                    observer.OnError(exception);
                }
                isStopped = true;
                observers.Clear();
            }
        };
        this.OnCompleted = function ()
        {
            if (!isStopped)
            {
                var currentObservers = observers.ToArray();
                for (var i = 0; i < currentObservers.length; i++)
                {
                    var observer = currentObservers[i];
                    observer.OnCompleted();
                }
                isStopped = true;
                observers.Clear();
            }
        };
        this._subscribe = function (observer)
        {
            if (!isStopped)
            {
                observers.Add(observer);
                return disposableCreate(function () { observers.Remove(observer) });
            }
            else
            {
                return disposableEmpty;
            }
        };
    }

    subject.prototype = new observable;

    for (var k in observer.prototype)
    {
        subject.prototype[k] = observer.prototype[k];
    }

    var asyncSubject = root.AsyncSubject = function (scheduler)
    {
        var observers = new list();
        var last;
        var completed = false;
        if (scheduler === _undefined)
        {
            scheduler = immediateScheduler;
        }

        this.OnNext = function (value)
        {
            if (!completed)
            {
                last = new notification("N", value);
            }
        };
        this.OnError = function (exception)
        {
            if (!completed)
            {
                last = new notification("E", exception);
                var currentObservers = observers.ToArray();
                for (var i = 0; i < currentObservers.length; i++)
                {
                    var observer = currentObservers[i];
                    if (observer !== _undefined)
                        observer.OnError(exception);
                }
                completed = true;
                observers.Clear();
            }
        };
        this.OnCompleted = function ()
        {
            if (!completed)
            {
                if (last === _undefined)
                    last = new notification("C");

                var currentObservers = observers.ToArray();
                for (var i = 0; i < currentObservers.length; i++)
                {
                    var observer = currentObservers[i];
                    if (observer !== _undefined)
                    {
                        last._subscribe(observer);
                    }
                }
                completed = true;
                observers.Clear();
            }
        };
        this._subscribe = function (observer)
        {
            if (!completed)
            {
                observers.Add(observer);
                return disposableCreate(function () { observers.Remove(observer); });
            }
            else
            {
                return scheduler.Schedule(function ()
                {
                    last._subscribe(observer);
                });
            }
        };
    }

    asyncSubject.prototype = new subject;

    var behaviorSubject = root.BehaviorSubject = function (value, scheduler)
    {
        var underlyingSubject = new replaySubject(1, -1, scheduler);
        underlyingSubject.OnNext(value);
        return underlyingSubject;
    };

    var replaySubject = root.ReplaySubject = function (bufferSize, window, scheduler)
    {
        var observers = new list();
        var q = new list();
        var isStopped = false;

        if (scheduler === _undefined)
            scheduler = currentThreadScheduler;
        var hasWindow = window > 0;

        var addToQueue = function (kind, value)
        {
            q.Add({ Value: new notification(kind, value), Timestamp: scheduler.Now() });
        };
        this._Trim = function ()
        {
            if (bufferSize !== _undefined)
            {
                while (q.GetCount() > bufferSize)
                {
                    q.RemoveAt(0);
                }
            }
            if (hasWindow)
            {
                while (q.GetCount() > 0 && scheduler.Now() - q.GetItem(0).Timestamp > window)
                {
                    q.RemoveAt(0);
                }
            }
        };

        this.OnNext = function (value)
        {
            if (!isStopped)
            {
                var currentObservers = observers.ToArray();
                for (var i = 0; i < currentObservers.length; i++)
                {
                    var observer = currentObservers[i];
                    observer.OnNext(value);
                }
                addToQueue("N", value);
            }
        };
        this.OnError = function (exception)
        {
            if (!isStopped)
            {
                var currentObservers = observers.ToArray();
                for (var i = 0; i < currentObservers.length; i++)
                {
                    var observer = currentObservers[i];
                    observer.OnError(exception);
                }
                isStopped = true;
                observers.Clear();
                addToQueue("E", exception);
            }
        };
        this.OnCompleted = function ()
        {
            if (!isStopped)
            {
                var currentObservers = observers.ToArray();
                for (var i = 0; i < currentObservers.length; i++)
                {
                    var observer = currentObservers[i];
                    observer.OnCompleted();
                }
                isStopped = true;
                observers.Clear();
                addToQueue("C");
            }
        };
        this._subscribe = function (observer)
        {
            var subscription = new removableDisposable(this, observer);
            var group = new compositeDisposable(subscription);
            var parent = this;
            group.Add(scheduler.Schedule(function ()
            {
                if (!subscription._isStopped)
                {
                    parent._Trim();
                    for (var i = 0; i < q.GetCount(); i++)
                    {
                        q.GetItem(i).Value.Accept(observer);
                    }
                    observers.Add(observer);
                    subscription._isStarted = true;
                }
            }));
            return group;
        }
        this._Unsubscribe = function (observer)
        {
            observers.Remove(observer);
        }
    };

    replaySubject.prototype = new subject;

    var removableDisposable = function (subject, observer)
    {
        this._subject = subject;
        this._observer = observer;
        this._isStarted = false;
        this._isStopped = false;

        this.Dispose = function ()
        {
            if (this._isStarted)
            {
                this._subject._Unsubscribe(this._observer);
            }
            this._isStopped = true;
        };
    };

    var observableToAsync = observable.ToAsync = function (original, scheduler)
    {
        if (scheduler === _undefined)
            scheduler = timeoutScheduler;

        return function ()
        {
            var subject = new asyncSubject(scheduler);
            var delayed = function ()
            {
                var result;
                try
                {
                    result = original.apply(this, arguments);
                }
                catch (e)
                {
                    subject.OnError(e);
                    return;
                }
                subject.OnNext(result);
                subject.OnCompleted();
            };
            var parent = this;
            var args = cloneArray(arguments);
            scheduler.Schedule(function ()
            {
                delayed.apply(parent, args);
            });
            return subject;
        }
    }

    var observableStart = observable.Start = function (original, instance, args, scheduler)
    {
        if (args === _undefined)
            args = [];
        return observableToAsync(original, scheduler).apply(instance, args);
    };

    var connectableObservable = root.ConnectableObservable = function (source, s)
    {
        if (s === _undefined)
            s = new subject();

        this._subject = s;
        this._source = source;
        this._hasSubscription = false;

        this.Connect = function ()
        {
            var group;
            var shouldRun = false;
            if (!this._hasSubscription)
            {
                this._hasSubscription = true;
                var parent = this;
                group = new compositeDisposable(disposableCreate(function ()
                {
                    parent._hasSubscription = false;
                }));
                this._subscription = group;
                group.Add(source.Subscribe(this._subject));
            }

            return this._subscription;
        };

        this._subscribe = function (observer)
        {
            return this._subject.Subscribe(observer);
        };

        this.RefCount = function ()
        {
            var count = 0;
            var connectable = this;
            var connectableSubscription;

            return observableCreate(function (observer)
            {
                var shouldConnect = false;

                count++;
                shouldConnect = count == 1;

                var subscription = connectable.Subscribe(observer);

                if (shouldConnect)
                    connectableSubscription = connectable.Connect();

                return function ()
                {
                    subscription.Dispose();

                    count--;
                    if (count == 0)
                        connectableSubscription.Dispose();
                };
            });
        }
    };

    connectableObservable.prototype = new observable;

})();


