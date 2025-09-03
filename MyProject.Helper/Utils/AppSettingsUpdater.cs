using Newtonsoft.Json;

public static class AppSettingsUpdater
{
    public static void UpdateAppSetting(string section, string key, string value, string filePath = "appsettings.json")
    {
        var json = File.ReadAllText(filePath);
        dynamic jsonObj = JsonConvert.DeserializeObject(json);

        if (jsonObj[section] != null)
        {
            jsonObj[section][key] = value;
        }

        string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
        File.WriteAllText(filePath, output);
    }
}
