using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;

using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace GameClient.Network.Communicator
{
	/*
        Handles Communication with Game Server
    */
	public class Communicator
	{
		private Receiver receiver;
		private Configuration configuration;
		
		private Communicator()
		{
			
		}    
		
		//set to true after instalized has been called
		private bool instalized = false;
		
		//event raised when listner stops automatically or manually
		public event EventHandler MessageReceiverStopped;
		
		//event raised when message listener encouters an exception
		public event MessageReceiveErrorHandler MessageReceiveError;
		public delegate void MessageReceiveErrorHandler(object Sender, MessageReceiveErrorEventArgs args);
		
		//event raised when message listener receive a message
		public event MessageReceivedHandler MessageReceived;
		public delegate void MessageReceivedHandler(object Sender, MessageReceivedEventArgs args);
		
		private static Communicator mInstance = null;
		public static Communicator Instance
		{
			get
			{
				if (mInstance == null)
					mInstance = new Communicator();
				return mInstance;
			}
		}
		
		//is the message listner running
		public bool IsMessageReceiverRunning
		{
			get {
				if (!instalized)
					throw new Exception("Not Instalized");
				return receiver.IsListening; }
		}
		
		//Manually start receiving messages
		public void StartMessageReceiver()
		{
			if (!instalized)
				throw new Exception("Not Instalized");
			
			if (this.receiver.IsListening)
				return;
			this.receiver.StartListener();
		}
		
		//Manually stop receiving messages
		public void StopMessageReceiver()
		{
			if (!instalized)
				throw new Exception("Not Instalized");
			if (this.receiver.IsListening)
				this.receiver.StopListener();
		}
		
		//Call first before using other methods. Setup the configuration and start listener.
		public void Instalatize(Communicator.Configuration configuration)
		{
			this.configuration = configuration;
			this.receiver = new Receiver(configuration);
			this.receiver.StartListener();
			instalized = true;
		}
		
		//sends a message to server. May throw network related exceptions
		public void SendMessage(String message) 
		{
			if (!instalized)
				throw new Exception("Not Instalized");
			
			IPHostEntry ipHostInfo = Dns.GetHostEntry(configuration.ServerHost);
			IPAddress ipAddress = null;
			try
			{
				ipAddress = IPAddress.Parse(configuration.ServerHost);
				
			}
			catch(FormatException)
			{
				//server host is not an ip address. it could be host. resolve
				foreach (IPAddress add in ipHostInfo.AddressList)
				{
					if (add.AddressFamily == AddressFamily.InterNetwork)
					{
						ipAddress = add;
					}
					
				}
			}
			
			Socket sock = null;
			StreamWriter writer = null;
			try
			{
				//Setup Connection
				IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, configuration.ServerPort);
				sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				sock.Connect(remoteEndPoint);
				
				//Transmit
				Stream strm = new NetworkStream(sock);
				writer = new StreamWriter(strm);
				writer.AutoFlush = true;
				writer.Write(message);
				writer.Flush();
				
			}
			finally
			{
				//close
				NetworkUtils.CloseSafely(writer);
				NetworkUtils.CloseSafely(sock);
			}
			
		}
		
		//Event Arguements passed to MessageReceived event
		public class MessageReceivedEventArgs : EventArgs
		{
			public string Message { get; set; }
		}
		
		//Event Arguement passed to MessageReceiveErrorEvent
		public class MessageReceiveErrorEventArgs : EventArgs
		{
			public Exception Error { get; set; }
		}
		
		/*
            Inner class implementing the receiver thread. Contains information used by the receiver thread.
        */
		private class Receiver
		{
			//values used in progress changed events of background worker
			private const int PROGRESS_NEW_MESSAGE = 1;
			private const int PROGRESS_ERROR = 2;
			
			private BackgroundWorker backgroundWorker; //background thread used to listen
			private int port; //clients port
			
			//Is the listener running
			public bool IsListening
			{
				get
				{
					return backgroundWorker.IsBusy;
				}
			}
			
			
			public Receiver(Communicator.Configuration configuration)
			{
				this.port = configuration.ClientPort;           
			}
			
			//Should be called when restarting listener
			public void StartListener()
			{
				//background worker will be listening to incomming messages from server
				//backgroundWorker = new BackgroundWorker();
				//backgroundWorker.DoWork += BackgroundWorker_DoWork;
				//backgroundWorker.WorkerReportsProgress = true;
				//backgroundWorker.WorkerSupportsCancellation = true;
				//backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
				//backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
				//start listener thread
				//backgroundWorker.RunWorkerAsync(port); //clients port is passed through arguement

				int cPort = port;
				Loom.RunAsync (() => {
					BackgroundWorker_DoWork(cPort);
				});
			}
			
			private void BackgroundWorker_RunWorkerCompleted()
			{
				//For some reason, background worker has finished
				Debug.WriteLine("Listener has stopped");
				
				//call the message received event
				EventHandler handler = Communicator.Instance.MessageReceiverStopped;
				if (handler != null)
				{
					
					handler(Communicator.Instance,new EventArgs());
				}
			}
			
			private void BackgroundWorker_ProgressChanged(int percentage, Object userState)
			{
				//Progress changed is called whenever client receives a message
				//This method is called in the UI thread
				if(percentage ==  PROGRESS_NEW_MESSAGE ) //a message is there to be dispatched
				{
					//call the message received event
					MessageReceivedHandler handler = Communicator.Instance.MessageReceived;
					if (handler != null)
					{
						MessageReceivedEventArgs margs = new MessageReceivedEventArgs();
						margs.Message = (string)userState;
						handler(Communicator.Instance, margs);
					}
				}
				else if(percentage == PROGRESS_ERROR)//an exception is thrown
				{
					//call the message received event
					MessageReceiveErrorHandler handler = Communicator.Instance.MessageReceiveError;
					if (handler != null)
					{
						MessageReceiveErrorEventArgs margs = new MessageReceiveErrorEventArgs();
						margs.Error = (Exception)userState;
						handler(Communicator.Instance, margs);
					}
				}
				
				
			}
			
			private void BackgroundWorker_DoWork(int cPort)
			{
				//The method responsible for doing the actual listening task
				
				//BackgroundWorker worker = (BackgroundWorker)sender; //will be used to send received messages to ui thread
				int port = cPort; //port to listen is passed through arguement

				Boolean cancel = false;
				Socket listener = null;
				StreamReader reader = null;
				Socket handler = null;
				Stream networkStream = null;
				try
				{
					//Establishing Server Socket
					IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);
					
					listener = new Socket(localEndPoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
					listener.Bind(localEndPoint);
					listener.Listen(10); //Start listening to servers requests
					while (!cancel)
					{
						try
						{
							Debug.WriteLine("Waiting for connections...");
							handler = listener.Accept(); //Wait for a message from server
							
							networkStream = new NetworkStream(handler);
							reader = new StreamReader(networkStream);
							//Read message from server
							while (reader.Peek() != -1) //check whether input is available
							{
								String line = reader.ReadLine();
								
								Loom.QueueOnMainThread (() => {
									BackgroundWorker_ProgressChanged(PROGRESS_NEW_MESSAGE, line); //dispatch the message to UI thread
								});


							}
						}
						catch (SocketException ex)
						{
							//Report to Client
							Debug.WriteLine("Error: " + ex.Message);
							Loom.QueueOnMainThread(() => {
								BackgroundWorker_ProgressChanged(PROGRESS_ERROR, ex); //report error condition
							});

						}
						catch (IOException ex)
						{
							//Report to Client
							Debug.WriteLine("Error: " + ex.Message);
							Loom.QueueOnMainThread(() => {
								BackgroundWorker_ProgressChanged(PROGRESS_ERROR, ex); //report error condition
							});
						}
						finally
						{
							//close connections
							NetworkUtils.CloseSafely(reader);
							NetworkUtils.CloseSafely(networkStream);
							NetworkUtils.CloseSafely(handler);
							
						}
						
					}
					
					
				}
				catch (SocketException ex)
				{
					//Report to Client
					Debug.WriteLine("Error: " + ex.Message);
					Loom.QueueOnMainThread(() => {
						BackgroundWorker_ProgressChanged(PROGRESS_ERROR, ex); //report error condition
					});
				}
				catch(IOException ex)
				{
					//Report to Client
					Debug.WriteLine("Error: " + ex.Message);
					Loom.QueueOnMainThread(() => {
						BackgroundWorker_ProgressChanged(PROGRESS_ERROR, ex); //report error condition
					});
				}
				finally
				{
					//close connections 
					NetworkUtils.CloseSafely(listener);
				}


				Loom.QueueOnMainThread (() => {
					BackgroundWorker_RunWorkerCompleted();
				});
				
				
			}
			
			public void StopListener()
			{
				backgroundWorker.CancelAsync();
			}
			
		}
		
		/*
        Configurations are used to initialize a Communication Connection
        */
		public class Configuration
		{
			private int mServerPort;
			private int mClientPort;
			private string mServerHost;
			public Configuration(int ClientPort, string ServerHost, int ServerPort)
			{
				mClientPort = ClientPort;
				mServerHost = ServerHost;
				mServerPort = ServerPort;
			}
			
			/*
                Port used to connect to server
            */
			public int ServerPort
			{
				get
				{
					return mServerPort;
				}
				
			}
			/*
                Port to which server should connect to in client
            */
			public int ClientPort
			{
				get
				{
					return mClientPort;
				}
			}
			/*
            Host address of server
            */
			public string ServerHost
			{
				get
				{
					return mServerHost;
				}
			}
		}
	}
}
