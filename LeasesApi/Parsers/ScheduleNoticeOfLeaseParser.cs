using LeasesApi.DTOs;
using System.Text;
using static System.Reflection.Metadata.BlobBuilder;

namespace LeasesApi.Parsers
{
    /// <inheritdoc cref="IScheduleNoticeOfLeaseParser"/>
    /// <remarks>
    /// This implementation only works for inputs inside the Basic Multilingual Plane (not chars like 🐂𐒻𐓟).
    /// It is also assumed that whitespace comes as spaces ' '.
    /// </remarks>
    public class ScheduleNoticeOfLeaseParser : IScheduleNoticeOfLeaseParser
    {
        private readonly ILogger<ScheduleNoticeOfLeaseParser> _logger;

        public ScheduleNoticeOfLeaseParser(ILogger<ScheduleNoticeOfLeaseParser> logger)
        {
            _logger = logger;
        }

        public virtual List<TextBlock> Parse(List<string> entryText)
        {
            var allEntries = PreParse(entryText);

            var (regularEntries, irregularEntry, notes) = ClassifyEntries(allEntries);

            var paddedEntries = AddPadding(regularEntries);

            var coverage = AggregateTextCoverage(paddedEntries);

            var blockRanges = GetBlockRanges(coverage);

            var regularBlocks = GetBlocks(paddedEntries, blockRanges);

            var blocks = AppendIrregularEntry(regularBlocks, irregularEntry);

            return blocks;
        }

        protected virtual List<string> PreParse(List<string> entries)
        {
            _logger.LogTrace("Parsing {RowsCount} entries.", entries.Count);
            return entries.ToList(); // Don't assume ownership of passed entries, create own copy to modify.
        }

        protected virtual (List<string> regularEntries, string? irregularEntry, List<string> notes) ClassifyEntries(List<string> entries)
        {
            var regularEntries = new List<string>();
            string? irregularEntry = null!;
            var notes = new List<string>();

            foreach (var entry in entries)
            {
                if (entry.StartsWith("NOTE")) // TODO: More robust note matching algorithm.
                {
                    notes.Add(entry);
                }
                else if (!entry.EndsWith(' ')) // TODO: More robust algorithm for matching trailing rows that can't be matched to a block.
                {
                    if (irregularEntry is not null)
                    {
                        throw new ParserException($"Expected no more than 1 irregular entry.");
                    }
                    irregularEntry = entry;
                }
                else
                {
                    regularEntries.Add(entry);
                }
            }

            return (regularEntries, irregularEntry, notes);
        }

        protected virtual List<string> AddPadding(List<string> entries)
        {
            var maxLength = entries.Max(x => x.Length);

            return entries.Select(x => x.PadLeft(maxLength)).ToList();
        }

        protected virtual bool[] AggregateTextCoverage(List<string> entries)
        {
            var maxLength = entries.Max(x => x.Length);
            var aggregatedTextCoverage = Enumerable.Repeat(false, maxLength).ToArray();

            foreach (var entry in entries)
            {
                if (entry.Length != maxLength)
                {
                    continue;
                }

                // Assuming no surrogate chars.
                for (int i = 0; i < maxLength; i++)
                {
                    if (entry[i] != ' ') // TODO: Accomodate other whitespace chars.
                    {
                        aggregatedTextCoverage[i] = true;
                    }
                }
            }

            return aggregatedTextCoverage;
        }

        /// <summary>
        /// Identifies blocks delimited by at least two spaces.
        /// </summary>
        /// <param name="coverage"></param>
        /// <returns></returns>
        protected virtual List<(int start, int end)> GetBlockRanges(bool[] coverage)
        {
            if (!coverage.Any())
            {
                return new();
            }

            var blocks = new List<(int, int)>();
            int start = -1;
            bool passedSingleSpace = false;

            for (int i = 0; i < coverage.Length; i++)
            {
                if (coverage[i])
                {
                    passedSingleSpace = false;

                    if (start == -1)
                    {
                        start = i;
                    }
                }
                else if ((!coverage[i]) && start != -1)
                {
                    if (passedSingleSpace)
                    {
                        blocks.Add((start, i - 2));
                        start = -1;
                        passedSingleSpace = false;
                    }
                    else
                    {
                        passedSingleSpace = true;
                    }
                }
            }

            if (start != -1)
            {
                blocks.Add((start, coverage.Length - (passedSingleSpace ? 2 : 1)));
            }

            return blocks;
        }

        protected virtual List<TextBlock> GetBlocks(List<string> entries, List<(int start, int end)> blockRanges)
        {
            var blocks = new List<TextBlock>();

            var stringBuilder = new StringBuilder();

            for (int i = 0; i < blockRanges.Count; i++)
            {
                var (start, end) = blockRanges[i];
                var block = new TextBlock();

                blocks.Add(block);

                foreach (var entry in entries)
                {
                    var substring = entry.AsSpan(start, end - start + 1).Trim();
                    if (substring.IsWhiteSpace())
                    {
                        break;
                    }
                    stringBuilder.Append(substring).Append(' ');
                    block.Height++;
                }

                stringBuilder.Remove(stringBuilder.Length - 1, 1);
                block.Text = stringBuilder.ToString();

                stringBuilder.Clear();
            }

            return blocks;
        }

        protected List<TextBlock> AppendIrregularEntry(List<TextBlock> blocks, string? irregularEntry)
        {
            if (irregularEntry is null)
            {
                return blocks;
            }

            var block = blocks.MaxBy(x => x.Height) ?? throw new ParserException($"{nameof(blocks)} cannot be empty.");

            block.Text += ' ' + irregularEntry;

            return blocks;
        }

        protected virtual void ValidateParseOutput(List<TextBlock> blocks)
        {
            if (blocks.Count != 4)
            {
                throw new ParserException($"Expected 4 blocks count, actual block count: {blocks.Count}");
            }
        }

        protected virtual (List<string> blocks, List<string> notes) PostParse(List<string> blocks, List<string> notes)
        {
            _logger.LogTrace("Parsing done.");

            return (blocks, notes);
        }
    }
}
