using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nucumber.Core.Features
{
	public class Table
	{
		public IList<Row> Rows { get; set; }

		public Table()
		{
			Rows = new List<Row>();
		}
	}

	public class Row
	{
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

		public int LineNumber { get; set; }
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