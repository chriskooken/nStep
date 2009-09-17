using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nucumber.Framework
{
    public class Table
    {
        IList<Row> rows = new List<Row>();

        public static Table Parse(string tableText)
        {
            var lines = tableText.Split(new[] {@"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            throw new NotImplementedException();
            
        }

        
    }

    internal class Row
    {
        IList<Column> columns = new List<Column>();
    }

    internal class Column
    {
    }
}
