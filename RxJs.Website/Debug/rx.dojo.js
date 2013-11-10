// Copyright (c) Microsoft Corporation.  All rights reserved.
// This code is licensed by Microsoft Corporation under the terms
// of the MICROSOFT REACTIVE EXTENSIONS FOR JAVASCRIPT AND .NET LIBRARIES License.
// See http://go.microsoft.com/fwlink/?LinkId=186234.

(function()
{
	var fromDojoEvent = Rx.Observable.FromDojoEvent = function(dojoObject, eventType, context, dontFix) 
	{
	    return Rx.Observable.Create(function(observer)
	    {
	        var handler = function(eventObject) 
	        {
	            observer.OnNext(eventObject);
	        };
		var handle = dojo.connect(dojoObject, eventType, context, handler, dontFix);
	        return function() 
	        {
	            dojo.disconnect(handle);
	        };
	    });
	};	
})();


