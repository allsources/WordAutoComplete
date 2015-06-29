using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WordAutoComplete.Classes;
using WordAutoComplete.Enums;
using WordAutoComplete.Helpers;
using WordAutoComplete.Services;
using WordAutoComplete.WordCollections;
using WordAutoComplete.WordTrie;

namespace WordAutoComplete
{
  class Program
  {
    #region "Private members"

    private static ServiceMode _serviceMode;

    private static HashSet<Word> _wordDictionary;

    private static IEnumerable<string> _wordPrefixCollection;

    private static IWordCollection _wordCollection;

    private static WordClient _client;
    
    private static WordServer _server;

    #endregion "Private members"

    #region "Main"

    static void Main(string[] args)
    {
      if(!Initialization(args))
        return;

      switch (_serviceMode)
      {
        case ServiceMode.Standalone:
          ProcessStandalone();
          break;
        case ServiceMode.Client:
          ProcessClient();
          break;
        case ServiceMode.Server:
          ProcessServer();
          break;
      }
    }

    #endregion "Main"

    #region "Initialization"

    private static bool Initialization(string[] args)
    {
        Enum.TryParse<ServiceMode>(ConfigurationManager.AppSettings["ServiceMode"], out _serviceMode);
        switch (_serviceMode)
      {
        case ServiceMode.Client:
          return InitializeClient();
        case ServiceMode.Server:
          return InitializeServer(args);
        default:
          return InitializeStandalone(args);
      }
    }

    private static bool InitializeStandalone(string[] args)
    {
      GetWordDictionaryData();
      GetPrefixCollectionData();
      InitializeWordsCollection(args);
      BuildWordsCollection();

      return
        _wordPrefixCollection != null && _wordPrefixCollection.Any() && _wordCollection != null && _wordCollection.HasData();
    }

    private static bool InitializeServer(string[] args)
    {
      Console.WriteLine("I'm a Server.");
      GetWordDictionaryData();
      InitializeWordsCollection(args);
      BuildWordsCollection();
      int port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
      _server = new WordServer(port, ProcessRequestData);

      return
        _wordCollection != null && _wordCollection.HasData();
    }

    private static bool InitializeClient()
    {
      Console.WriteLine("I'm a Client.");
      string host = ConfigurationManager.AppSettings["Host"];
      int port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
      _client = new WordClient(host, port);
      return _client != null;
    }

    private static void GetWordDictionaryData()
    {
      var sw = new Stopwatch();
      Console.Write("Load word dictionary... ");
      _wordDictionary = ResourceHelper.GetDictionaryData();
      sw.Stop();
      Console.WriteLine(string.Format("{0} ms", sw.ElapsedMilliseconds));
    }

    private static void GetPrefixCollectionData()
    {
      var sw = new Stopwatch();
      Console.Write("Load word prefix collection... ");
      _wordPrefixCollection = ResourceHelper.GetPrefixCollectionData();
      sw.Stop();
      Console.WriteLine(string.Format("{0} ms", sw.ElapsedMilliseconds));
    }

    private static void InitializeWordsCollection(string[] args)
    {
      StandaloneMode mode = StandaloneMode.Trie;
      if (args.Length == 1)
        Enum.TryParse<StandaloneMode>(args[0].Substring(1), true, out mode);
      int mostPopularWordsLimit = Convert.ToInt32(ConfigurationManager.AppSettings["MostPopularWordsLimit"]);
      var config = new WordNodeConfig { MostPopularWordsLimit = mostPopularWordsLimit };
      _wordCollection = CommonHelper.CreateWordsCollection(mode, config);
      Console.WriteLine("Standalone mode: {0}", mode.ToString());
    }

    private static void BuildWordsCollection()
    {
      var sw = new Stopwatch();
      Console.Write("Build words collection... ");
      sw.Start();
      _wordCollection.Build(_wordDictionary);
      sw.Stop();
      Console.WriteLine(string.Format("{0} ms", sw.ElapsedMilliseconds));
    }

    #endregion "Initialization"

    #region "Process Standalone mode"

    private static void ProcessStandalone()
    {
      while (true)
      {
        ArrayList command = CommandLineHelper.GetCommand();
        if (command == null)
          continue;
        switch ((CommandName)command[0])
        {
          case CommandName.Add:
            AddWord(new Word((string)command[1], Convert.ToInt32(command[2])));
            break;
          case CommandName.Del:
            DelWord((string)command[1]);
            break;
          case CommandName.Get:
            GetWord((string)command[1]);
            break;
          case CommandName.Match:
            ProcessMatchByPrefix((string)command[1]);
            break;
        }
      }
    }

    private static void AddWord(Word word)
    {
      _wordCollection.Add(word);
    }

    private static void DelWord(string value)
    {
      bool result = _wordCollection.Del(value);
      Console.WriteLine("\nDeleted: {0}", result ? "YES" : "NO");
    }

    private static void GetWord(string value)
    {
      Word word = _wordCollection.Get(value);
      string msg =
        word != null
          ? string.Format("{0} {1}", word.Count, word.Value)
          : "Not found.";
      Console.WriteLine(string.Concat("\n", msg));
    }

    private static void ProcessMatchByPrefix(string prefix)
    {
      var sw = new Stopwatch();
      sw.Start();
      if (string.IsNullOrWhiteSpace(prefix))
        Parallel.ForEach(_wordPrefixCollection, (p) => DisplayMostPopularWords(_wordCollection.Match(p)));
      else
        DisplayMostPopularWords(_wordCollection.Match(prefix));
      sw.Stop();
      Console.WriteLine(string.Format("\nTime elapsed: {0} ms", sw.ElapsedMilliseconds));
    }
    
    private static void DisplayMostPopularWords(IEnumerable<Word> words)
    {
      if (words.Any())
        Console.WriteLine(string.Concat("\n", string.Join("\n", words.Select(w => string.Format("{0} {1}", w.Count, w.Value)))));
      else
        Console.WriteLine("\nNot found.");
    }

    #endregion "Process Standalone mode"

    #region "Process Clinet mode"

    private static void ProcessClient()
    {
      while (true)
      {
        ArrayList command = CommandLineHelper.GetCommand();
        if (command == null)
          continue;
        switch ((CommandName)command[0])
        {
          case CommandName.Match:
            ProcessServerRequest((string)command[1]);
            break;
          default:
            Console.WriteLine("Unsupported command.");
            break;
        }
      }
    }

    private static void ProcessServerRequest(string prefix)
    {
      string result = _client.PerformServerRequest(prefix);
      Console.WriteLine(string.IsNullOrWhiteSpace(result) ? "Not found." : result);
    }

    #endregion "Process Clinet mode"

    #region "Process Server mode"

    private static void ProcessServer()
    {
      _server.Start();
    }

    public static string ProcessRequestData(string prefix)
    {
      if (string.IsNullOrWhiteSpace(prefix))
        return string.Empty;

      IEnumerable<Word> words = _wordCollection.Match(prefix);

      return string.Join("\n", words.Select(w => string.Format("{0} {1}", w.Count, w.Value)));
    }

    #endregion "Process Server mode"
  }
}
