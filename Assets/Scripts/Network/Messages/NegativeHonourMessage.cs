using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClient.Network.Messages
{
    /*
    A Server originated message stating that it cannot honour a movement or shoot request
    */
    public class NegativeHonourMessage : ServerMessage
    {
        
        private NegativeHonourReason reason;
        /*
            The reason for negative honour
        */
        public NegativeHonourReason Reason
        {
            get
            {
                return reason;
            }
            set
            {
                reason = value;
            }
        }

        public NegativeHonourMessage(NegativeHonourReason reason)
        {
            this.reason = reason;
        }

        //TODO: Report to GameEngine about Negative Honour
        public override void Execute()
        {

            GameDomain.GameWorld.Instance.NotifyNegativeHonour(reason);
        }

        /*
        Textual Representation for loging purposes
        */
        public override string ToString()
        {
           
            switch(reason)
            {
                case NegativeHonourReason.CellOccupied:
                    return "Negative Honour: Cell is already occupied";
                case NegativeHonourReason.Dead:
                    return "Negative Honour: Player is dead";
                case NegativeHonourReason.GameHasFinished:
                    return "Negative Honour: The game has finished already";
                case NegativeHonourReason.GameHasNotStarted:
                    return "Negative Honour: Game has not started still";
                case NegativeHonourReason.InvalidCell:
                    return "Negative Honour: Cannot go to that cell";
                case NegativeHonourReason.InvalidContestant:
                    return "Negative Honour: You are not a valid contestant";
                case NegativeHonourReason.Obstacle:
                    return "Negative Honour: You have met with an obstacle";
                case NegativeHonourReason.TooQuick:
                    return "Negative Honour: You have to wait for atleast a second before sending next message";
                case NegativeHonourReason.PitFall:
                    return "Negative Honour: You have fallen to water";
            }
            return "Negative Honour";
        }

        /*
        The possible reasons for negative honours
        */
        public enum NegativeHonourReason
        {
            Obstacle,CellOccupied,Dead,TooQuick,InvalidCell,GameHasFinished,GameHasNotStarted,InvalidContestant,PitFall
        }
        /*
        Parser to parse NegativeHonour Messages. If succesful, returns NegativeHonourMessage object with appropriate Reason. Otherwise return null
        */
        public class NegativeHonourMessageParser : ServerMessageParser
        {
            /*
            Singleton
            */
            private NegativeHonourMessageParser()
            {

            }
            private static NegativeHonourMessageParser instance = null;

            public static NegativeHonourMessageParser Instance
            {
                get
                {
                    if (instance == null)
                        instance = new NegativeHonourMessageParser();
                    return instance;
                }
            }
            /*
            Detect and parse message. IF not a NegativeHonour, returns null
            */
            public override ServerMessage TryParse(string[] sections)
            {
                string msg = sections[0];
                string[] parameters = Tokenizer.TokernizeParameters(msg);
                switch (parameters[0].ToUpper().Trim())
                {
                    case "OBSTACLE":
                        return new NegativeHonourMessage(NegativeHonourReason.Obstacle);
                    case "CELL_OCCUPIED":
                        return new NegativeHonourMessage(NegativeHonourReason.CellOccupied);
                    case "DEAD":
                        return new NegativeHonourMessage(NegativeHonourReason.Dead);
                    case "TOO_QUICK":
                        return new NegativeHonourMessage(NegativeHonourReason.TooQuick);
                    case "INVALID_CELL":
                        return new NegativeHonourMessage(NegativeHonourReason.InvalidCell);
                    case "GAME_HAS_FINISHED":
                        return new NegativeHonourMessage(NegativeHonourReason.GameHasFinished);
                    case "GAME_NOT_STARTED_YET":
                        return new NegativeHonourMessage(NegativeHonourReason.GameHasNotStarted);
                    case "NOT_A_VALID_CONTESTANT":
                        return new NegativeHonourMessage(NegativeHonourReason.InvalidContestant);
                    case "PITFALL":
                        return new NegativeHonourMessage(NegativeHonourReason.PitFall);

                }
                //Not a NegativeHonour message
                return null;
            }
        }



      

    }
}
