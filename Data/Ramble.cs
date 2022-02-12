namespace Rambles.Data {
    public class Ramble {
        public Ramble(string text, RambleInfo info, RambleAttributes attributes) {
            Text = text;
            _info = info;
            _attributes = attributes;
        }
        private RambleInfo _info;
        private RambleAttributes _attributes;

        public RambleInfo GetRambleInfo() => _info;

        public string Text { get; }
        public string Id => _info.Id;
        public DateTime LastWriteTime => _info.LastWriteTime;
        public string Title => _attributes.Title ?? Id;
        public DateOnly? Date => _attributes.Date;
        public int? HeaderIndex => _attributes.HeaderIndex;

        public static Ramble NotFound
            = new Ramble(
                text: @"<h2>Not Found</h2><p>...back to <a href=""/"">index</a>?</p>",
                info: new("notfound.md", DateTime.MinValue),
                attributes: new() { Title = "NotFound" }
            );
        public bool Is404 => this == NotFound;
    }

    public record RambleInfo(string Id, DateTime LastWriteTime);

    public class RambleAttributes {
        public string Title { get; init; }
        public DateOnly? Date { get; init; }
        public int? HeaderIndex { get; init; }
        public bool HideFooter { get; init; }
    }
}