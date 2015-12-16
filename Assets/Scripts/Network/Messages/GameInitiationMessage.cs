using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.GameDomain;
using GameClient.Foundation;

namespace GameClient.Network.Messages
{
    /*
    Server Originated Message indicating that Game has begun. Contains information on Map.
    */
    public class GameInitiationMessage : ServerMessage
    {
        //The details of map sent by the message
        public MapDetails mapDetails { get; set; }

        //Apply the message to GameWorld
        public override void Execute()
        {
            GameWorld.Instance.Map = mapDetails;
            //The actual game begins when the first global update is received
            GameWorld.Instance.State = GameWorld.GameWorldState.Ready;

        }

        //Obtain textual representation
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(" ");
            builder.AppendLine("Game Initiation Message");
            builder.AppendLine(mapDetails.ToString());
            builder.AppendLine(" ");
            return builder.ToString();
        }

        //The parser to detect and initiate GameInitiation message
        public class GameInitiationMessageParser : ServerMessage.ServerMessageParser
        {
            private static GameInitiationMessageParser instance = null;


            public static GameInitiationMessageParser Instance
            {
                get
                {
                    if(instance == null)
                        instance = new GameInitiationMessageParser();
                    return instance;
                }
            }
            //Returns null if parsing fails. Otherwise, return GameInitiationMessage object
            public override ServerMessage TryParse(string[] sections)
            {
                if (sections[0].ToLower() == "i")
                {
                    //I:P<num>: <Brick x>,<Brick y>;<Brick x>,<Stone x>.<Stone y>;<Stone x>,<Stone y>:<Water x>.<Water y>;<Water x>,<Water y>#
                    //Load map
                    MapDetails mapDetails = new MapDetails();
					GameWorld.Instance.MyPlayerNumber = (int)Char.GetNumericValue(sections[1][1]);

                    
                    string section = sections[2];
                    string[] parameters = Tokenizer.TokernizeParameters(section);
                    
                    //load bricks
                    Coordinate[] coordinates = new Coordinate[parameters.Length];
                    for (int j = 0; j < parameters.Length; j++ )
                    {
                        coordinates[j] = Tokenizer.TokernizeCoordinates(parameters[j]);
                    }
                    mapDetails.Brick = coordinates;

                    section = sections[3];
                    parameters = Tokenizer.TokernizeParameters(section);
                    //load stones
                    coordinates = new Coordinate[parameters.Length];
                    for (int j = 0; j < parameters.Length; j++)
                    {
                        coordinates[j] = Tokenizer.TokernizeCoordinates(parameters[j]);
                    }
                    mapDetails.Stone = coordinates;

                    
                    section = sections[4];
                    parameters = Tokenizer.TokernizeParameters(section);
                    coordinates = new Coordinate[parameters.Length];
                    //load water pools
                    for (int j = 0; j < parameters.Length; j++)
                    {
                        coordinates[j] = Tokenizer.TokernizeCoordinates(parameters[j]);
                    }
                    mapDetails.Water = coordinates;
                    
                    GameInitiationMessage result = new GameInitiationMessage();
                    result.mapDetails = mapDetails;
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
