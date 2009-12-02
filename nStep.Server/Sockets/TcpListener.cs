using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nStep.Server.Sockets
{
	public interface ITcpListener
	{
		bool Pending();
		ITcpClient AcceptTcpClient();
		void Start();
		void Stop();
	}

	public class TcpListener : ITcpListener
	{
		private readonly System.Net.Sockets.TcpListener listener;

		public TcpListener(System.Net.Sockets.TcpListener listener)
		{
			this.listener = listener;
		}

		public bool Pending()
		{
			return listener.Pending();
		}

		public ITcpClient AcceptTcpClient()
		{
			return new TcpClient(listener.AcceptTcpClient());
		}

		public void Start()
		{
			listener.Start();
		}

		public void Stop()
		{
			listener.Stop();
		}
	}
}