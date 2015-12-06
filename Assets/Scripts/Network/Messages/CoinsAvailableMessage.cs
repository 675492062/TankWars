using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameClient.Foundation;
using GameClient.GameDomain;


namespace GameClient.Network.Messages
{
    /*
    Message sent from server to client, informing availavility of a new Coin
    */
    public class CoinsAvailableMessage : ServerMessage
    {
        // Positon, time of availabilit and value of Coin
        public Coin coin { get; set; }

        // Update GameWorld
        public override void Execute()
        {
            
            GameWorld.Instance.Coins.Add(coin);
			GameWorld.Instance.NotifyCoinPackAdded (coin);

        }

        // Textual represnatation of variables for logging purpose
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(" ");
            builder.AppendLine("Coin Available Message");
            builder.AppendLine(coin.ToString());
            builder.AppendLine(" ");
            return builder.ToString();
        }

        /*
        The Praser to parse CoinAvailableMessage
        */
        public class CoinAvailbleMessageParser : ServerMessage.ServerMessageParser
        {
            private static CoinAvailbleMessageParser instance = null;

            public static CoinAvailbleMessageParser Instance
            {
                get
                {
                    if (instance == null)
                        instance = new CoinAvailbleMessageParser();
                    return instance;
                }
            }

            public override ServerMessage TryParse(string[] sections)
            {
                if (sections[0].ToLower() == "c")
                {
                    //C:<x>,<y>:<LT>:<Val>#
                    Coin coin = new Coin();
                    coin.Position = Tokenizer.TokernizeCoordinates(sections[1]);
                    coin.TimeLimit = Convert.ToInt32(sections[2]);
                    coin.Value = Convert.ToInt32(sections[3]);

                    CoinsAvailableMessage result = new CoinsAvailableMessage();
                    result.coin = coin;
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
