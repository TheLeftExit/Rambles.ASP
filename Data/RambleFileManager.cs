namespace Rambles.Data {
    public static class RambleFileManager {
        public static IEnumerable<RambleInfo> EnumerateRambleInfo() {
            return Directory.EnumerateFiles(RambleSettings.Default.RootDirectory)
                .Where(x => Path.GetExtension(x) == ".md")
                .Select(path => new RambleInfo(
                        Id: Path.GetFileName(path),
                        LastWriteTime: new FileInfo(path).LastWriteTimeUtc
                    )
                );
        }

        public static async Task<string> ReadRambleTextAsync(string id) {
            string path = Path.Combine(RambleSettings.Default.RootDirectory, id);
            return await File.ReadAllTextAsync(path);
        }
    }
}
