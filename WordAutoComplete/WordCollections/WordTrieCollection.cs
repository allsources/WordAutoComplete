using System.Collections.Generic;
using WordAutoComplete.Classes;
using WordAutoComplete.WordTrie;

namespace WordAutoComplete.WordCollections
{
  /// <summary>
  /// Represents words collection based on prefix tree.
  /// </summary>
  public class WordTrieCollection : WordTrieNode, IWordCollection
  {
    #region ".ctor"

    public WordTrieCollection(WordNodeConfig config) : base(config)
    {
    }

    #endregion ".ctor"

    #region "INTERFACE METHODS IMPLEMENTATION"

    new public void Build(IEnumerable<Word> words)
    {
      base.Build(words);
    }

    new public void Add(Word word)
    {
      base.Add(word);
    }

    new public Word Get(string value)
    {
      WordTrieNode node = base.Get(value);
      return
        node != null && node.HasValue
          ? node.Value
          : null;
    }

    new public bool Del(string value)
    {
      return base.Del(value);
    }

    new public IEnumerable<Word> Match(string prefix)
    {
      return
        base.Match(prefix);
    }

    public bool HasData()
    {
      return base.HasChildNodes;
    }

    #endregion "INTERFACE METHODS IMPLEMENTATION"
  }
}
