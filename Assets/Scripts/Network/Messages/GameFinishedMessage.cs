using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameClient.GameDomain;

namespace GameClient.Network.Messages
{
    /*
    A server originated message informing that the Game has finished
    */
    class GameFinishedMessage : ServerMessage
    {
        //Update the GameWorld
        public override void Execute()
        {
            GameWorld.Instance.State = GameWorld.GameWorldState.Finished;
            
        }

        public override string ToString()
        {
            return "Game finished!!!";
        }

        //The parser to detect and parse Game Finished Messages
        public class GameFinishedMessageParser : ServerMessageParser
        {
            private GameFinishedMessageParser()
            {

            }
            private static GameFinishedMessageParser instance = null;
            public static GameFinishedMessageParser Instance
            {
                get
                {
                    if (instance == null)
                        instance = new GameFinishedMessageParser();
                    return instance;
                }
            }
            //If success, return parsed object. Otherwise returns null
            public override ServerMessage TryParse(string[] sections)
            {
                if(sections[0].Trim().ToUpper() == "GAME_FINISHED")
                {
                    return new GameFinishedMessage();
                }
                return null;
            }
        }
    }
}
