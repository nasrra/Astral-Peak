using System;
using System.Text.Json;
using System.IO;
using UnityEngine;

namespace Entropek{

public static class FileManager{
    private static string default_path = "";
    public static void set_default_file_path(string _path, string _file_name) => default_path = Path.Combine(_path,_file_name); 

    /// <summary>
    /// Saves data to a JSON file in the default file path.
    /// </summary>
    /// <typeparam name="T">The type of the data to save.</typeparam>
    /// <param name="data">The data to save.</param>
    public static void save_data<T>(T data) => save_data(data, default_path);

    /// <summary>
    /// Saves data to a JSON file.
    /// </summary>
    /// <typeparam name="T">The type of the data to save.</typeparam>
    /// <param name="data">The data to save.</param>
    /// <param name="path">The file path where the data will be saved.</param>
    public static void save_data<T>(T data, string path){
        try{
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            string jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, jsonData);
        }
        catch (Exception e){
            throw new Exception($"Failed to save data to {path}", e);
        }
    }

    /// <summary>
    /// Loads data from a JSON file from the default file path.
    /// </summary>
    /// <typeparam name="T">The type of the data to load.</typeparam>
    /// <returns>The loaded data.</returns>
    public static T load_data<T>() where T : new() => load_data<T>(default_path);

    /// <summary>
    /// Loads data from a JSON file.
    /// </summary>
    /// <typeparam name="T">The type of the data to load.</typeparam>
    /// <param name="path">The file path to load the data from.</param>
    /// <returns>The loaded data.</returns>
    public static T load_data<T>(string path) where T : new(){
        try{
            if (!File.Exists(path)){
                #if UNITY_EDITOR
                Debug.LogWarning($"File not found: {path}");
                #endif
                return new T();
            }
            string jsonData = File.ReadAllText(path);
            return JsonSerializer.Deserialize<T>(jsonData);
        }
        catch (Exception e){
            throw new Exception($"Failed to load data from {path}", e);
        }
    }

    public static bool file_exists() => File.Exists(default_path);
    public static void delete_data() => delete_data(default_path);
    public static void delete_data(string path){
        try{
            if (!File.Exists(path)){
                #if UNITY_EDITOR
                Debug.LogWarning($"File deleteion UNSUCCESSFUL, File not found: {path}");
                #endif
            }
            else{
                File.Delete(path);
                #if UNITY_EDITOR
                Debug.Log("File deletion SUCCESSFUL");
                #endif
            }
        }
        catch(Exception e){
            throw new Exception($"Failed to load path: {path}. ",e);
        }
    }
}

}
