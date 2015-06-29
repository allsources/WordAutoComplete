using System.Net.Sockets;
using System.Text;
using WordAutoComplete.Enums;
using WordAutoComplete.WordCollections;
using WordAutoComplete.WordTrie;

namespace WordAutoComplete.Helpers
{
  /// <summary>
  /// Represents common helper methods.
  /// </summary>
  public static class CommonHelper
  {
    #region "Word Collection"

    /// <summary>
    /// Create an instance of <see cref="IWordCollection"/> object.
    /// </summary>
    /// <param name="matchByTrie">Defines a type of <see cref="IWordCollection"/> object that will used for words processing.</param>
    /// <param name="config"><see cref="WordNodeConfig"/></param>
    public static IWordCollection CreateWordsCollection(StandaloneMode mode, WordNodeConfig config)
    {
      switch(mode)
      {
        case StandaloneMode.Dictionary:
          return new WordDictionaryCollection(config.MostPopularWordsLimit);
        case StandaloneMode.HashSet:
          return new WordHashSetCollection(config.MostPopularWordsLimit);
        default:
          return new WordTrieCollection(config);
      }
    }

    #endregion "Word Collection"

    #region "Network Stream"

    public static string GetStreamData(NetworkStream stream)
    {
      var msg = new byte[1024];
      int byteCount;
      byteCount = stream.Read(msg, 0, msg.Length);
      if (byteCount == 0)
        return string.Empty;

      ASCIIEncoding encoder = new ASCIIEncoding();
      return
        encoder.GetString(msg, 0, byteCount);
    }

    public static void SendData(NetworkStream stream, string result)
    {
      ASCIIEncoding encoder = new ASCIIEncoding();
      byte[] buffer = encoder.GetBytes(result);
      stream.Write(buffer, 0, buffer.Length);
    }

    #endregion "Network Stream"
  }
}
