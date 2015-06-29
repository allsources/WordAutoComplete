using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WordAutoComplete.Classes;

namespace WordAutoComplete.WordCollections
{
  public interface IWordCollection
  {
    void Build(IEnumerable<Word> words);

    void Add(Word word);

    bool Del(string value);

    Word Get(string value);

    IEnumerable<Word> Match(string value);

    bool HasData();
  }
}
