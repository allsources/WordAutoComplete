using System;
using System.Collections.Generic;
using System.Linq;
using WordAutoComplete.Classes;

namespace WordAutoComplete.WordCollections
{
  /// <summary>
  /// Represents words collection based on <see cref="Dictionary"/> object.
  /// </summary>
  public class WordDictionaryCollection : Dictionary<string, Word>, IWordCollection
  {
    #region "PRIVATE MEMBERS"

    private readonly int _mostPopularWordsLimit;

    #endregion "PRIVATE MEMBERS"

    #region ".ctor"

    public WordDictionaryCollection(int mostPopularWordsLimit)
    {
      _mostPopularWordsLimit = mostPopularWordsLimit;
    }

    #endregion ".ctor"

    #region "INTERFACE METHODS IMPLEMENTATION"

    public void Build(IEnumerable<Word> words)
    {
      base.Clear();
      foreach(Word word in words)
        base.Add(word.Value, word);
    }

    new public void Add(Word word)
    {
      Word w = this.Get(word.Value);
      if (w != null)
        w.Count += word.Count;
      else
        base.Add(word.Value, word);
    }

    public Word Get(string value)
    {
      return
        base.ContainsKey(value)
          ? this[value]
          : null;
    }

    public bool Del(string value)
    {
      return
        this.Remove(value);
    }

    public IEnumerable<Word> Match(string prefix)
    {
      return
        this
          .Where(w => w.Key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
          .OrderByDescending(w => w.Value.Count).ThenBy(w => w.Key)
          .Take(_mostPopularWordsLimit)
          .Select(w => w.Value);
    }

    public bool HasData()
    {
      return this.Any();
    }

    #endregion "INTERFACE METHODS IMPLEMENTATION"
  }
}
