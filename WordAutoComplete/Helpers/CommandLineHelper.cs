using System;
using System.Collections;
using System.Linq;
using WordAutoComplete.Enums;

namespace WordAutoComplete.Helpers
{
  public class CommandLineHelper
  {
    #region "Public methods"

    public static ArrayList GetCommand()
    {
      Console.Write("\n> ");
      string command = Console.ReadLine();
      return ParseCommand(command);
    }

    #endregion "Public methods"

    #region "Private methods"

    private static ArrayList ParseCommand(string command)
    {
      string[] args = command.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(w => w.ToLower()).ToArray();
      if (!args.Any())
        return null;
      if (args[0][0] != '-' && args.Length == 1)
        return new ArrayList { CommandName.Match, args[0] };
      switch (args[0].ToLower())
      {
        case "-add":
          if (args.Length == 3)
            return new ArrayList { CommandName.Add, args[1], args[2] };
          else
            break;
        case "-del":
          if (args.Length == 2)
            return new ArrayList { CommandName.Del, args[1] };
          else
            break;
        case "-get":
          if (args.Length == 2)
            return new ArrayList { CommandName.Get, args[1] };
          else
            break;
        case "-match":
          return new ArrayList { CommandName.Match, args.Length == 2 ? args[1] : string.Empty };
        default:
          Console.WriteLine("\nCommand not found.");
          break;
      }
      return null;
    }

    #endregion "Private methods"
  }
}
