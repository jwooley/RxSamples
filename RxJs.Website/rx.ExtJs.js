// Copyright (c) Microsoft Corporation.  All rights reserved.
// This code is licensed by Microsoft Corporation under the terms
// of the MICROSOFT REACTIVE EXTENSIONS FOR JAVASCRIPT AND .NET LIBRARIES License.
// See http://go.microsoft.com/fwlink/?LinkId=186234.

(function(){var a=Rx.Observable.FromExtJSEvent=function(b,c,d,e){return Rx.Observable.Create(function(f){var g=function(h){f.OnNext(h);};Ext.EventManager.on(b,c,g,d,e);return function(){Ext.EventManager.un(b,c,g,d);};});};Ext.Element.prototype.ToObservable=function(b,c,d){return a(this,b,c,d);};})();