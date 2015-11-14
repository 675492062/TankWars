using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.GameDomain;
using GameClient.Foundation;

namespace GameClient.Network.Messages
{
    /*
    Message sent from server to client, accepting the clients join request
    */
     public class JoinAcceptanceMessage : ServerMessage
    {
        /*
        The locations of players
        */
        private PlayerDetails[] playerDetails;
        public PlayerDetails[] PlayerDetails
        {
            get
            {
                return playerDetails;
            }
            set
            {
                playerDetails = value;
            }
        }

        //Apply the message to GameWorld
        public override void Execute()
        {
            GameWorld.Instance.Players = playerDetails;
        }

        //Obtain textual representation
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Join Acceptance Message");
            foreach(PlayerDetails p in playerDetails)
            {
                builder.AppendLine(p.ToString());
            }
            return builder.ToString();
        }

        /*
            The parser to detect and parse JoinAcceptanceMessage
        */

        public class JoinAcceptanceMessageParser : ServerMessage.ServerMessageParser
        {
            private JoinAcceptanceMessageParser()
            {

            }
            private static JoinAcceptanceMessageParser instance = null;
            public static JoinAcceptanceMessageParser Instance
            {
                get
                {
                    if (instance == null)
                        instance = new JoinAcceptanceMessageParser();
                    return instance;
                }
            }
            public override ServerMessage TryParse(string[] sections)
            {
                if(sections[0].ToLower() == "s")
                {
                    //S:P0;0,0;0#
                    PlayerDetails[] players = new PlayerDetails[sections.Length - 1];
                    //identify individual player details received
                    for(int i = 1; i < sections.Length; i++)
                    {
                        string section = sections[i];
                        string[] parameters = Tokenizer.TokernizeParameters(section);
                        PlayerDetails player = new PlayerDetails();
                        player.Name = parameters[0];
                        player.Position = Tokenizer.TokernizeCoordinates(parameters[1]);
                        int direction = Convert.ToInt32(parameters[2]);
                        player.Direction = (Direction)direction;
                        players[i - 1] = player;
                    }

                    JoinAcceptanceMessage result = new JoinAcceptanceMessage();
                    result.PlayerDetails = players;
                    return result;
                }
                else
                {
                    //Not a valid join acceptance message
                    return null;
                }
            }
        }

       

    }
}
