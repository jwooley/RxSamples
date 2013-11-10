// Copyright (c) Microsoft Corporation.  All rights reserved.
// This code is licensed by Microsoft Corporation under the terms
// of the MICROSOFT REACTIVE EXTENSIONS FOR JAVASCRIPT AND .NET LIBRARIES License.
// See http://go.microsoft.com/fwlink/?LinkId=186234.

(function()
{
	var fromMooToolsEvent = Rx.Observable.FromMooToolsEvent = function(mooToolsObject, eventType) 
	{
	    return Rx.Observable.Create(function(observer)
	    {
	        var handler = function(eventObject) 
	        {
	            observer.OnNext(eventObject);
	        };
	        var handle = mooToolsObject.addEvent(eventType, handler);
	        return function() 
	        {
	            mooToolsObject.removeEvent(eventType, handler);
	        };
	    });
	};
})();