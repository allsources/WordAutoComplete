using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordAutoComplete.Classes;
using WordAutoComplete.WordTrie;

namespace WordAutoCompleteTest
{
  [TestClass]
  public class MostPopularWordsTest
  {
    private static readonly WordNodeConfig _config = new WordNodeConfig { MostPopularWordsLimit = 4 };

    [TestMethod]
    public void WordsCountTest1()
    {
      var root = new WordTrieNode(_config);
      root.Add(new Word("aaaa", 111));
      root.Add(new Word("aabb", 222));
      root.Add(new Word("aacc", 333));
      WordTrieNode node = root.Get("a");
      Assert.AreEqual(node.MostPopularWords.Count(), 3);
    }

    [TestMethod]
    public void WordsCountTest2()
    {
      var root = new WordTrieNode(_config);
      root.Add(new Word("aaaa", 111));
      root.Add(new Word("aabb", 222));
      root.Add(new Word("aacc", 333));
      WordTrieNode node = root.Get("aab");
      Assert.AreEqual(node.MostPopularWords.Count(), 1);
    }

    [TestMethod]
    public void WordsLimitTest()
    {
      var root = new WordTrieNode(_config);
      root.Add(new Word("aaaa", 333));
      root.Add(new Word("aaba", 111));
      root.Add(new Word("aabc", 333));
      root.Add(new Word("aaca", 222));
      root.Add(new Word("aacb", 333));
      WordTrieNode node = root.Get("aa");
      Assert.AreEqual(node.MostPopularWords.Count(), _config.MostPopularWordsLimit);
    }

    [TestMethod]
    public void WordsOrberByCountTest()
    {
      var root = new WordTrieNode(_config);
      root.Add(new Word("aaaa", 222));
      root.Add(new Word("aabb", 111));
      root.Add(new Word("aabc", 444));
      root.Add(new Word("aaac", 111));
      root.Add(new Word("aacc", 555));
      WordTrieNode node = root.Get("aa");
      IList<Word> words = node.MostPopularWords.ToList();
      Assert.IsTrue(
          words[0].Value.Equals("aacc") &&
          words[1].Value.Equals("aabc") &&
          words[2].Value.Equals("aaaa") &&
          words[3].Value.Equals("aaac")
        );
    }

    [TestMethod]
    public void WordsOrberByAbcTest()
    {
      var root = new WordTrieNode(_config);
      root.Add(new Word("aaaa", 555));
      root.Add(new Word("aacc", 555));
      root.Add(new Word("aabb", 555));
      root.Add(new Word("aaca", 555));
      root.Add(new Word("aaba", 555));
      WordTrieNode node = root.Get("aa");
      IList<Word> words = node.MostPopularWords.ToList();
      Assert.IsTrue(
          words[0].Value.Equals("aaaa") &&
          words[1].Value.Equals("aaba") &&
          words[2].Value.Equals("aabb") &&
          words[3].Value.Equals("aaca")
        );
    }
  }
}
