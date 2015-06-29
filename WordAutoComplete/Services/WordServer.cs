using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using WordAutoComplete.Helpers;
using WordAutoComplete.Loggers;

namespace WordAutoComplete.Services
{
  /// <summary>
  /// Represents a server that receives prefix from a client and then return popular words.
  /// </summary>
  public class WordServer
  {
    #region "PRIVATE MEMBERS"

    private readonly TcpListener _listener;

    private Func<string, string> _processRequestData;

    private AutoResetEvent _processEvent = new AutoResetEvent(false);

    #endregion "PRIVATE MEMBERS"

    #region ".ctor"

    public WordServer(int port, Func<string, string> processRequestData)
    {
      _listener = new TcpListener(IPAddress.Any, port);
      _processRequestData = processRequestData;
    }

    #endregion ".ctor"

    #region "PUBLIC METHODS"

    public void Start()
    {
      _listener.Start();
      while (true)
      {
        _listener.BeginAcceptTcpClient(new AsyncCallback(PerformAcceptTcpClient), _listener);
        _processEvent.WaitOne();
      }
    }

    #endregion "PUBLIC METHODS"

    #region "PRIVATE HELPER METHODS"

    private void PerformAcceptTcpClient(IAsyncResult args)
    {
      var listener = (TcpListener)args.AsyncState;
      if (listener == null)
        return;

      TcpClient client = listener.EndAcceptTcpClient(args);

      _processEvent.Set();

      try
      {
        ProcessClientRequest(client);
      }
      catch(Exception ex)
      {
        Logger.LogException(ex);
      }
      finally
      {
        client.Close();
      }
    }

    private void ProcessClientRequest(TcpClient client)
    {
      NetworkStream stream = client.GetStream();

      string data = CommonHelper.GetStreamData(stream);

      Console.WriteLine("Process: {0}", data);

      string result = _processRequestData(data);

      CommonHelper.SendData(stream, result);

      stream.Close();
    }

    #endregion "PRIVATE HELPER METHODS"
  }
}
