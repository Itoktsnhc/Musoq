﻿namespace Musoq.Schema.Disk.Directories
{
    public class DirectoriesBasedTable : ISchemaTable
    {
        public ISchemaColumn[] Columns { get; } = SchemaDirectoriesHelper.DirectoriesColumns;
    }
}