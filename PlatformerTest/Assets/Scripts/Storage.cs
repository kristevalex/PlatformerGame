using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class Storage
{
    private static string FullPath(string path)
    {
        return Application.persistentDataPath + "/" + path;
    }

    private static string GetFilePath<T>()
    {
        if (typeof(T) == typeof(KeyBindsData))
            return "KeyBinds.userprefs";

        return "";
    }

    public static void Save<Serializable>(string path, Serializable obj)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        using (FileStream stream = new FileStream(FullPath(path), FileMode.Create))
            formatter.Serialize(stream, obj);
    }

    public static void Save<Serializable>(Serializable obj)
    {
        Save(GetFilePath<Serializable>(), obj);
    }

    public static Serializable Load<Serializable>(string path) where Serializable : class, new()
    {
        var fullPath = FullPath(path);
        if (File.Exists(fullPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            var loaded = default(Serializable);
            using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                loaded = formatter.Deserialize(stream) as Serializable;

            return loaded;
        }
        else
            return new Serializable();
    }

    public static Serializable Load<Serializable>() where Serializable : class, new()
    {
        return Load<Serializable>(GetFilePath<Serializable>());
    }

    public static void LoadKeyBinds(KeyBindsData data = null)
    {
        if (data == null)
            data = Load<KeyBindsData>();

        KeyBinds.keyBinds = new Dictionary<string, KeyCode>();
        KeyBinds.keyBinds.Add("Right"    , (KeyCode)System.Enum.Parse(typeof(KeyCode), data.right));
        KeyBinds.keyBinds.Add("Left"     , (KeyCode)System.Enum.Parse(typeof(KeyCode), data.left));
        KeyBinds.keyBinds.Add("Up"       , (KeyCode)System.Enum.Parse(typeof(KeyCode), data.up));
        KeyBinds.keyBinds.Add("Down"     , (KeyCode)System.Enum.Parse(typeof(KeyCode), data.down));
        KeyBinds.keyBinds.Add("Jump"     , (KeyCode)System.Enum.Parse(typeof(KeyCode), data.jump));
        KeyBinds.keyBinds.Add("Dash"     , (KeyCode)System.Enum.Parse(typeof(KeyCode), data.dash));
        KeyBinds.keyBinds.Add("Timelapse", (KeyCode)System.Enum.Parse(typeof(KeyCode), data.timelapse));
    }

    public static void SaveKeyBinds()
    {
        KeyBindsData data = new KeyBindsData();
        data.right     = KeyBinds.keyBinds["Right"].ToString();
        data.left      = KeyBinds.keyBinds["Left"].ToString();
        data.up        = KeyBinds.keyBinds["Up"].ToString();
        data.down      = KeyBinds.keyBinds["Down"].ToString();
        data.jump      = KeyBinds.keyBinds["Jump"].ToString();
        data.dash      = KeyBinds.keyBinds["Dash"].ToString();
        data.timelapse = KeyBinds.keyBinds["Timelapse"].ToString();

        Save<KeyBindsData>(data);
    }

    public static void ResetKeyBinds()
    {
        LoadKeyBinds(new KeyBindsData());
        SaveKeyBinds();
    }
}
