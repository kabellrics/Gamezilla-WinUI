using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GameZilla.Core.Models.Emulateur;
public partial class Platforms
{
    [JsonProperty("Name")]
    public string Name
    {
        get; set;
    }

    [JsonProperty("Id")]
    public string Id
    {
        get; set;
    }

    [JsonProperty("IgdbId", NullValueHandling = NullValueHandling.Ignore)]
    public long? IgdbId
    {
        get; set;
    }

    [JsonProperty("Databases", NullValueHandling = NullValueHandling.Ignore)]
    public string[] Databases
    {
        get; set;
    }

    [JsonProperty("Emulators", NullValueHandling = NullValueHandling.Ignore)]
    public string[] Emulators
    {
        get; set;
    }
}
public partial class Platforms
{
    public static Platforms[] FromJson(string json) => JsonConvert.DeserializeObject<Platforms[]>(json, Converter.Settings);
}
public static partial class Serialize
{
    public static string ToJson(this Platforms[] self) => JsonConvert.SerializeObject(self, Converter.Settings);
}

