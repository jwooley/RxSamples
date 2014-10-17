<Query Kind="Statements">
  <NuGetReference>Rx-Main</NuGetReference>
  <NuGetReference>Rx-WinForms</NuGetReference>
  <Namespace>System.Collections.ObjectModel</Namespace>
  <Namespace>System.Reactive.Concurrency</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var query = Enumerable.Range(1,10)
	.Where(num => num % 2 == 0)
	.Do(num => Task.Delay((10 - num) * 200).Wait());


query.Dump();















//var query = Observable.Range(1,10)
//	.Where(num => num % 2 == 0)
//	.Do(num => Task.Delay((10 - num) * 200).Wait());
//
//query.Subscribe(val => Console.WriteLine(val));