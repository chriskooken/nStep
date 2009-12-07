using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace nStep.Server.Sockets
{
	public interface ITcpListener
	{
		bool Pending();
		void Start();
		void Stop();

		IAsyncResult BeginAcceptSocket(AsyncCallback callback, object state);
		Socket EndAcceptSocket(IAsyncResult asyncResult);
	}

	public class TcpListener : ITcpListener
	{
		private readonly System.Net.Sockets.TcpListener listener;

		public TcpListener(System.Net.Sockets.TcpListener listener)
		{
			this.listener = listener;
		}

		public bool Pending() { return listener.Pending(); }
		public void Start() { listener.Start(); }
		public void Stop() { listener.Stop(); }

		public IAsyncResult BeginAcceptSocket(AsyncCallback callback, object state)
		{
			return listener.BeginAcceptSocket(callback, state);
		}

		public Socket EndAcceptSocket(IAsyncResult asyncResult)
		{
			return new Socket(listener.EndAcceptSocket(asyncResult));
		}
	}
}