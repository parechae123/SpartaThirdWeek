using System.Data;

using Border = Test.Utils.Console.Border.Normal;

namespace Test.Utils.Extension
{
    public static class DataTableExtensions
    {
        private const string IndexColumnName = "#";

        public static List<string> ToArray(this DataTable table, int count)
        {
            var rows = table.AsEnumerable().Take(count).ToArray();
            var columns = table.Columns.Cast<DataColumn>().ToArray();

            var names = columns.Select(column => column.ColumnName);
            var lengths = GetIndentLengths(rows, columns);

            var list = new List<string>();

            var enumerable = lengths.Select(length => new string(Border.HorizontalLine, length + 2));
            var text = string.Join(Border.TopCross, enumerable);
            list.Add($"{Border.LeftTopEdge}{text}{Border.RightTopEdge} ");

            enumerable = names.Select((name, index) => name.PadRight(lengths[index] - name.GetUnicodeLength()));
            text = string.Join($" {Border.VerticalLine} ", enumerable);
            list.Add($"{Border.VerticalLine} {text} {Border.VerticalLine} ");

            foreach (var row in GetAlignedBody(rows, columns, lengths))
            {
                enumerable = lengths.Select(length => new string(Border.HorizontalLine, length + 2));
                text = string.Join(Border.CenterCross, enumerable);
                list.Add($"{Border.LeftCross}{text}{Border.RightCross} ");

                foreach (var column in row)
                {
                    text = string.Join($" {Border.VerticalLine} ", column);
                    list.Add($"{Border.VerticalLine} {text} {Border.VerticalLine} ");
                }
            }

            enumerable = lengths.Select(length => new string(Border.HorizontalLine, length + 2));
            text = string.Join(Border.BottomCross, enumerable);
            list.Add($"{Border.LeftBottomEdge}{text}{Border.RightBottomEdge} ");

            return list;
        }

        public static List<int> GetRowLineLengths(this DataTable table, int count)
        {
            var rows = table.AsEnumerable().Take(count).ToArray();
            var columns = table.Columns.Cast<DataColumn>().ToArray();

            return GetRowLineLengths(rows, columns);
        }

        private static List<int> GetRowLineLengths(DataRow[] rows, DataColumn[] columns)
        {
            var lengths = Enumerable.Repeat(1, rows.Length).ToList();

            for (var i = 0; i < columns.Length; i++)
            {
                for (var j = 0; j < rows.Length; j++)
                {
                    if (rows[j][columns[i]] is List<string> texts)
                    {
                        lengths[j] = Math.Max(lengths[j], texts.Count);
                    }
                }
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

        private static List<List<List<string>>> GetAlignedBody(DataRow[] rows, DataColumn[] columns, List<int> indents)
        {
            var lengths = GetRowLineLengths(rows, columns);
            var body = new List<List<List<string>>>();
            for (var i = 0; i < rows.Length; i++)
            {
                var column = new List<List<string>>();
                for (var j = 0; j < lengths[i]; j++)
                {
                    column.Add(new List<string>());
                }
                
                body.Add(column);
            }
            
            for (var i = 0; i < columns.Length; i++)
            {
                var column = columns[i];
                var alignment = GetAlignment(column.DataType);

                for (var j = 0; j < rows.Length; j++)
                {
                    var value = rows[j][column];

                    for (var k = 0; k < lengths[j]; k++)
                    {
                        string text;

                        if (value is List<string> list)
                        {
                            text = k < list.Count ? list[k] : " ";
                        }
                        else
                        {
                            text = k == 0 ? value?.ToString() ?? string.Empty : " ";
                        }

                        var factor = alignment == Alignment.Left ? -1 : 1;
                        var indent = factor * (indents[i] - text.GetUnicodeLength());

                        body[j][k].Add(string.Format($"{{0,{indent}}}", text));
                    }
                }
            }

            return body;
        }
        
        private static Alignment GetAlignment(Type type)
        {
            var name = type.ToString().ToLower();
            switch (name)
            {
                case var _ when name.StartsWith("system.byte"):
                case var _ when name.StartsWith("system.sbyte"):
                case var _ when name.StartsWith("system.short"):
                case var _ when name.StartsWith("system.ushort"):
                case var _ when name.StartsWith("system.int"):
                case var _ when name.StartsWith("system.uint"):
                case var _ when name.StartsWith("system.long"):
                case var _ when name.StartsWith("system.ulong"):
                case var _ when name.StartsWith("system.float"):
                case var _ when name.StartsWith("system.double"):
                case var _ when name.StartsWith("system.decimal"): return Alignment.Right;
                default:                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        return Alignment.Left;
            }
        }

        private enum Alignment
        {
            Left, Right
        }
    }
}