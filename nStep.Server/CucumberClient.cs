using System;
using System.Net.Sockets;
using System.Text;
using nStep.Server.Sockets;
using Socket=nStep.Server.Sockets.Socket;

namespace nStep.Server
{
	public class CucumberClient
	{
		private const int BufferSize = 1500;

		// A read buffer for this Socket
		private readonly byte[] ReadBuffer = new byte[BufferSize];

		// The current offset into the read buffer for new data
		private int ReadOffset;

		private readonly IProcessor processor;

		public CucumberClient(ISocket socket, IProcessor processor)
		{
			this.processor = processor;
			//Console.WriteLine("Accepting new connection from {0}", socket.RemoteEndPoint.ToString());
			//Console.WriteLine("\tto {0}", socket.LocalEndPoint.ToString());

			// Start waiting for data
			socket.BeginReceive(ReadBuffer, ReadOffset, ReadBuffer.Length - ReadOffset, SocketFlags.None, DoReceive, socket);
		}

		public void DoReceive(IAsyncResult ar)
		{
			// Get the socket
			var s = (ISocket)ar.AsyncState;

			// How much data did I actually get
			int len; 
			try
			{
				len = s.EndReceive(ar);
			}
			catch (SocketException e)
			{
				// Socket is probably already closed, this should probably be a bit better
				len = 0;
			}

			// If no data, then the socket has been disconnected
			if (len == 0)
			{
				//Console.WriteLine("Disconnecting {0}", s.RemoteEndPoint.ToString());
				// Make sure to shut down
				s.Shutdown(SocketShutdown.Both);
				// and close the socket
				s.Close();
				return;
			}

			// increment the ReadOffset
			ReadOffset += len;

			// Is there a new line?
			var i = Array.IndexOf<byte>(ReadBuffer, 0x0A);

			if (i != -1)
				ReadOffset -= (i + 1); // move the ReadOffset back

			// Start listening again
			s.BeginReceive(ReadBuffer, ReadOffset, ReadBuffer.Length - ReadOffset, SocketFlags.None, DoReceive, s);

			// If there is a new line
			if(i != -1) {
				// Get the string out of the buffer
				var json = Encoding.ASCII.GetString(ReadBuffer, 0, i);
				
				// Shift the buffer back, really this should be a ring buffer
				Array.Copy(ReadBuffer, i + 1, ReadBuffer, 0, ReadBuffer.Length - i - 1);
				
				// Parse the request
				var request = Messages.Request.ParseFromJson(json);
				var response = processor.Process(request);
			}
		}
	}
}