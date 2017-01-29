namespace SetVersion
{
  using Mono.Options;

  public interface IParametersFactory
  {
    ParametersModel GetParameters(string[] args);
  }

  class ParametersFactory : IParametersFactory
  {
    public ParametersModel GetParameters(string[] args)
    {
      var options = new OptionSet();
      var parameters = new ParametersModel();
      parameters.Options = options;
      options.Add("i=", "the input file to load", s => parameters.SourceFile = s);
      options.Add("o=", "the output file to save", s => parameters.TargetFile = s);
      options.Add("v|--version=", "the version string", s => parameters.Version = s);
      options.Add("l|--list", "list the versions", s => parameters.Action = ActionType.List);
      options.Add("s", "set the version", s => parameters.Action = ActionType.SetVersion);
      options.Add("h|--help", "prints the help", s => parameters.Action = ActionType.Help);
      options.Parse(args);
      return parameters;
    }
  }
}
