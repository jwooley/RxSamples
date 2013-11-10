using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.FxCop.Sdk;
using CodeAnalysisRules;

namespace GMS_Rules
{
    public class ConstructorMustCallInitializeComponent : Microsoft.FxCop.Sdk.BaseIntrospectionRule
    {
        public ConstructorMustCallInitializeComponent() : base("ConstructorMustCallInitializeComponent", "", null) { }



        public override ProblemCollection Check(TypeNode type)
        {
            Debug.WriteLine("Checking type:" + type.FullName);
            var initializer = type.Members.OfType<Method>().FirstOrDefault(x => x.FullName == type.FullName + ".InitializeComponent");

            if (initializer == null)
                return null;
            Debug.WriteLine(initializer.FullName);
            var constructorsWithNoInitCall = type.Members.OfType<Method>().Where(m => m.NodeType == NodeType.InstanceInitializer).ToList();
            var visitedMethods = new HashSet<string>();
            var foundMethods = new ObservableHashSet<Method>();


            var whenMethodsFound = Observable.FromEvent<NotifyCollectionChangedEventArgs>(foundMethods, "CollectionChanged")
                .Where(e => e.EventArgs.Action == NotifyCollectionChangedAction.Add
                    && constructorsWithNoInitCall.Any());


           

            whenMethodsFound.Subscribe(e => Parallel.ForEach(e.EventArgs.NewItems.OfType<Method>()
                                     .Where(m => !visitedMethods.Any(v => v == m.FullName)),
                                        i =>
                                        {
                                            lock (visitedMethods)
                                            {
                                                if (visitedMethods.Contains(i.FullName))
                                                    return;
                                                visitedMethods.Add(i.FullName);
                                            }
                                            Debug.WriteLine("Visiting:" + i.FullName);
                                            var callers = (CallGraph.CallersFor(i));
                                            constructorsWithNoInitCall.RemoveAll(x => callers.Any(c => x.FullName == c.FullName));
                                            if (constructorsWithNoInitCall.Any())
                                                foreach (var item in callers.Where(c => visitedMethods.Any(v => v == c.FullName) == false))
                                                {
                                                    foundMethods.Add(item);
                                                }
                                        }));


            //e =>
            //{
            //                Parallel.ForEach(e.EventArgs.NewItems.OfType<Method>()
            //                                    .Where(m => !visitedMethods.Any(v => v == m.FullName) ),
            //                    i =>
            //                    {

            //                        lock (visitedMethods)
            //                        {
            //                            if (visitedMethods.Contains(i.FullName))
            //                                return;
            //                            visitedMethods.Add(i.FullName);

            //                        }
            //                        Debug.WriteLine("Visiting:" + i.FullName);
            //                        var callers = (CallGraph.CallersFor(i));
            //                        constructorsWithNoInitCall.RemoveAll(x => callers.Any(c => x.FullName == c.FullName));
            //                        if (constructorsWithNoInitCall.Any())
            //                            foreach (var item in callers.Where(c => visitedMethods.Any(v => v == c.FullName) == false))
            //                            {
            //                                foundMethods.Add(item);
            //                            }
            //                    });
            //));



            foundMethods.Add(initializer);

            //whenMethodsFound.Run();

            //_currentType=type;
            ReportProblem(constructorsWithNoInitCall, type);

            return Problems;
        }
        private void ReportProblem(IEnumerable<Method> badConstructorMethods, TypeNode type)
        {
            //sourceContext is wrong
            foreach (var constructor in badConstructorMethods)
            {



                Func<Instruction, string, bool> compareFullName =
                    (i, fullName) => ((InstanceInitializer)i.Value).FullName.StartsWith(fullName + ".#ctor");

                var source2 = constructor.Instructions.Where(i => i.Value is InstanceInitializer && i.SourceContext.FileName != null)
                    .FirstOrDefault(i => compareFullName(i, type.FullName)
                    || compareFullName(i, type.BaseType.FullName));

                Problems.Add(new Problem(this.GetResolution(constructor.FullName), source2 != null ? source2.SourceContext : type.SourceContext));
            }

        }



    }
}

