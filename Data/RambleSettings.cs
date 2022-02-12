using Markdig;
using System.Globalization;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

public class RambleSettings {
    public static RambleSettings Default { get; private set; }

    public static void ApplyDefault(string yamlConfig) =>
        Default = YamlDeserializer.Deserialize<RambleSettings>(yamlConfig);

    private static MarkdownPipeline _markdownPipeline;
    public static MarkdownPipeline MarkdownPipeline =>
        _markdownPipeline ??= new MarkdownPipelineBuilder()
            .UseYamlFrontMatter()
            .Build();

    private static IDeserializer _yamlDeserializer;
    public static IDeserializer YamlDeserializer =>
        _yamlDeserializer ??= new DeserializerBuilder()
            .IgnoreUnmatchedProperties()
            .WithTypeConverter(new DateOnlyConverter())
            .Build();

    public string WebsiteName { get; init; }
    public string BaseUrl { get; init; }
    public string RootDirectory { get; init; }
}

// Enforcing invariant culture for DateOnly parsing
public class DateOnlyConverter : IYamlTypeConverter {
    public bool Accepts(Type type) => type == typeof(DateOnly?);

    public object ReadYaml(IParser parser, Type type) {
        var value = parser.Consume<Scalar>().Value;
        return DateOnly.Parse(value, CultureInfo.InvariantCulture);
    }

    public void WriteYaml(IEmitter emitter, object value, Type type) {
        throw new NotSupportedException();
    }
}