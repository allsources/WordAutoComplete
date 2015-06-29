using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WordAutoComplete.Classes
{
  /*
    ToDo: Review Add operation performance.
    ToDo: Define a way how to rebuild _mostPopularWords collection after removal of the word.
  */

  /// <summary>
  /// Represents collection of most popular words.
  /// </summary>
  public class MostPopularWords
  {
    #region "Private members"

    private const int UNKNOWN_INDEX_TO_INSERT = -1;

    private int _mostPopularWordsLimit;

    private readonly List<Word> _mostPopularWords;

    #endregion "Private members"

    #region ".ctor"

    public MostPopularWords(int mostPopularWordsLimit)
    {
      _mostPopularWordsLimit = mostPopularWordsLimit;
      _mostPopularWords = new List<Word>();
    }

    #endregion ".ctor"

    #region "Public members"

    public IEnumerable<Word> Value { get { return new ReadOnlyCollection<Word>(_mostPopularWords); } }

    #endregion "Public members"

    #region "Public methods"

    public void Add(Word word)
    {
      if (IsNotMostPopularWord(word))
        return;

      for (int i = 0; i < _mostPopularWords.Count; i++)
        if (_mostPopularWords[i].Value.Equals(word.Value, System.StringComparison.OrdinalIgnoreCase))
        {
          _mostPopularWords.Remove(_mostPopularWords[i]);
          break;
        }

      if (TryInsertWord(word))
        ApplyMostPopularWordsLimit();
    }

    public void Del(string word)
    {
      Word mostPopularWord = _mostPopularWords.FirstOrDefault(w => w.Value.Equals(word, System.StringComparison.OrdinalIgnoreCase));
      if (mostPopularWord != null)
        _mostPopularWords.Remove(mostPopularWord);
    }

    #endregion "Public methods"

    #region "Private methods"

    private bool IsNotMostPopularWord(Word word)
    {
      return
        _mostPopularWords.Count == _mostPopularWordsLimit &&
        _mostPopularWords.All(w => w.Count > word.Count);
    }

    private bool TryInsertWord(Word word)
    {
      int indexToInsert = GetIndexToInsert(word);
      bool isMostPopularWord = indexToInsert != UNKNOWN_INDEX_TO_INSERT;
      if (isMostPopularWord)
        _mostPopularWords.Insert(indexToInsert, word);
      return
        isMostPopularWord;
    }

    private int GetIndexToInsert(Word word)
    {
      for (int index = 0; index < _mostPopularWords.Count; index++)
      {
        Word popularWord = _mostPopularWords[index];
        if (popularWord.Count > word.Count)
          continue;
        if (popularWord.Count == word.Count && popularWord.Value.CompareTo(word.Value) == -1)
          continue;
        return index;
      }
      if (_mostPopularWords.Count != _mostPopularWordsLimit)
        return _mostPopularWords.Count;
      return
        UNKNOWN_INDEX_TO_INSERT;
    }

    private void ApplyMostPopularWordsLimit()
    {
      if (_mostPopularWords.Count > _mostPopularWordsLimit)
        _mostPopularWords.RemoveAt(_mostPopularWordsLimit);
    }

    private bool IsExists(Word word)
    {
      Word existingWord = _mostPopularWords.FirstOrDefault(w => w.Value.Equals(word.Value, System.StringComparison.OrdinalIgnoreCase));
      return
        existingWord != null;
    }

    #endregion "Private methods"
  }
}
