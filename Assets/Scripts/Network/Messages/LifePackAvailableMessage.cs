using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.GameDomain;
using GameClient.Foundation;

namespace GameClient.Network.Messages
{
    /*
   Message sent from server to client, informing availavility of a new LifePack
   */
    public class LifePackAvailableMessage : ServerMessage
    {
        // Positon, time of availabilit and value of LifePack
        public LifePack lifePack { get; set; }

        // Update GameWorld
        public override void Execute()
        {
            GameWorld.Instance.LifePacks.Add(lifePack);
			GameWorld.Instance.NotifyLifePackAdded (lifePack);
        }

        // Textual represnatation of variables for logging purpose
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(" ");
            builder.AppendLine("Lifepack Available Message");
            builder.AppendLine(lifePack.ToString());
            builder.AppendLine(" ");
            return builder.ToString();
        }

        /*
        The Praser to parse LifePackAvailableMessage
        */
        public class LifePackAvailbleMessageParser : ServerMessage.ServerMessageParser
        {
            private static LifePackAvailbleMessageParser instance = null;

            public static LifePackAvailbleMessageParser Instance
            {
                get
                {
                    if (instance == null)
                        instance = new LifePackAvailbleMessageParser();
                    return instance;
                }
            }

            public override ServerMessage TryParse(string[] sections)
            {
                if (sections[0].ToLower() == "l")
                {
                    //C:<x>,<y>:<LT>:<Val>#
                    LifePack lifePack = new LifePack();
                    lifePack.Position = Tokenizer.TokernizeCoordinates(sections[1]);
                    lifePack.TimeLimit = Convert.ToInt32(sections[2]);

                    LifePackAvailableMessage result = new LifePackAvailableMessage();
                    result.lifePack = lifePack;
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
