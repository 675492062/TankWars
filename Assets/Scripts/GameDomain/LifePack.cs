using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Foundation;

namespace GameClient.GameDomain
{

    /*
     Lifepacks are displayed in Gameworld only for a limited time. A player may grab a lfiepack by moving over it.
    */
    public class LifePack
    {
        public Coordinate Position { get; set; }

        //is the lifepack already grabed by someone
        private bool grabbed = false;
        //time since addition of lifepack to world
        private int elapsedTime = 0;
        public int RemainingTime {
            get
            {
                return TimeLimit - elapsedTime;
            }

        }

        /*
         Is the medi pack still available in map?
        */
        public bool IsAlive
        {
            get
            {
                return RemainingTime > 0 & (!grabbed);
            }
        }

        /*
        Mark the lifepack as grabbed
        */
        public void Grab()
        {
            this.grabbed = true;
        }

        //The lifetime of life-pack
        public int TimeLimit { get; set; }

        /*
        A textual representation of lifepack
        */
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("LifePack position: " + Position.ToString() + "\tTimeLimit: " + TimeLimit);
            return builder.ToString();
        }

        /*
        Advance time to next frame. Updating the elapsed time of lifepack.
        Checks whether the lifepack is grabbed at this frame
        */
        public void AdvanceFrame()
        {
            if (elapsedTime < TimeLimit)
                elapsedTime+=1000;

            if(IsAlive)
            {
                //check whether the lifepack is grabbed by any players
                foreach(PlayerDetails p in GameWorld.Instance.Players)
                {
                    if (p.Position.X==Position.X && p.Position.Y == Position.Y)
                    {
                        Grab();
                    }
                }
            }
        }
    }
}
