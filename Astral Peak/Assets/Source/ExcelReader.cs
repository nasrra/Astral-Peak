using System;
using System.Collections.Generic;
using ExcelDataReader; // For reading Excel files
using System.IO;        // For working with file streams
using UnityEngine;
using System.Linq.Expressions;

public static class ExcelReader{
    public static List<string> read_dialogue(string file, string column){
        List<string> dialogue = new List<string>();
        string path = Path.Combine(Application.streamingAssetsPath, $"{file}.xlsb");
        //string path = $"Assets/Resources/Dialogue/{file}.xlsb";
        try{
            using(var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None))
            using(var reader = ExcelReaderFactory.CreateReader(stream)){
                int colIndex = -1;
                bool isHeaderRow = true;

                while (reader.Read()){
                    if (isHeaderRow){
                        // Find the column index in the header row
                        for (int i = 0; i < reader.FieldCount; i++)
                            if (reader.GetString(i) == column){
                                colIndex = i;
                                break;
                            }
                        if (colIndex == -1)
                            throw new SystemException($"{column} not found!");
                        isHeaderRow = false;
                    }
                    else{
                        // Read data rows
                        if (colIndex < 0 || reader.FieldCount <= colIndex)
                            continue;
                        var value = reader.GetValue(colIndex)?.ToString();
                        if (string.IsNullOrEmpty(value))
                            break;
                        dialogue.Add(value.Trim());
                    }
                }
            }
        }
        catch(Exception e){
            return new List<string>(){e.ToString()};
        }

        return dialogue;
    }



}
