using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nucumber.Framework
{
    public class Table
    {
        IList<Row> rows = new List<Row>();
        public IList<Row> Rows { get
        { return rows;} }

        public static Table Parse(string tableText)
        {
            return new Table(tableText);
        }

        public Table()
        {
            
        }

        private Table(string tableText)
        {
            var lines = tableText.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).AsEnumerable();

            foreach (var line in lines)
            {
                rows.Add(new Row(line.Trim()));
            } 
        }
    }

    public class Row
    {
        public Row(string line)
        {
            var cols = line.Split(new[] {"|"}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var s in cols)
            {
                columns.Add(new Column(s.Trim()));
            }
        }

        IList<Column> columns = new List<Column>();
        public IList<Column> Columns
        {
            get
            { return columns; }
        }
    }

    public class Column
    {
        public string Value { get; set; }
        public Column(string cell)
        {
            Value = cell;
        }
    }
}
