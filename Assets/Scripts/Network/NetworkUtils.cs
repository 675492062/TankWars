using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;


namespace GameClient.Network
{
	//Support tool to safely close connections
	//Avoids possible null pointer exceptions due to uninitialized variables
	//Avoids already closed type exceptions
	class NetworkUtils
	{
		private NetworkUtils()
		{
			
		}
		//Safely close a socket
		public static void CloseSafely(Socket socket)
		{
			if(socket != null)
			{
				try
				{                    
					socket.Close();
				}
				catch(IOException ex)
				{
					Debug.WriteLine("Close Safely Failed: " + ex.Message);
				}
				catch (SocketException ex)
				{
					Debug.WriteLine("Close Safely Failed: " + ex.Message);
				}
				
			}
		}
		
		//Safely close a stream
		public static void CloseSafely(Stream stream)
		{
			if (stream != null)
			{
				try
				{
					stream.Close();
				}
				catch (IOException ex)
				{
					Debug.WriteLine("Close Safely Failed: " + ex.Message);
				}
				catch (SocketException ex)
				{
					Debug.WriteLine("Close Safely Failed: " + ex.Message);
				}
				
			}
		}
		
		//Safely close a StreamReader
		public static void CloseSafely(StreamReader r)
		{
			if (r != null)
			{
				try
				{
					r.Close();
				}
				catch (IOException ex)
				{
					Debug.WriteLine("Close Safely Failed: " + ex.Message);
				}
				catch (SocketException ex)
				{
					Debug.WriteLine("Close Safely Failed: " + ex.Message);
				}
				
			}
		}
		
		//Safely close a StreamWritter
		public static void CloseSafely(StreamWriter w)
		{
			if (w != null)
			{
				try
				{
					w.Close();
				}
				catch (IOException ex)
				{
					Debug.WriteLine("Close Safely Failed: " + ex.Message);
				}
				catch (SocketException ex)
				{
					Debug.WriteLine("Close Safely Failed: " + ex.Message);
				}
				
			}
		}
	}
}
