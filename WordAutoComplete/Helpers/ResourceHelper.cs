using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WordAutoComplete.Classes;
using WordAutoComplete.Loggers;

namespace WordAutoComplete.Helpers
{
  /// <summary>
  /// Represents a helper class for works with resource data.
  /// </summary>
  public static class ResourceHelper
  {
    #region "Private members"

    private const string DICTIONARY_FILE_NAME = @".\Data\WordDictionary.txt";

    private const string PREFIX_COLLECTION_FILE_NAME = @".\Data\WordPrefixCollection.txt";

    #endregion "Private members"

    #region "Public methods"

    public static HashSet<Word> GetDictionaryData()
    {
      string[] lines = LoadResourceRawData(DICTIONARY_FILE_NAME);
      return
        lines != null && lines.Any()
          ? GetWords(lines)
          : new HashSet<Word>();
    }

    public static IEnumerable<string> GetPrefixCollectionData()
    {
      string[] lines = LoadResourceRawData(PREFIX_COLLECTION_FILE_NAME);
      return
        lines != null && lines.Any()
          ? GetPrefixes(lines)
          : new string[]{};
    }

    #endregion "Public methods"

    #region "Private helper methods"

    /// <summary>
    /// Load raw data from file.
    /// </summary>
    private static string[] LoadResourceRawData(string fileName)
    {
      try
      {
        return
          File.ReadAllLines(fileName);
      }
      catch(Exception ex)
      {
        Logger.LogException(ex);
      }
      return null;
    }

    private static HashSet<Word> GetWords(string[] lines)
    {
      IEnumerable<Word> words =
        lines
          .AsParallel()
          .Select(ConvertRawDataToWord);
      return
        new HashSet<Word>(words);
    }

    private static Word ConvertRawDataToWord(string rawData)
    {
      string[] data = rawData.Split(' ');
      return
        data.Length == 2
          ? new Word(data[1], Convert.ToInt32(data[0]))
          : new Word();
    }

    private static IEnumerable<string> GetPrefixes(string[] lines)
    {
      return lines;
    }

    #endregion "Private helper methods"
  }
}
