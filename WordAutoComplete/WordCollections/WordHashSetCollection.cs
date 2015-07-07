using System;
using System.Collections.Generic;
using System.Linq;
using WordAutoComplete.Classes;

namespace WordAutoComplete.WordCollections
{
  /// <summary>
  /// Represents words collection based on <see cref="HashSet"/> object.
  /// This class created to compare performance with the prefix-tree class.
  /// </summary>
  public class WordHashSetCollection : HashSet<Word>, IWordCollection
  {
    #region "PRIVATE MEMBERS"

    private readonly int _mostPopularWordsLimit;

    #endregion "PRIVATE MEMBERS"

    #region ".ctor"

    public WordHashSetCollection(int mostPopularWordsLimit)
    {
      _mostPopularWordsLimit = mostPopularWordsLimit;
    }

    #endregion ".ctor"

    #region "INTERFACE METHODS IMPLEMENTATION"

    public void Build(IEnumerable<Word> words)
    {
      base.Clear();
      foreach(Word word in words)
        base.Add(word);
    }

    new public void Add(Word word)
    {
      Word w = this.Get(word.Value);
      if (w != null)
        w.Count += word.Count;
      else
        base.Add(word);
    }

    public Word Get(string value)
    {
      return
        this.FirstOrDefault(w => w.Value.Equals(value, StringComparison.OrdinalIgnoreCase));
    }

    public bool Del(string value)
    {
      Word word = this.Get(value);
      return
        word != null
          ? this.Remove(word)
          : false;
    }

    public IEnumerable<Word> Match(string prefix)
    {
      return
        this
          .Where(w => w.Value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
          .OrderByDescending(w => w.Count).ThenBy(w => w.Value)
          .Take(_mostPopularWordsLimit);
    }

    public bool HasData()
    {
      return this.Any();
    }

    #endregion "INTERFACE METHODS IMPLEMENTATION"
  }
}
