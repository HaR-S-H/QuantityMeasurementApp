namespace QuantityMeasurementApp.Api.Options;

public sealed class GoogleAuthOptions
{
    public const string SectionName = "GoogleAuth";

    public string ClientId { get; set; } = string.Empty;
    
    /// <summary>
    /// Comma-separated list of additional allowed Client IDs for different environments.
    /// Allows tokens from multiple OAuth app configurations (local dev, staging, production, etc.)
    /// </summary>
    public string AllowedClientIds { get; set; } = string.Empty;

    public string[] GetAllowedClientIds()
    {
        var ids = new List<string> { ClientId };
        
        if (!string.IsNullOrWhiteSpace(AllowedClientIds))
        {
            ids.AddRange(
                AllowedClientIds
                    .Split(',')
                    .Select(id => id.Trim())
                    .Where(id => !string.IsNullOrEmpty(id))
            );
        }
        
        return ids.ToArray();
    }
}
