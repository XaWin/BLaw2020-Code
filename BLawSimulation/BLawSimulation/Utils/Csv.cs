using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace BLawSimulation.Utils
{
    public static class Csv
    {
        public static void Write(DataTable dt, FileInfo fileOut)
        {
            //before your loop
            var csv = new StringBuilder();

            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            csv.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                csv.AppendLine(string.Join(",", fields));
            }

            //after your loop
            File.WriteAllText(fileOut.FullName, csv.ToString());
        }
    }
}
