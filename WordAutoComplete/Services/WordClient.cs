using System;
using System.Net.Sockets;
using System.Threading;
using WordAutoComplete.Helpers;
using WordAutoComplete.Loggers;

namespace WordAutoComplete.Services
{
  /// <summary>
  /// Represents a client that sends a prefix to server and receives from it a collection with popular words.
  /// </summary>
  public class WordClient
  {
    #region "PRIVATE MEMBERS"

    private readonly string _host;

    private readonly int _port;

    private TcpClient _client;

    #endregion "PRIVATE MEMBERS"

    #region ".ctor"

    public WordClient(string host, int port)
    {
      _host = host;
      _port = port;
    }

    #endregion ".ctor"

    #region "PUBLIC METHODS"

    public string PerformServerRequest(string prefix)
    {
      Connect();

      string result =
        _client.Connected
          ? ProcessRequest(prefix)
          : string.Empty;

      Disconnect();

      return result;
    }

    #endregion "PUBLIC METHODS"

    #region "PRIVATE METHODS"

    private void Connect()
    {
      try
      {
        _client = new TcpClient(_host, _port);
      }
      catch(Exception ex)
      {
        Logger.LogException(ex);
      }
    }

      private void Disconnect()
      {
        if (_client.Connected)
          _client.Close();
      }

    private string ProcessRequest(string prefix)
    {
      SendRequest(prefix);
      return ReceiveData();
    }

    private void SendRequest(string prefix)
    {
      try
      {
        NetworkStream stream = _client.GetStream();
        CommonHelper.SendData(stream, prefix);
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
      }
    }

    private string ReceiveData()
    { 
      NetworkStream stream = _client.GetStream();
      int counter = 0;
      while (counter < 3)
      {
        counter++;
        if (stream.DataAvailable)
          return CommonHelper.GetStreamData(stream);
        Thread.Sleep(500);
      };
      return string.Empty;
    }

    #endregion "PRIVATE METHODS"
  }
}
