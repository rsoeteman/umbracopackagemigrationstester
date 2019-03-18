using System;
using Newtonsoft.Json;

namespace PackageMigrationTester.Model
{
    public class MigrationInfo
    {
        [JsonProperty("migrationClassName")]
        public string MigrationClassName{get;set;}

        [JsonProperty("assemblyQualifiedName")]
        public string AssemblyQualifiedName{get;set;}
    }
}