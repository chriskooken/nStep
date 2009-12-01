using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nStep.Framework.Exceptions;

namespace nStep.Framework.Steps
{
	public class Table
	{
		public IEnumerable<string> ColumnHeadings { get; private set; }
		public IList<Row> Rows { get; private set; }
		private int maxColumnWidth;

		public Table(IEnumerable<Row> rowsIncludingHeader)
		{
			var headerRow = rowsIncludingHeader.First();
			var dataRows = rowsIncludingHeader.Skip(1);

			ColumnHeadings = (from c in headerRow.Cells
			                  select c.Value).ToArray();
			Rows = dataRows.ToList();

			GetMaxColumnWidth(rowsIncludingHeader);
		}

		void GetMaxColumnWidth(IEnumerable<Row> rowsIncludingHeader)
		{
			foreach (var row in rowsIncludingHeader)
			{
				foreach (var cell in row.Cells)
				{
					maxColumnWidth = Math.Max(maxColumnWidth, cell.Value.Length);
				}
			}
		}

		public IEnumerable<IDictionary<string, string>> GetDictionaries()
		{
			return from r in Rows
			       select r.ToDictionary(ColumnHeadings);
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("|");
			foreach (var heading in ColumnHeadings)
			{
				var padding = maxColumnWidth - heading.Length;
				sb.Append(heading + string.Empty.PadRight(padding));
				sb.Append("|");
			}
			sb.AppendLine("");
			foreach (var row in Rows)
			{
				sb.AppendLine(row.ToString(maxColumnWidth));
			}

			return sb.ToString();
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
				dictionary[columnHeading] = Cells[i++].Value;

			return dictionary;
		}

		public string ToString(int maxColumnWidth)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("|");
			foreach (var list in Cells)
			{
				var padding = maxColumnWidth - list.Value.Length;
				sb.Append( list.Value + string.Empty.PadRight(padding));
				sb.Append("|");
			}
            
			return sb.ToString();
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