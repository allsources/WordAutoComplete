using System.Collections.Generic;
using System.Linq;
using WordAutoComplete.Classes;

namespace WordAutoComplete.WordTrie
{
  /// <summary>
  /// Represents a node in a prefix tree.
  /// </summary>
  public class WordTrieNode
  {
    #region "Private members"

    private WordNodeConfig _config;

    private readonly WordTrieNode _parent;

    private Word _word;

    private MostPopularWords _mostPopularWords;

    private readonly Dictionary<char, WordTrieNode> _childNodes;

    #endregion "Private members"

    #region "Public members"

    public bool IsRoot { get { return _parent == null; } }

    public Word Value { get { return _word; } }

    public bool HasValue { get { return _word != null; } }

    public bool HasChildNodes { get { return _childNodes.Any(); } }

    public IEnumerable<Word> MostPopularWords { get { return _mostPopularWords.Value; } }

    #endregion "Public members"

    #region ".ctor"

    public WordTrieNode(WordNodeConfig config)
    {
      _config = config;
      _mostPopularWords = new MostPopularWords(config.MostPopularWordsLimit);
      _childNodes = new Dictionary<char, WordTrieNode>();
    }

    public WordTrieNode(WordNodeConfig config, WordTrieNode parent, Word wordToAdd, bool isNodeValue) : this(config)
    {
      _parent = parent;
      _word = isNodeValue ? wordToAdd : null;
      _mostPopularWords.Add(wordToAdd);
    }

    #endregion ".ctor"

    #region "BUILD"

    public bool Build(IEnumerable<Word> words)
    {
      if (!this.IsRoot)
        return false;

      if (words == null || !words.Any())
        return false;

      this.Clear();
      foreach (Word word in words)
        this.AddChildNode(word);

      return true;
    }

    public bool Clear()
    {
      if (!this.IsRoot)
        return false;

      _childNodes.Clear();

      return true;
    }

    #endregion "BUILD"

    #region "ADD"

    public WordTrieNode Add(Word word)
    {
      if (!this.IsRoot)
        return null;

      if (word == null || string.IsNullOrWhiteSpace(word.Value))
        return null;

      WordTrieNode node = this.Get(word.Value);
      if (node != null)
        word = node.UpdateValue(word)._word;

      return AddChildNode(word);
    }

    private WordTrieNode AddChildNode(Word word)
    {
      WordTrieNode node = this;
      for (int i = 0; i < word.Value.Length; i++)
        node = node.AddChildNode(i, word);

      return node;
    }

    private WordTrieNode AddChildNode(int index, Word word)
    {
      char symbol = word.Value[index];
      bool isNodeValue = index == word.Value.Length - 1;
      if (!_childNodes.ContainsKey(symbol))
        return AddChildNode(symbol, word, isNodeValue);
      else
        return _childNodes[symbol].UpdateMostPopularWords(word);
    }

    private WordTrieNode AddChildNode(char symbol, Word word, bool isNodeValue)
    {
      _childNodes.Add(symbol, new WordTrieNode(_config, this, word, isNodeValue));
      return
        _childNodes[symbol];
    }

    #endregion "ADD"

    #region "GET"

    public WordTrieNode Get(string prefix)
    {
      if (!this.IsRoot)
        return null;

      if (string.IsNullOrWhiteSpace(prefix))
        return null;

      WordTrieNode node = this;
      for (int i = 0; i < prefix.Length; i++)
        if (node != null)
          node = node.GetChildNode(i, prefix);
        else
          break;

      return node;
    }

    private WordTrieNode GetChildNode(int index, string prefix)
    {
      char symbol = prefix[index];
      return _childNodes.ContainsKey(symbol)
          ? _childNodes[symbol]
          : null;
    }

    #endregion "GET"

    #region "DEL"

    public bool  Del(string value)
    {
      if (!this.IsRoot)
        return false;

      if (string.IsNullOrWhiteSpace(value))
        return false;

      WordTrieNode node = this.Get(value);
      if (node == null)
        return false;

      for (int i = value.Length; i-- > 0; )
        if (node != null)
          node = node.DelValue(i, value);

      return node != null;
    }

    private WordTrieNode DelValue(int index, string value)
    {
      bool isNodeValue = index == value.Length - 1;
      if (isNodeValue && !this.HasChildNodes)
        this._parent._childNodes.Remove(value[index]);
      else if (isNodeValue)
        this._word = null;

      this._mostPopularWords.Del(value);

      return this._parent;
    }

    #endregion "DEL"

    #region "MATCH"

    public IEnumerable<Word> Match(string prefix)
    {
      if (!this.IsRoot)
        return new List<Word>();

      if (string.IsNullOrWhiteSpace(prefix))
        return this.MostPopularWords;

      WordTrieNode node = this.Get(prefix);
      return
        node != null
          ? node.MostPopularWords
          : new List<Word>();
    }

    #endregion "MATCH"

    #region "UPDATE"

    private WordTrieNode UpdateValue(Word word)
    {
      if (this.HasValue)
      {
        _word.Count += word.Count;
      }
      else
      {
        _word = word;
      }
      return this;
    }

    private WordTrieNode UpdateMostPopularWords(Word word)
    {
      _mostPopularWords.Add(word);
      return this;
    }

    #endregion "UPDATE"
  }
}
