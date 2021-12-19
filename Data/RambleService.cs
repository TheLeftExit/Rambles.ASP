using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Syntax;
using YamlDotNet.Serialization;

namespace Rambles.Data {
    public partial class RambleService {
        private RambleFileManager _fileManager;

        private MarkdownPipeline _markdownPipeline;
        private IDeserializer _yamlDeserializer;

        private Dictionary<string, Ramble> _ramblesByUrl;

        public RambleService(string contentRoot) {
            _markdownPipeline = new MarkdownPipelineBuilder()
                .UseYamlFrontMatter()
                .Build();

            _yamlDeserializer = new DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .Build();

            _fileManager = new(contentRoot);

            _ramblesByUrl = new(StringComparer.OrdinalIgnoreCase);
        }

        public Ramble GetRambleByUrl(string url) {
            var trimmedUrl = PrepareUrl(url);
            return _ramblesByUrl[trimmedUrl];
        }

        public Ramble[] GetRambles() {
            return _ramblesByUrl.Values.ToArray();
        }

        public async Task Refresh() {
            await UpdateRambles();
        }

        public bool Contains(string url) {
            var trimmedUrl = PrepareUrl(url);
            return _ramblesByUrl.ContainsKey(trimmedUrl);
        }

        private string PrepareUrl(string url) {
            if (url == "/") return "index.md";
            return Path.ChangeExtension(url.TrimStart('/'), ".md");
        }

        private async Task UpdateRambles() {
            // Id : LastWriteTime
            Dictionary<string, DateTime> lastWriteTimesById = _fileManager
                .EnumerateRambleInfo()
                .ToDictionary(
                    x => x.Id,
                    x => x.LastWriteTime
                );

            // 1. Clear removed/outdated entries.
            var urlsToRemove = _ramblesByUrl.Values
                .Where(x =>
                    !lastWriteTimesById.TryGetValue(x.Id, out DateTime lastWriteTime)
                    || lastWriteTime != x.LastWriteTime
                )
                .Select(x => x.Url)
                .ToArray();
            foreach(string url in urlsToRemove) {
                _ramblesByUrl.Remove(url);
            }

            // 2. Add updated/missing entries.
            var rambleIds = _ramblesByUrl.Values
                .Select(x => x.Id)
                .ToHashSet();
            foreach(string id in lastWriteTimesById.Keys) {
                bool toAdd = !rambleIds.Contains(id);
                if (toAdd) {
                    RambleInfo info = new(id, lastWriteTimesById[id]);
                    Ramble ramble = await GetRambleAsync(info);
                    _ramblesByUrl.Add(ramble.Url, ramble);
                }
            }
        }

        private async Task<Ramble> GetRambleAsync(RambleInfo info) {
            var rawText = await _fileManager.ReadTextAsync(info.Id);
            var markdownDocument = Markdown.Parse(rawText, _markdownPipeline);
            var text = markdownDocument.ToHtml(_markdownPipeline);
            var attributes = GetAttributes(markdownDocument);
            return new Ramble(info, text, attributes);
        }

        private RambleAttributes GetAttributes(MarkdownDocument document) {
            string? rawAttributes = document
                .Descendants<YamlFrontMatterBlock>()
                .FirstOrDefault()?
                .Lines
                .Lines
                .Select(x => $"{x}\n")
                .Where(x => !string.IsNullOrWhiteSpace(x) && !x.StartsWith("---"))
                .Aggregate(string.Empty, string.Concat);

            if (string.IsNullOrEmpty(rawAttributes))
                return new();

            return _yamlDeserializer.Deserialize<RambleAttributes>(rawAttributes);
        }
    }
}
