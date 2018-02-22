﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Musoq.Service.Client.Helpers
{
    public static class MapHelper
    {
        public static IReadOnlyList<T> MapToType<T>(ResultTable table)
            where T : new()
            => MapToType<T>(table, new Dictionary<string, Func<object, object>>());

        public static IReadOnlyList<T> MapToType<T>(ResultTable table, Dictionary<string, Func<object, object>> converters)
            where T : new()
        {
            var items = new List<T>();

            var destType = typeof(T);
            var destProperties = destType.GetProperties().ToDictionary(f => f.Name);

            foreach (var row in table.Rows)
            {
                var obj = new T();
                for (int i = 0; i < row.Length; i++)
                {
                    var columnName = table.Columns[i];
                    var mapResult = row[i];

                    if (converters.ContainsKey(table.Columns[i]))
                        mapResult = converters[columnName](mapResult);

                    destProperties[columnName].SetValue(obj, mapResult);
                }

                items.Add(obj);
            }

            return items;
        }
    }
}
