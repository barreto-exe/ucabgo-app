using Newtonsoft.Json;

namespace UcabGo.App.Utils;

public static class EnviromentVariables
{
    public static string GetValue(string key)
    {
        using var stream = FileSystem.OpenAppPackageFileAsync("data.json").Result;
        using var reader = new StreamReader(stream);
        var jsonVariables = reader.ReadToEnd();

        //Convert the json into a dictionary
        var variables = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonVariables);

        //Return the value of the key if it exists, otherwise return null
        return variables.TryGetValue(key, out string value) ? value : null;
    }
}
