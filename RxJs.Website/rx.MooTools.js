// Copyright (c) Microsoft Corporation.  All rights reserved.
// This code is licensed by Microsoft Corporation under the terms
// of the MICROSOFT REACTIVE EXTENSIONS FOR JAVASCRIPT AND .NET LIBRARIES License.
// See http://go.microsoft.com/fwlink/?LinkId=186234.

(function(){var a=Rx.Observable.FromMooToolsEvent=function(b,c){return Rx.Observable.Create(function(d){var e=function(g){d.OnNext(g);};var f=b.addEvent(c,e);return function(){b.removeEvent(c,e);};});};})();