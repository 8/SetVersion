//using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using Vestris.ResourceLib;

namespace SetVersion
{
  class Program
  {
    static void Main(string[] args)
    {
      new Program().Run(args);
        //new ParametersModel
        //{
        //  SourceFile = "SampleApplication.exe",
        //  TargetFile = "SampleApplication_patched.exe",
        //  Version    = "4.3.2.1"
        //});
    }

    private void Log(string text)
    {
      Console.Write($"{text}");
    }

    private void LogLine(string text)
    {
      this.Log($"{text}{System.Environment.NewLine}");
    }

    private void Run(string[] args)
    {
      var p = new ParametersFactory().GetParameters(args);

      switch (p.Action)
      {
        case ActionType.Help:
          this.Help(p);
          break;

        case ActionType.SetVersion:
          this.SetVersions(p);
          break;

        case ActionType.List:
          this.List(p);
          break;
      }
    }

    public void List(ParametersModel p)
    {
      var versionResource = new VersionResource();
      versionResource.LoadFrom(p.SourceFile);

      using (var vi = new ResourceInfo())
      {
        vi.Load(p.SourceFile);
        foreach (ResourceId id in vi.ResourceTypes)
        {
          Console.WriteLine(id);
          foreach (Resource resource in vi.Resources[id])
          {
            Console.WriteLine("{0} ({1}) - {2} byte(s)",
                resource.Name, resource.Language, resource.Size);
          }
        }
      }

      var stringFileInfo = (StringFileInfo)versionResource["StringFileInfo"];
      foreach (KeyValuePair<string, StringTableEntry> stringTableEntry in stringFileInfo.Default.Strings)
      {
        LogLine($"{stringTableEntry.Value.Key} = {stringTableEntry.Value.StringValue}");
      }
    }

    public void Help(ParametersModel p)
    {
      p.Options.WriteOptionDescriptions(Console.Out);
    }

    public void SetVersions(ParametersModel p)
    {
      /* load the version */
      var versionResource = new VersionResource();
      versionResource.LoadFrom(p.SourceFile);
      versionResource.Language = 0; /* bug? language is set when it shouldn't */

      /* update the versions */
      versionResource.FileVersion = p.Version;
      //versionResource.ProductVersion = p.Version;

      var stringFileInfo = (StringFileInfo)versionResource["StringFileInfo"];

      stringFileInfo.Default.Strings["ProductVersion"].Value = p.Version;

      //stringFileInfo.Default.Strings["FileVersion"].Value = p.Version;
      //stringFileInfo.Default.Strings["Assembly Version"].Value = p.Version;

      /* create the new output file */
      if (p.SourceFile != p.TargetFile)
        File.Copy(p.SourceFile, p.TargetFile, true);

      /* save the updated version info to the copy */
      versionResource.SaveTo(p.TargetFile);
    }
  }
}
