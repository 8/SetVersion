using Mono.Options;

namespace SetVersion
{
  public enum ActionType
  {
    Help,
    List,
    SetVersion
  }

  public class ParametersModel
  {
    public OptionSet Options { get; set; }
    public ActionType Action { get; set; }
    public string SourceFile { get; set; }
    public string TargetFile { get; set; }
    public string Version { get; set; }
  }
}
