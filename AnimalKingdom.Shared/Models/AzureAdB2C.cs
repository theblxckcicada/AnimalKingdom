namespace AnimalKingdom.Shared.Models
{
    public class AzureAdB2C
    {

        public string Instance { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string SignUpSignInPolicyId { get; set; } = string.Empty;
        public string ResetPasswordPolicyId { get; set; } = string.Empty;
        public string EditProfilePolicyId { get; set; } = string.Empty;
        public string CacheFileName { get; set; } = string.Empty;
        public string CacheDir { get; set; } = string.Empty;
    }

    public class DownstreamApi
    {
        public string Scopes { get; set; } = string.Empty;
    }

}
