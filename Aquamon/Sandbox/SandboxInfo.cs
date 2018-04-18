namespace Aquamon
{
    public class SandboxInfo
    {
        public string SandboxName { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string LicenseType { get; set; }
        public string ApexClassId { get; set; }
        public bool AutoActivate { get; set; }

        public SandboxInfo()
        {
            AutoActivate = true;
            LicenseType = "DEVELOPER";
            Description = "Sandbox created via API";
        }

        public string ToJSON()
        {
            return $@"
            {{
                ""AutoActivate"": {AutoActivate},
                ""SandboxName"": ""{SandboxName}"",
                ""Description"": ""{Description}"",
                ""LicenseType"": ""{LicenseType}"",
                ""ApexClassId"": ""{ApexClassId}""
            }}";
        }
    }
}