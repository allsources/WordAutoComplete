using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordAutoComplete.WordTrie
{
  /// <summary>
  /// Represents a config of a <see cref="WordNode"/> object.
  /// </summary>
  public class WordNodeConfig
  {
    #region "Public members"

    public int MostPopularWordsLimit { get; set; }

    #endregion "Public members"
  } 
}
