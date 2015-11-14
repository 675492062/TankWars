using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GameClient.Network
{
	/*
    The parser used to parse messages received from server.
    Relies on Chain Of Responsibility Design pattern: 
        Individual parsers that could parse a specific type of message are linked up in a chain
        The string to be parsed are sent to each individual parser through the chain
    
    */
	public class MessageParser
	{
		//The root node of chain of responsibility
		private ParserNode rootNode;
		private static MessageParser mInstance = null;
		public static MessageParser Instance
		{
			get
			{
				if(mInstance == null)
				{
					mInstance = new MessageParser();
				}
				return mInstance;
			}
		}
		
		private MessageParser()
		{
			//setup chain of responsibility here
			rootNode = null;
			rootNode = new ParserNode(Messages.JoinAcceptanceMessage.JoinAcceptanceMessageParser.Instance);
			rootNode.NextNode = new ParserNode(Messages.JoinFailureMessage.JoinFailureMessageParser.Instance);
			rootNode.NextNode.NextNode = new ParserNode(Messages.NegativeHonourMessage.NegativeHonourMessageParser.Instance);
			rootNode.NextNode.NextNode.NextNode = new ParserNode(Messages.GameInitiationMessage.GameInitiationMessageParser.Instance);
			rootNode.NextNode.NextNode.NextNode.NextNode = new ParserNode(Messages.GlobalUpdateMessage.GlobalUpdateMessageParser.Instance);
			rootNode.NextNode.NextNode.NextNode.NextNode.NextNode = new ParserNode(Messages.CoinsAvailableMessage.CoinAvailbleMessageParser.Instance);
			rootNode.NextNode.NextNode.NextNode.NextNode.NextNode.NextNode = new ParserNode(Messages.LifePackAvailableMessage.LifePackAvailbleMessageParser.Instance);
			rootNode.NextNode.NextNode.NextNode.NextNode.NextNode.NextNode.NextNode = new ParserNode(Messages.GameFinishedMessage.GameFinishedMessageParser.Instance);
		}
		
		//Check whether the termination delimeter of message has been received
		public static bool ValidateMessageFooter(String message)
		{
			String shortMessage = message.Trim();
			if(shortMessage.EndsWith("#"))
			{
				return true;
			}
			return false;
		}
		
		/*
        Returns parsed message. If parser fails, return null. 
        Utilizes Chain of Responsibility as described at class description comments
        */
		public Messages.ServerMessage Parse(String message)
		{
			if (rootNode == null)
				return null;
			
			/*
            Validate message format
            */
			if(ValidateMessageFooter(message))
			{
				//Message is of appropriate format. Pass the message to root of chain of responsibility
				string[] sections = Tokenizer.TokenizeSections(message);
				return rootNode.Parse(sections);
			}
			else
			{
				/*
                Parser fails. Message footer is missing
                */
				return null;
			}
			
			
		}
		
		/*
        Message Parser uses a chain of responsibility of specific parsers that could parse specific types of messages. 
        This chain of responsibility is formed using ParserNode objects, containing reference to specific parser and next parserNode.
        */
		private class ParserNode
		{
			//A specific parser (aka individual parser)
			private Messages.ServerMessage.ServerMessageParser parser;
			//link to next node in chain
			private ParserNode nextNode = null;
			
			public ParserNode(Messages.ServerMessage.ServerMessageParser parser)
			{
				this.parser = parser;
			}
			//The specific parser (aka individual parser) referenced by this node
			public Messages.ServerMessage.ServerMessageParser Parser
			{
				get { return parser; }
				set { parser = value; }
			}
			
			//link to next node in chain
			public ParserNode NextNode
			{
				get { return nextNode; }
				set { nextNode = value; }
			}
			
			
			public Messages.ServerMessage Parse(string[] sections)
			{
				/*
                   if this parsenode can parse, then parse and return result. Otherwise, pass to next node in chain.
                   If end of chain, return null.
                 */
				if (this.parser == null)
					return null;
				//Try to parse
				Messages.ServerMessage result = this.parser.TryParse(sections);
				if(result == null)
				{
					//Parser failed. Pass to next node
					if (this.nextNode == null)
						return null;
					return this.nextNode.Parse(sections);
				}
				return result;
			}
			
		}
	}
}
