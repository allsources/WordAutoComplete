using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordAutoComplete.Classes
{
  /// <summary>
  /// Represents a word object.
  /// </summary>
  public class Word
  {
    #region "Public members"

    /// <summary>
    /// A word.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// A number that defines how many times the word occurs in the text.
    /// </summary>
    public int Count { get; set; }

    #endregion "Public members"

    #region ".ctor"

    public Word()
    {
    }

    public Word(string word, int count)
    {
      Value = word;
      Count = count;
    }

    #endregion ".ctor"
  }
}
