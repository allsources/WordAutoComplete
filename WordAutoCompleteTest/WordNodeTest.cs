using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordAutoComplete.WordTrie;
using WordAutoComplete.Classes;
using System.Collections.Generic;

namespace WordAutoCompleteTest
{
  [TestClass]
  public class WordNodeTest
  {
    private static readonly WordNodeConfig _config = new WordNodeConfig { MostPopularWordsLimit = 4 };

    #region "CLEAR"

    [TestMethod]
    public void ClearTest()
    {
      var root = new WordTrieNode(_config);
      root.Add(new Word("abc", 111));
      root.Add(new Word("def", 111));
      root.Add(new Word("qwe", 111));
      root.Clear();
      Assert.IsFalse(root.HasChildNodes);
    }

    [TestMethod]
    public void ClearOnChildNodeTest()
    {
      var root = new WordTrieNode(_config);
      root.Add(new Word("abc", 111));
      root.Add(new Word("def", 111));
      root.Add(new Word("qwe", 111));
      var node = root.Get("qwe");
      node.Clear();
      Assert.IsTrue(root.HasChildNodes && node.HasValue);
    }

    #endregion "CLEAR"

    #region "BUILD"

    [TestMethod]
    public void BuildTest()
    {
      var words =
        new List<Word>
          {
            new Word("qwe", 111),
            new Word("asd", 222),
            new Word("zxc", 333),
          };
      var root = new WordTrieNode(_config);
      root.Build(words);
      Assert.IsTrue(root.HasChildNodes);
    }

    [TestMethod]
    public void BuildWithNullTest()
    {
      IEnumerable<Word> words = null;
      var root = new WordTrieNode(_config);
      root.Build(words);
      Assert.IsFalse(root.HasChildNodes);
    }

    [TestMethod]
    public void BuildAfterAddTest()
    {
      var root = new WordTrieNode(_config);
      root.Add(new Word("abc", 111));
      root.Add(new Word("def", 111));
      root.Add(new Word("qwe", 111));
      var words =
        new List<Word>
          {
            new Word("qwe", 111),
            new Word("asd", 222),
            new Word("zxc", 333),
          };
      root.Build(words);
      var node1 = root.Get("abc");
      var node2 = root.Get("qwe");
      Assert.IsTrue(node1 == null && node2 != null && node2.Value.Value == "qwe");
    }

    #endregion "BUILD"

    [TestMethod]
    public void AddNullTest()
    {
      var root = new WordTrieNode(_config);
      root.Add(null);
      Assert.IsFalse(root.HasChildNodes);
    }

    [TestMethod]
    public void AddWordWithNullValueTest()
    {
      var root = new WordTrieNode(_config);
      var word = new Word();
      root.Add(word);
      Assert.IsFalse(root.HasChildNodes);
    }

    [TestMethod]
    public void AddWordTest()
    {
      var root = new WordTrieNode(_config);
      root.Add(new Word("aaa", 111));
      Assert.IsTrue(root.HasChildNodes);
    }

    [TestMethod]
    public void ChildNodeNullTest1()
    {
      var root = new WordTrieNode(_config);
      root.Add(new Word("abc", 111));
      var node = root.Get("aaa");
      Assert.IsNull(node);
    }

    [TestMethod]
    public void ChildNodeNullTest2()
    {
      var root = new WordTrieNode(_config);
      root.Add(new Word("abc", 111));
      var node = root.Get(string.Empty);
      Assert.IsNull(node);
    }

    [TestMethod]
    public void ChildNodeNotNullTest1()
    {
      var root = new WordTrieNode(_config);
      root.Add(new Word("abc", 111));
      var node = root.Get("abc");
      Assert.IsNotNull(node);
    }

    [TestMethod]
    public void ChildNodeNotNullTest2()
    {
      var root = new WordTrieNode(_config);
      root.Add(new Word("abc", 111));
      var node = root.Get("ab");
      Assert.IsNotNull(node);
    }
  }
}
