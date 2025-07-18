using System.Data;

namespace RtanRPG.Utils.Extension
{
    public static class DataTableExtensions
    {
        private const string IndexColumnName = "#";

        public static List<string> ToList(this DataTable table, int count, bool isIndexVisible = false)
        {
            // Set the index column.
            if (isIndexVisible)
            {
                table = table.Copy();
                table.Columns.Add(IndexColumnName, typeof(int)).SetOrdinal(0);

                foreach (var row in table.AsEnumerable())
                {
                    row[IndexColumnName] = table.Rows.IndexOf(row) + 3;
                }
            }

            var rows = table.AsEnumerable().Take(count).ToArray();
            var columns = table.Columns.Cast<DataColumn>().ToArray();

            var names = columns.Select(column => column.ColumnName);
            var rowLengths = GetRowLineLengths(rows, columns);
            var indentLengths = GetIndentLengths(rows, columns);

            var list = new List<string>();

            // Top border
            list.Add($"{Console.Border.Normal.LeftTopEdge}{string.Join(Console.Border.Normal.TopCross, indentLengths.Select(l => new string('─', l + 2)))}{Console.Border.Normal.RightTopEdge} ");

            // Table name row
            list.Add($"│ {string.Join(" │ ", names.Select((name, index) => name.PadRight(indentLengths[index] - name.GetUnicodeLength())))} │ ");

            foreach (var row in GetAlignedBody(rows, columns, rowLengths, indentLengths))
            {
                list.Add($"├{string.Join('┼', indentLengths.Select(l => new string('─', l + 2)))}┤ ");

                foreach (var column in row)
                {
                    list.Add($"│ {string.Join(" │ ", column)} │ ");
                }
            }

            // Bottom border
            list.Add($"└{string.Join('┴', indentLengths.Select(l => new string('─', l + 2)))}┘ ");

            return list;
        }

        private static List<int> GetRowLineLengths(DataRow[] rows, DataColumn[] columns)
        {
            var lengths = new List<int>();

            for (var i = 0; i < rows.Length; i++)
            {
                var length = 1;
                for (var j = 0; j < columns.Length; j++)
                {
                    if (rows[i][columns[j]] is List<string> texts)
                    {
                        length = Math.Max(length, texts.Count);
                    }
                }

                lengths.Add(length);
            }

            return lengths;
        }

        private static List<int> GetIndentLengths(DataRow[] rows, DataColumn[] columns)
        {
            var lengths = new List<int>();

            for (var i = 0; i < columns.Length; i++)
            {
                var length01 = columns[i].ColumnName.GetGraphicLength();
                var length02 = -1;

                for (var j = 0; j < rows.Length; j++)
                {
                    var length03 = length01;
                    switch (rows[j][columns[i]])
                    {
                        case List<string> texts:
                            length03 = texts.Select(text => text.GetGraphicLength()).Max();
                            break;
                        default:
                            var text = rows[j][columns[i]].ToString();
                            length03 = text?.GetGraphicLength() ?? length01;
                            break;
                    }

                    length02 = Math.Max(length02, Math.Max(length01, length03));
                }

                lengths.Add(rows.Length == 0 ? length01 : length02);
            }

            return lengths;
        }

        private static Alignment GetAlignment(Type type)
        {
            var name = type.ToString().ToLower();
            switch (name)
            {
                case var _ when name.StartsWith("system.byte"): return Alignment.Right;
                case var _ when name.StartsWith("system.sbyte"): return Alignment.Right;
                case var _ when name.StartsWith("system.short"): return Alignment.Right;
                case var _ when name.StartsWith("system.ushort"): return Alignment.Right;
                case var _ when name.StartsWith("system.int"): return Alignment.Right;
                case var _ when name.StartsWith("system.uint"): return Alignment.Right;
                case var _ when name.StartsWith("system.long"): return Alignment.Right;
                case var _ when name.StartsWith("system.ulong"): return Alignment.Right;
                case var _ when name.StartsWith("system.float"): return Alignment.Right;
                case var _ when name.StartsWith("system.double"): return Alignment.Right;
                case var _ when name.StartsWith("system.decimal"): return Alignment.Right;
                default: return Alignment.Left;
            }
        }

        private static List<List<List<string>>> GetAlignedBody(DataRow[] rows, DataColumn[] columns, List<int> columnLengths, List<int> indentLengths)
        {
            var body = new List<List<List<string>>>();

            for (var i = 0; i < rows.Length; i++)
            {
                var column = new List<List<string>>();
                for (var j = 0; j < columnLengths[i]; j++)
                {
                    column.Add(new List<string>());
                }

                for (var j = 0; j < columns.Length; j++)
                {
                    for (var k = 0; k < columnLengths[i]; k++)
                    {
                        int length;
                        Alignment alignment;
                        int indent;
                        switch (rows[i][columns[j]])
                        {
                            case List<string> elements:
                                if (k < elements.Count)
                                {
                                    length = elements[k].GetUnicodeLength();
                                    alignment = GetAlignment(columns[j].DataType);
                                    indent = (alignment == Alignment.Left ? -1 : 1) * (indentLengths[j] - length);

                                    column[k].Add(string.Format($"{{0,{indent}}}", elements[k]));
                                }
                                else
                                {
                                    length = " ".GetUnicodeLength();
                                    alignment = GetAlignment(columns[j].DataType);
                                    indent = (alignment == Alignment.Left ? -1 : 1) * (indentLengths[j] - length);

                                    column[k].Add(string.Format($"{{0,{indent}}}", " "));
                                }

                                break;
                            default:
                                if (k < 1)
                                {
                                    var text = rows[i][columns[j]].ToString() ?? string.Empty;
                                    length = text.GetUnicodeLength();
                                    alignment = GetAlignment(columns[j].DataType);
                                    indent = (alignment == Alignment.Left ? -1 : 1) * (indentLengths[j] - length);

                                    column[k].Add(string.Format($"{{0,{indent}}}", text));
                                }
                                else
                                {
                                    length = " ".GetUnicodeLength();
                                    alignment = GetAlignment(columns[j].DataType);
                                    indent = (alignment == Alignment.Left ? -1 : 1) * (indentLengths[j] - length);

                                    column[k].Add(string.Format($"{{0,{indent}}}", " "));
                                }

                                break;
                        }
                    }
                }

                body.Add(column);
            }

            return body;
        }

        private enum Alignment
        {
            Left, Right
        }
    }
}