﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Musoq.Evaluator.Tables;
using Musoq.Evaluator.TemporarySchemas;
using Musoq.Schema;
using Musoq.Schema.DataSources;

namespace Musoq.Evaluator.Helpers
{
    public static class EvaluationHelper
    {
        public static TableRowSource ConvertTableToSource(Table table)
        {
            return new TableRowSource(table);
        }

        public static Table GetSchemaDescription(ISchemaTable table)
        {
            var newTable = new Table("desc", new[]
            {
                new Column("Name", typeof(string), 0),
                new Column("Index", typeof(int), 1),
                new Column("Type", typeof(string), 2)
            });

            foreach (var column in table.Columns)
            {
                newTable.Add(new ObjectsRow(new object[] { column.ColumnName, column.ColumnIndex, column.ColumnType.Name }));
            }

            return newTable;
        }

        public static string GetCastableType(Type type)
        {
            if (type.IsGenericType) return GetFriendlyTypeName(type);

            return $"{type.Namespace}.{type.Name}";
        }

        public static Type[] GetNestedTypes(Type type)
        {
            if (!type.IsGenericType)
                return new[] {type};

            var types = new Stack<Type>();

            types.Push(type);
            var finalTypes = new List<Type>();

            while (types.Count > 0)
            {
                var cType = types.Pop();
                finalTypes.Add(cType);

                if (cType.IsGenericType)
                    foreach (var argType in cType.GetGenericArguments())
                        types.Push(argType);
            }

            return finalTypes.ToArray();
        }

        private static string GetFriendlyTypeName(Type type)
        {
            if (type.IsGenericParameter) return type.Name;

            if (!type.IsGenericType) return type.FullName;

            var builder = new StringBuilder();
            var name = type.Name;
            var index = name.IndexOf("`", StringComparison.Ordinal);
            builder.AppendFormat("{0}.{1}", type.Namespace, name.Substring(0, index));
            builder.Append('<');
            var first = true;
            foreach (var arg in type.GetGenericArguments())
            {
                if (!first) builder.Append(',');
                builder.Append(GetFriendlyTypeName(arg));
                first = false;
            }

            builder.Append('>');
            return builder.ToString();
        }
    }
}