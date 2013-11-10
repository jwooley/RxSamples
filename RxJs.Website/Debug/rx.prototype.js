// Copyright (c) Microsoft Corporation.  All rights reserved.
// This code is licensed by Microsoft Corporation under the terms
// of the MICROSOFT REACTIVE EXTENSIONS FOR JAVASCRIPT AND .NET LIBRARIES License.
// See http://go.microsoft.com/fwlink/?LinkId=186234.

(function()
{
	var fromPrototypeEvent = Rx.Observable.FromPrototypeEvent = function(prototypeElement, eventType) 
	{
	    return Rx.Observable.Create(function(observer)
	    {
	        var handler = function(eventObject) 
	        {
	            observer.OnNext(eventObject);
	        };
		    Element.observe(prototypeElement, eventType, handler);
	        return function() 
	        {
	            Element.stopObserving(prototypeElement, eventType, handler);
	        };
	    });
	};
	
	Element.addMethods( { ToObservable : function(element, eventType) { return fromPrototypeEvent(element, eventType); } });
})();


