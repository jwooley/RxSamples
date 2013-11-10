// Copyright (c) Microsoft Corporation.  All rights reserved.
// This code is licensed by Microsoft Corporation under the terms
// of the MICROSOFT REACTIVE EXTENSIONS FOR JAVASCRIPT AND .NET LIBRARIES License.
// See http://go.microsoft.com/fwlink/?LinkId=186234.

(function(){var a=Rx.Observable.FromDojoEvent=function(b,c,d,e){return Rx.Observable.Create(function(f){var g=function(i){f.OnNext(i);};var h=dojo.connect(b,c,d,g,e);return function(){dojo.disconnect(h);};});};})();