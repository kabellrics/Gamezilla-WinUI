using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GameZilla.Core.Models.Emulateur;
public partial class Emulateur
{
    [JsonProperty("Id")]
    public string Id
    {
        get; set;
    }

    [JsonProperty("Name")]
    public string Name
    {
        get; set;
    }

    [JsonProperty("Website", NullValueHandling = NullValueHandling.Ignore)]
    public Uri Website
    {
        get; set;
    }

    [JsonProperty("Profiles", NullValueHandling = NullValueHandling.Ignore)]
    public Profile[] Profiles
    {
        get; set;
    }
}

public partial class Profile
{
    [JsonProperty("Name")]
    public string Name
    {
        get; set;
    }

    [JsonProperty("StartupArguments", NullValueHandling = NullValueHandling.Ignore)]
    public string StartupArguments
    {
        get; set;
    }

    [JsonProperty("Platforms", NullValueHandling = NullValueHandling.Ignore)]
    public string[] Platforms
    {
        get; set;
    }

    [JsonProperty("ImageExtensions", NullValueHandling = NullValueHandling.Ignore)]
    public string[] ImageExtensions
    {
        get; set;
    }

    [JsonProperty("ProfileFiles", NullValueHandling = NullValueHandling.Ignore)]
    public string[] ProfileFiles
    {
        get; set;
    }

    [JsonProperty("StartupExecutable", NullValueHandling = NullValueHandling.Ignore)]
    public string StartupExecutable
    {
        get; set;
    }
}
public partial class Emulateur
{
    public static Emulateur FromJson(string json) => JsonConvert.DeserializeObject<Emulateur>(json, Converter.Settings);
}

public static partial class Serialize
{
    public static string ToJson(this Emulateur self) => JsonConvert.SerializeObject(self, Converter.Settings);
}

