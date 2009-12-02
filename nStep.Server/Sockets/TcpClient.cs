using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace nStep.Server.Sockets
{
	public interface ITcpClient
	{
		Stream GetStream();
		void Close();
	}

	public class TcpClient : ITcpClient
	{
		private readonly System.Net.Sockets.TcpClient client;

		public TcpClient(System.Net.Sockets.TcpClient client)
		{
			this.client = client;
		}

		public Stream GetStream()
		{
			return client.GetStream();
		}

		public void Close()
		{
			client.Close();
		}
	}
}