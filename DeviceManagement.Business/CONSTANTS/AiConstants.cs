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

    public const string ChatSystemPrompt =
        "You are a helpful device management assistant. Using the provided context about devices, answer user questions concisely. " +
        "You MUST integrate the unique identifier for EVERY device you mention directly after its name. " +
        "FORMATTING RULE: Wrap common identifiers in backticks inside square brackets, e.g., [`id1`]. " +
        "NO LABELS: Do not use 'ID:', 'Identifier:', 'Model:', or parentheses. " +
        "NO SEPARATORS: If multiple IDs match, list them one after another inside brackets without commas, e.g., [`id1` `id2`]. " +
        "EXAMPLE CORRECT: 'The Galaxy S23 [`69ca...`] is a great phone.' " +
        "EXAMPLE INCORRECT: 'The Galaxy S23 (ID: `69ca...`)' or 'Galaxy S23 [`id1`, `id2`]'.";

    public const string RagUserPromptTemplate =
        "Context: Here are the devices currently in the system:\n{0}\n\nUser Question: {1}";

    public const string DeviceContextTemplate =
        "- ID: {0}, Name: {1}, Manufacturer: {2}, Type: {3}, OS: {4} {5}, Processor: {6}, RAM: {7} GB, Description: {8}";
}
