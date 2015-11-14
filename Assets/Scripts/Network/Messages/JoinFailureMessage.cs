using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClient.Network.Messages
{
    /*
        A join failure message is broadcasted in response to a join request, when the request fails
    */
    class JoinFailureMessage : ServerMessage
    {
        private MessageType messageType;
        /*
            The exact failure message
        */
        public MessageType Type
        {
            get
            {
                return messageType;
            }
            set
            {
                messageType = value;
            }
        }

        public JoinFailureMessage(MessageType type)
        {
            this.messageType = type;
        }

        //TODO: Report to GUI stating connection failed
        public override void Execute()
        {

        }

        //Textual Representation
        public override string ToString()
        {
            switch(messageType)
            {
                case MessageType.AlreadyAdded:
                    return "Join Failure: Player is already added";
                case MessageType.GameAlreadyStarted:
                    return "Join Failure: Game already started";
                case MessageType.PlayersFull:
                    return "Join Failure: Maximum number of players reached";

            }
            return "Join Failure: ";
        }

        //The parser to detect and parse JoinFailure messages
        public class JoinFailureMessageParser : ServerMessageParser
        {
            private JoinFailureMessageParser()
            {

            }
            private static JoinFailureMessageParser instance = null;
            public static JoinFailureMessageParser Instance
            {
                get
                {
                    if (instance == null)
                        instance = new JoinFailureMessageParser();
                    return instance;
                }
            }

            
            public override ServerMessage TryParse(string[] sections)
            {
                if(sections[0].ToUpper().Trim() == "PLAYERS_FULL")
                {
                    return new JoinFailureMessage(MessageType.PlayersFull);
                }
                else if(sections[0].ToUpper().Trim() == "ALREADY_ADDED")
                {
                    return new JoinFailureMessage(MessageType.AlreadyAdded);
                }
                else if (sections[0].ToUpper().Trim() == "GAME_ALREADY_STARTED")
                {
                    return new JoinFailureMessage(MessageType.GameAlreadyStarted);
                }
                return null;

            }
        }

        /*
            Set of possible failure messages
        */
        public enum MessageType
        {
            PlayersFull,
            GameAlreadyStarted,
            AlreadyAdded
        }
    }


}
