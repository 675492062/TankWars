using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.GameDomain;
using GameClient.Foundation;

namespace GameClient.Network.Messages
{
    /*
    A Server originated message containing periodic update information
    */
    public class GlobalUpdateMessage : ServerMessage
    {
        //The specific details embedded to the message
        public GlobalUpdate globalUpdate { get; set; }

        //Apply the message's effect to GameWorld
        public override void Execute()
        {
            GameWorld.Instance.State = GameWorld.GameWorldState.Running;
            GameWorld.Instance.BrickState = globalUpdate.brickUpdate;
            GameWorld.Instance.Players = globalUpdate.PlayerUpdates;
            GameWorld.Instance.AdvanceFrame();
            
        }

        //Obtain String representation
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(" ");
            builder.AppendLine("Global update Message");
            builder.AppendLine(globalUpdate.ToString());
            builder.AppendLine("------------------------\n");
            return builder.ToString();
        }

        //The class holding particular details of a GlobalUpdate message
        public class GlobalUpdate
        {
            //The details of players
            public PlayerDetails[] PlayerUpdates { get; set; }

            //The updated details of bricks
            public Brick[] brickUpdate { get; set; }

            //Obtain textual represention of GlobalUpdate
            public override string ToString()
            {
                StringBuilder builder = new StringBuilder();
                foreach (PlayerDetails player in PlayerUpdates)
                {
                    builder.AppendLine(" ");
                    builder.AppendLine(player.ToString());
                }

                foreach (Brick brick in brickUpdate)
                {
                    builder.AppendLine(brick.Postition.ToString() + "\tDamage level: " + brick.DamageLevel);
                }
                return builder.ToString();
            }
        }

        /*
        The parser used to parse GlobalUpdate Messages. Returns null if the message is not a valid GlobalUpdateMessage
        */
        public class GlobalUpdateMessageParser : ServerMessage.ServerMessageParser
        {
            private static GlobalUpdateMessageParser instance = null;


            public static GlobalUpdateMessageParser Instance
            {
                get
                {
                    if (instance == null)
                        instance = new GlobalUpdateMessageParser();
                    return instance;
                }
            }

            public override ServerMessage TryParse(string[] sections)
            {
                if (sections[0].ToLower() == "g")
                {
                    /*
                    G:P1;< player location  x>,< player location  y>;<Direction>;< whether shot>;<health>;< coins>;< points>: …. :
                    P5;< player location  x>,< player location  y>;<Direction>;< whether shot>;<health>;< coins>;< points>:
                    < x>,<y>,<damage-level>;< x>,<y>,<damage-level>…..< x>,<y>,<damage-level># 
                    */

                    GlobalUpdate globalUpdate = new GlobalUpdate();
                    //Load player details
                    PlayerDetails[] playerUpdate = new PlayerDetails[sections.Length-2];
                    for(int i = 1; i < sections.Length - 1; i++)
                    {
                        string[] parameters = Tokenizer.TokernizeParameters(sections[i]);
                        PlayerDetails player = new PlayerDetails();
                        player.Name = parameters[0];
                        player.Position = Tokenizer.TokernizeCoordinates(parameters[1]);
                        player.Direction = (Direction)Convert.ToInt32(parameters[2]);
                        int shot = Convert.ToInt32(parameters[3]);
                        player.IsShooting = shot == 1;
                        player.Health = Convert.ToInt32(parameters[4]);
                        player.Coins = Convert.ToInt32(parameters[5]);
                        player.Points = Convert.ToInt32(parameters[6]);
                        playerUpdate[i-1] = player;
                    }

                    //Load bricks updates
                    string[] paras = Tokenizer.TokernizeParameters(sections[sections.Length-1]);
                    Brick[] brickUpdate = new Brick[paras.Length];
                    for(int i = 0; i < paras.Length; i++)
                    {
                        Brick brick = new Brick();
                        int[] brickDamage= Tokenizer.TokernizeIntArray(paras[i]);
                        brick.Postition = new Coordinate(brickDamage[0], brickDamage[1]);
                        brick.DamageLevel = brickDamage[2];
                        brickUpdate[i] = brick;
                    }

                    //Formulate result

                    globalUpdate.PlayerUpdates = playerUpdate;
                    globalUpdate.brickUpdate = brickUpdate;
                    GlobalUpdateMessage result = new GlobalUpdateMessage();
                    result.globalUpdate = globalUpdate;
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
