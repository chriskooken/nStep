using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nStep.Core.Features
{
	public class Table
	{
		public IEnumerable<string> ColumnHeadings { get; private set; }
		public IList<Row> Rows { get; private set; }

		public Table(IEnumerable<Row> rowsIncludingHeader)
		{
			var headerRow = rowsIncludingHeader.First();
			var dataRows = rowsIncludingHeader.Skip(1);

			ColumnHeadings = (from c in headerRow.Cells
							  select c.Value).ToArray();
			Rows = dataRows.ToList(); ;
		}

		public IEnumerable<IDictionary<string, string>> GetDictionaries()
		{
			return from r in Rows
			        select r.ToDictionary(ColumnHeadings);
		}
	}

	public class Row
	{
		public IList<Cell> Cells { get; private set; }
		public int LineNumber { get; set; }

		public Row(IList<Cell> columns)
		{
			Cells = columns;
		}

		public IDictionary<string, string> ToDictionary(IEnumerable<string> columnHeadings)
		{
			var dictionary = new Dictionary<string, string>();
			var columnCount = columnHeadings.Count();

			if (Cells.Count != columnCount)
				throw new JaggedTableException(this, columnCount);

			var i = 0;
			foreach (var columnHeading in columnHeadings)
				dictionary[columnHeading] = Cells[i].Value;

			return dictionary;
		}
	}

	public class Cell
	{
		public string Value { get; private set; }

		public Cell(string @value)
		{
			Value = @value;
		}
	}
}