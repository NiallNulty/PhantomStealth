using System;
using System.IO;
using UnityEngine;

public class LogManager
{
    static string logFolderPath = "Logs";
    static string archiveFolderPath = @"Logs\Archive";
    static string logFileName = Application.productName + "-" + DateTime.Today.ToString("dd-MM-yyyy") + ".txt";
    static string logFilePath = Path.Combine(logFolderPath, logFileName);


    public static void Log(string message)
    {
#if UNITY_EDITOR
        Debug.Log(message);
#endif

#if PLATFORM_STANDALONE_WIN
        LogToFile(message);
#endif
    }

    private static void LogToFile(string message)
    {
        try
        {
            //Create the Logs directory if it doesn't exist
            if (!Directory.Exists(logFolderPath))
            {
                Directory.CreateDirectory(logFolderPath);
            }

            //make file editable so logging can be added
            if (File.Exists(logFilePath))
            {
                File.SetAttributes(logFilePath, File.GetAttributes(logFilePath) & ~FileAttributes.ReadOnly);
            }

            // Append the log message to the file
            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                writer.WriteLine(DateTime.Now.ToString() + ": " + message);
            }

            //make file read-only to prevent end users modifying the file
            File.SetAttributes(logFilePath, File.GetAttributes(logFilePath) | FileAttributes.ReadOnly);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private static void ArchiveOldLogs()
    {
        try
        {
            //Create the archive directory if it doesn't exist
            if (!Directory.Exists(archiveFolderPath))
            {
                Directory.CreateDirectory(archiveFolderPath);
            }

            // Get files in the source directory
            string[] files = Directory.GetFiles(logFolderPath);

            // Get today's date
            DateTime currentDate = DateTime.Today;

            foreach (string file in files)
            {
                // Get the creation date of the file
                DateTime creationDate = File.GetCreationTime(file);

                // Compare the creation date with today's date
                if (creationDate.Date < currentDate.Date)
                {
                    // Get the file name
                    string fileName = Path.GetFileName(file);

                    // Construct the destination path in the archive directory
                    string destinationPath = Path.Combine(archiveFolderPath, fileName);

                    // Move the file to the archive directory
                    File.Move(file, destinationPath);

                    Console.WriteLine($"File '{fileName}' archived successfully.");
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}
