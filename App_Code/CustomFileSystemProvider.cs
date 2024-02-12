using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
 
public class CustomHybridFileSystemProvider : PhysicalFileSystemProvider
{
    const string StoragePath = "C:\temp";

    public CustomHybridFileSystemProvider(string rootFolder)
        : base(rootFolder) { }

    public override System.IO.Stream ReadFile(FileManagerFile file)
    {
        string filePath = GetPhysicalFilePath(file.RelativeName);
        return File.Exists(filePath) ? File.OpenRead(filePath) : Stream.Null;
    }

    string GetPhysicalFilePath(string relativeName)
    {
        return Path.Combine(StoragePath, relativeName);
    }
}