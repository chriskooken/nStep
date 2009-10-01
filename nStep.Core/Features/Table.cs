using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nStep.Core.Features
{
	public class Table
	{
		public IList<Row> Rows { get; private set; }

		public Table(IList<Row> rows)
		{
			Rows = rows;
		}
	}

	public class Row
	{
		public IList<Column> Columns { get; private set; }
		public int LineNumber { get; set; }

		public Row(IList<Column> columns)
		{
			Columns = columns;
		}
	}

	public class Column
	{
		public string Value { get; private set; }

		public Column(string @value)
		{
			Value = @value;
		}
	}
}