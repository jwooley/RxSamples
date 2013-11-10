// Copyright (c) Microsoft Corporation.  All rights reserved.
// This code is licensed by Microsoft Corporation under the terms
// of the MICROSOFT REACTIVE EXTENSIONS FOR JAVASCRIPT AND .NET LIBRARIES License.
// See http://go.microsoft.com/fwlink/?LinkId=186234.

(function(){var a=Rx.Observable.FromPrototypeEvent=function(b,c){return Rx.Observable.Create(function(d){var e=function(f){d.OnNext(f);};Element.observe(b,c,e);return function(){Element.stopObserving(b,c,e);};});};Element.addMethods({ToObservable:function(b,c){return a(b,c);}});})();