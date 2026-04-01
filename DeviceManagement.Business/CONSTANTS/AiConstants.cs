namespace DeviceManagement.Business.CONSTANTS;

public static class AiConstants
{
    public const string AiSettingsSection = "AiSettings";
    public const string AiSettingsBaseUrl = "AiSettings:BaseUrl";
    public const string AiSettingsApiKey = "AiSettings:ApiKey";
    public const string AiSettingsModelName = "AiSettings:ModelName";
    public const string AiSettingsIntervalInSeconds = "AiSettings:DescriptionGeneratorIntervalInSeconds";

    public const string ChatCompletionsEndpoint = "api/v1/chat";

    public const string DeviceDescriptionSystemPrompt =
        "You are a technical writer specializing in IT device documentation. " +
        "Your task is to write a concise, clear, and informative human-readable description " +
        "of a device based on its technical specifications. " +
        "The description must be exactly 2-3 sentences long. " +
        "Do not include any headers, bullet points, or markdown. " +
        "Write in plain prose. Do not mention the word 'device'.";

    public const string DeviceDescriptionUserPromptTemplate =
        "Write a concise description for the following device:\n" +
        "Name: {0}\n" +
        "Manufacturer: {1}\n" +
        "Type: {2}\n" +
        "Operating System: {3} {4}\n" +
        "Processor: {5}\n" +
        "RAM: {6} GB";
}
