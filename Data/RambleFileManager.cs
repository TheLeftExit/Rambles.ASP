namespace Rambles.Data {
    public class RambleFileManager {
        public string RootPath { get; }

        public RambleFileManager(string rootPath) {
            RootPath = rootPath;
        }

        public IEnumerable<RambleInfo> EnumerateRambleInfo() {
            return Directory.EnumerateFiles(RootPath)
                .Where(x => Path.GetExtension(x) == ".md")
                .Select(path => new RambleInfo(
                        id: GetId(path),
                        lastWriteTime: GetLastWriteTime(path)
                    )
                );
        }

        public async Task<string> ReadTextAsync(string id) {
            string path = GetFullPath(id);
            return await File.ReadAllTextAsync(path);
        }

        private DateTime GetLastWriteTime(string path) {
            return new FileInfo(path).LastWriteTimeUtc;
        }

        private string GetFullPath(string id) {
            return Path.Combine(RootPath, id);
        }

        private string GetId(string path) {
            return Path.GetFileName(path);
        }
    }
}
