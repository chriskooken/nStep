using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nucumber.Core.Features
{
	public class Table
	{
		public IList<Row> Rows { get; set; }

		public static Table Parse(string tableText)
		{
			return new Table(tableText);
		}

		public Table()
		{
			Rows = new List<Row>();
		}

		private Table(string tableText)
		{
			Rows = new List<Row>();
			var lines = tableText.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).AsEnumerable();

			foreach (var line in lines)
			{
				Rows.Add(new Row(line.Trim()));
			}
			foreach (var row in Rows)
			{
				if(row.Columns.Count != Rows.First().Columns.Count)
				{
					throw new FormatException("All rows must have equal columns");
				}
			}
		}
	}

	public class Row
	{
		public Row(string line)
		{
			//Leading Pipe
			if(line.First() != '|')
				throw new FormatException("Table needs to begin with a '|'");
            
			//Trailing Pipe
			if (line.Last() != '|')
				throw new FormatException("Table needs to end with a '|'");

			var cols = line.Split(new[] {"|"}, StringSplitOptions.RemoveEmptyEntries);

			foreach (var s in cols)
			{
				columns.Add(new Column(s.Trim()));
			}
		}

		public Row(IList<Column> columns)
		{
			this.columns = columns;
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