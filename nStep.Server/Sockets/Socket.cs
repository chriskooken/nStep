using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace nStep.Server.Sockets
{
	public interface ISocket
	{
		EndPoint LocalEndPoint { get; }
		EndPoint RemoteEndPoint { get; }

		IAsyncResult BeginReceive(byte[] buffer, int offset, int length, SocketFlags socketFlags, AsyncCallback callback, object state);
		int EndReceive(IAsyncResult asyncResult);
		int Send(byte[] buffer);

		void Shutdown(SocketShutdown how);
		void Close();
	}

	public class Socket : ISocket
	{
		private readonly System.Net.Sockets.Socket socket;

		public Socket(System.Net.Sockets.Socket socket)
		{
			this.socket = socket;
		}

		public EndPoint LocalEndPoint
		{
			get { return socket.LocalEndPoint; }
		}

		public EndPoint RemoteEndPoint
		{
			get { throw new NotImplementedException(); }
		}

		public IAsyncResult BeginReceive(byte[] buffer, int offset, int length, SocketFlags socketFlags, AsyncCallback callback, object state)
		{
			throw new NotImplementedException();
		}

		public int EndReceive(IAsyncResult asyncResult)
		{
			throw new NotImplementedException();
		}

		public int Send(byte[] buffer)
		{
			throw new NotImplementedException();
		}

		public void Shutdown(SocketShutdown how)
		{
			throw new NotImplementedException();
		}

		public void Close()
		{
			throw new NotImplementedException();
		}
	}
}
