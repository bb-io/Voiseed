using Blackbird.Applications.Sdk.Common.Exceptions;
using ClosedXML.Excel;

namespace Apps.Voiseed.Utils
{
    public static class BatchExcelBuilder
    {
        public const string COL_ID = "ID";
        public const string COL_CHARACTER = "Character";
        public const string COL_EMOTION = "Emotion";
        public const string COL_INTENSITY = "Intensity";
        public const string COL_TEXT = "Text";

        public static MemoryStream BuildXlsx(
            IEnumerable<string> columns,
            IEnumerable<int>? ids,
            IEnumerable<string>? characters,
            IEnumerable<string>? emotions,
            IEnumerable<string>? intensities,
            IEnumerable<string> texts,
            string languageId
        )
        {
            var cols = NormalizeColumns(columns);

            if (!cols.Contains(COL_TEXT))
                throw new PluginMisconfigurationException("Columns must include 'Text'.");

            var rowsCount = texts?.Count() ?? 0;
            if (rowsCount == 0)
                throw new PluginMisconfigurationException("Texts must not be empty.");

            void CheckLen(IEnumerable<string>? arr, string name)
            {
                if (arr is null) return;
                var c = arr.Count();
                if (c != rowsCount)
                    throw new PluginMisconfigurationException($"'{name}' length ({c}) must equal Texts length ({rowsCount}).");
            }
            void CheckLenInt(IEnumerable<int>? arr, string name)
            {
                if (arr is null) return;
                var c = arr.Count();
                if (c != rowsCount)
                    throw new PluginMisconfigurationException($"'{name}' length ({c}) must equal Texts length ({rowsCount}).");
            }

            CheckLenInt(ids, nameof(ids));
            if (cols.Contains(COL_CHARACTER)) CheckLen(characters, nameof(characters));
            if (cols.Contains(COL_EMOTION)) CheckLen(emotions, nameof(emotions));
            if (cols.Contains(COL_INTENSITY)) CheckLen(intensities, nameof(intensities));

            var idSeq = ids?.ToArray() ?? Enumerable.Range(1, rowsCount).ToArray();
            var chSeq = characters?.ToArray();
            var emSeq = emotions?.ToArray();
            var inSeq = intensities?.ToArray();
            var txSeq = texts.ToArray();

            var ms = new MemoryStream();
            using (var wb = new XLWorkbook())
            {
                var ws = wb.AddWorksheet("Revoiceit_Prod_Template_Script");

                int col = 1;
                var colMap = new Dictionary<string, int>();
                foreach (var key in cols)
                {
                    string header = key == COL_TEXT ? $"Text_{languageId}" : key;
                    ws.Cell(1, col).Value = header;
                    colMap[key] = col;
                    col++;
                }

                for (int i = 0; i < rowsCount; i++)
                {
                    int r = i + 2;
                    foreach (var key in cols)
                    {
                        int c = colMap[key];
                        switch (key)
                        {
                            case COL_ID: ws.Cell(r, c).Value = idSeq[i]; break;
                            case COL_CHARACTER: ws.Cell(r, c).Value = chSeq?[i] ?? ""; break;
                            case COL_EMOTION: ws.Cell(r, c).Value = emSeq?[i] ?? ""; break;
                            case COL_INTENSITY: ws.Cell(r, c).Value = inSeq?[i] ?? ""; break;
                            case COL_TEXT: ws.Cell(r, c).Value = txSeq[i]; break;
                        }
                    }
                }

                ws.Columns().AdjustToContents();
                wb.SaveAs(ms);
            }
            ms.Position = 0;
            return ms;
        }

        private static IReadOnlyList<string> NormalizeColumns(IEnumerable<string> columns)
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var c in columns ?? Array.Empty<string>())
            {
                var k = c.Trim();
                if (k.Equals(COL_ID, StringComparison.OrdinalIgnoreCase)) set.Add(COL_ID);
                else if (k.Equals(COL_CHARACTER, StringComparison.OrdinalIgnoreCase)) set.Add(COL_CHARACTER);
                else if (k.Equals(COL_EMOTION, StringComparison.OrdinalIgnoreCase)) set.Add(COL_EMOTION);
                else if (k.Equals(COL_INTENSITY, StringComparison.OrdinalIgnoreCase)) set.Add(COL_INTENSITY);
                else if (k.Equals(COL_TEXT, StringComparison.OrdinalIgnoreCase)) set.Add(COL_TEXT);
            }

            if (!set.Contains(COL_TEXT)) set.Add(COL_TEXT);

            var ordered = new List<string>();
            if (set.Contains(COL_ID)) ordered.Add(COL_ID);
            if (set.Contains(COL_CHARACTER)) ordered.Add(COL_CHARACTER);
            if (set.Contains(COL_EMOTION)) ordered.Add(COL_EMOTION);
            if (set.Contains(COL_INTENSITY)) ordered.Add(COL_INTENSITY);
            ordered.Add(COL_TEXT);
            return ordered;
        }
    }
}
