﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Musoq.Plugins.Attributes;
using Musoq.Schema;
using Musoq.Schema.DataSources;
using Musoq.Schema.Managers;

namespace Musoq.Evaluator.Tests.Schema
{
    public class TestSchema<T> : ISchema
        where T : BasicEntity
    {
        private readonly MethodsAggregator _aggreagator;
        private readonly IEnumerable<T> _sources;

        public TestSchema(IEnumerable<T> sources)
        {
            _sources = sources;

            var methodManager = new MethodsManager();
            var propertiesManager = new PropertiesManager();

            var lib = new TestLibrary();

            propertiesManager.RegisterProperties(lib);
            methodManager.RegisterLibraries(lib);

            _aggreagator = new MethodsAggregator(methodManager, propertiesManager);
        }

        public string Name => "test";

        public ISchemaTable GetTableByName(string name, string[] parameters)
        {
            return new BasicEntitySchema();
        }

        public RowSource GetRowSource(string name, string[] parameters)
        {
            return new EntitySource<T>(_sources,
                new Dictionary<string, int>
                {
                        {nameof(BasicEntity.Name), 10},
                        {nameof(BasicEntity.City), 11},
                        {nameof(BasicEntity.Country), 12},
                        {nameof(BasicEntity.Population), 13},
                        {nameof(BasicEntity.Self), 14},
                        {nameof(BasicEntity.Money), 15},
                        {nameof(BasicEntity.Month), 16},
                        {nameof(BasicEntity.Time), 17}
                },
                new Dictionary<int, Func<T, object>>
                {
                        {10, arg => arg.Name},
                        {11, arg => arg.City},
                        {12, arg => arg.Country},
                        {13, arg => arg.Population},
                        {14, arg => arg.Self},
                        {15, arg => arg.Money},
                        {16, arg => arg.Month},
                        {17, arg => arg.Time}
                });
        }

        public MethodInfo ResolveMethod(string method, Type[] parameters)
        {
            return _aggreagator.ResolveMethod(method, parameters);
        }

        public MethodInfo ResolveAggregationMethod(string method, Type[] parameters)
        {
            return _aggreagator.ResolveMethod(method, parameters);
        }

        public bool TryResolveAggreationMethod(string method, Type[] parameters, out MethodInfo methodInfo)
        {
            var founded = _aggreagator.TryResolveMethod(method, parameters, out methodInfo);
            if (founded)
                return methodInfo.GetCustomAttribute<AggregationMethodAttribute>() != null;

            return false;
        }

        public MethodInfo ResolveProperty(string property)
        {
            return _aggreagator.ResolveProperty(property);
        }
    }
}