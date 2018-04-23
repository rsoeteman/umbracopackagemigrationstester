using System;
using Newtonsoft.Json;

namespace PackageMigrationTester.Model
{
    public class MigrationInfo
    {
        [JsonProperty("migrationClassName")]
        public string MigrationClassName{get;set;}

        [JsonProperty("targetVersion")]
        public string TargetVersion{get;set;}
    }
}