namespace Rambles.Data {
    public class Ramble {
        public string Text { get; }

        public string Id => _info.Id;
        public DateTime LastWriteTime => _info.LastWriteTime;
        public string Url => Id;
        public string Title => _attributes.Title ?? Id;
        // YamlDotNet (or rather the Linux .NET runtime) cannot System.Convert.ChangeType DateOnly.
        public DateOnly? Date => _attributes.Date is DateTime dateTime ? DateOnly.FromDateTime(dateTime) : null;
        public int? HeaderIndex => _attributes.HeaderIndex;

        private RambleInfo _info { get; }
        private RambleAttributes _attributes { get; }

        public RambleInfo GetInfo() => _info;

        public Ramble(RambleInfo info, string text, RambleAttributes attributes) {
            _info = info;
            Text = text;
            _attributes = attributes;
        }
    }

    public class RambleAttributes {
        public string? Title { get; set; }
        public DateTime? Date { get; set; }
        public int? HeaderIndex { get; set; }
    }

    public class RambleInfo {
        public string Id { get; }
        public DateTime LastWriteTime { get; }
        public RambleInfo(string id, DateTime lastWriteTime) {
            Id = id;
            LastWriteTime = lastWriteTime;
        }
    }
}