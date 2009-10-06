using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nStep.Core.Features
{
	public class JaggedTableException : ApplicationException
	{
		public JaggedTableException(Row row, int expectedColumns)
			: base(string.Format("Table row on line: {0} should have {1} columns, not {2}", row.LineNumber, expectedColumns, row.Cells.Count))
		{ }
	}
}
