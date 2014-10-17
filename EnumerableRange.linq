<Query Kind="Statements">
  <NuGetReference>Ix_Experimental-Main</NuGetReference>
  <NuGetReference>Ix-Async</NuGetReference>
  <NuGetReference>Rx-Main</NuGetReference>
  <Namespace>System.Collections.ObjectModel</Namespace>
  <Namespace>System.Linq</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var query = Enumerable.Range(1,10)
	.Where(num => num % 2 == 0)
	.Do(num => Task.Delay((10 - num) * 200).Wait());

query.Dump();