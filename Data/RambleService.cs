using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Syntax;
using System.Diagnostics;
using YamlDotNet.Serialization;

namespace Rambles.Data {
    public partial class RambleService {
        // Storage & public API
        private Dictionary<string, Ramble> _ramblesByUrl =
            new(StringComparer.OrdinalIgnoreCase);

        public Ramble GetRambleByUrl(string url) {
            url = url.Trim('/');
            url = string.IsNullOrEmpty(url)
                ? "index.md"
                : Path.ChangeExtension(url, ".md");
            return _ramblesByUrl.TryGetValue(url, out Ramble ramble)
                ? ramble
                : Ramble.NotFound;
        }

        public IEnumerable<Ramble> GetAllRambles() {
            return _ramblesByUrl.Values;
        }

        public async Task Update() {
            RambleInfo[] updatedInfo = RambleFileManager.EnumerateRambleInfo().ToArray();
            var isUpdated = _ramblesByUrl.Values
                .Select(x => x.GetRambleInfo())
                .SequenceEqual(updatedInfo);
            if (!isUpdated) {
                var tasks = updatedInfo.Select(GetRambleAsync);
                var result = await Task.WhenAll(tasks);
                _ramblesByUrl = result.ToDictionary(
                        x => x.Id,
                        x => x
                    );
            }
        }

        private async Task<Ramble> GetRambleAsync(RambleInfo info) {
            var rawText = await RambleFileManager.ReadRambleTextAsync(info.Id);
            var markdownDocument = Markdown.Parse(rawText, RambleSettings.MarkdownPipeline);
            var text = markdownDocument.ToHtml(RambleSettings.MarkdownPipeline);
            var attributes = GetAttributes(markdownDocument);
            return new Ramble(text, info, attributes);
        }

        private RambleAttributes GetAttributes(MarkdownDocument document) {
            string rawAttributes = document
                .Descendants<YamlFrontMatterBlock>()
                .FirstOrDefault()?
                .Lines
                .Lines
                .Select(x => $"{x}\n")
                .Where(x => !string.IsNullOrWhiteSpace(x) && !x.StartsWith("---"))
                .Aggregate(string.Empty, string.Concat);

            if (string.IsNullOrEmpty(rawAttributes))
                return new();

            return RambleSettings.YamlDeserializer.Deserialize<RambleAttributes>(rawAttributes);
        }
    }
}
