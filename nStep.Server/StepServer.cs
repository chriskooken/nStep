using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace nStep.Server
{
	public class StepServer : TcpListener
	{
		private ManualResetEvent serverActive = new ManualResetEvent(false);
		
		public StepServer(int port) : base(port)
		{
			Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
			this.Start();
			this.BeginAcceptSocket(DoAccept, this);
			serverActive.WaitOne();
		}

		void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
		{
			this.Stop();
			serverActive.Set();
		}

		private void DoAccept(IAsyncResult ar)
		{
			var l = (TcpListener)ar.AsyncState;
			new CucumberClient(l.EndAcceptSocket(ar));
			l.BeginAcceptSocket(DoAccept, l);
		}
	}

	public class CucumberClient
	{
		private byte[] read_buffer = new byte[1500];
		private int read_offset = 0;

		public CucumberClient(Socket socket)
		{
			Console.WriteLine("Accepting new connection from {0}", socket.RemoteEndPoint.ToString());
			Console.WriteLine("\tto {0}", socket.LocalEndPoint.ToString());
			socket.BeginReceive(read_buffer, read_offset, read_buffer.Length - read_offset, SocketFlags.None, DoReceive, socket);
		}

		private void DoReceive(IAsyncResult ar)
		{
			var s = (Socket)ar.AsyncState;
			int len; 
			try
			{
				len = s.EndReceive(ar);
			}
			catch (SocketException e)
			{
				len = 0;
			}

			if (len == 0)
			{
				Console.WriteLine("Disconnecting {0}", s.RemoteEndPoint.ToString());
				s.Shutdown(SocketShutdown.Both);
				s.Close();
				return;
			}
			read_offset += len;

			//Console.WriteLine(BitConverter.ToString(read_buffer, 0, read_offset));
			var i = Array.IndexOf<byte>(read_buffer, 0x0A);
			if (i != -1)
				read_offset -= (i + 1);

			s.BeginReceive(read_buffer, read_offset, read_buffer.Length - read_offset, SocketFlags.None, DoReceive, s);

			if(i != -1) {
				var json = Encoding.ASCII.GetString(read_buffer, 0, i);
				Array.Copy(read_buffer, i + 1, read_buffer, 0, read_buffer.Length - i - 1);
				
				var r = Messages.Request.ParseFromJson(json);
				if ((r is Messages.BeginScenarioRequest) || (r is Messages.EndScenarioRequest))
					s.Send(Encoding.ASCII.GetBytes("[\"success\", null]\n"));
				else if (r is Messages.StepMatchesRequest)
					s.Send(Encoding.ASCII.GetBytes("[\"step_matches\",[]]\n"));
			}

		}

	}
}
