namespace Configuration.Configurations
{
    public class SalesforceConfiguration
    {
        public LoginConfiguration Credentials { get; set; }
        public bool AutoActivateSandboxes { get; set; }
        public string ApexClassId { get; set; }
    }
}