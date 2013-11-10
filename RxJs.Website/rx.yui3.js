// Copyright (c) Microsoft Corporation.  All rights reserved.
// This code is licensed by Microsoft Corporation under the terms
// of the MICROSOFT REACTIVE EXTENSIONS FOR JAVASCRIPT AND .NET LIBRARIES License.
// See http://go.microsoft.com/fwlink/?LinkId=186234.

(function(){var a=Rx.Observable.YUI3Use=function(c){return Rx.Observable.CreateWithDisposable(function(d){var e=function(g){d.OnNext(g);};var f=YUI().use(c,e);return Rx.Disposable.Empty;});};var b=Rx.Observable.FromYUI3Event=function(c,d){return a("node-base").SelectMany(function(e){return Rx.Observable.Create(function(f){var g=function(h){f.OnNext(h);};e.on(d,g,c);return function(){e.detach(d,g,c);};});});};})();