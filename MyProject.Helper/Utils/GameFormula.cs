using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Helper.Utils
{
    public static class GameFormula
    {
        public static decimal CalculateTotalReward (int playersNumber)
        {
            return playersNumber * 0.95m;
        }

        public static decimal CalculateReward (int numberPlayer, decimal totalReward, int rank)
        {
            int winnerCount = numberPlayer / 2;

            decimal rewardFormula = 1 + (((decimal)winnerCount - (decimal)rank + 1) * 2 * (0.95m * numberPlayer - (decimal)winnerCount)) / ((decimal)winnerCount * ((decimal)winnerCount + 1));
            decimal reward = rank <= winnerCount ? rewardFormula : 0;

            return reward;
        }

        public static decimal CalculateDonate (decimal totalReward)
        {
            return totalReward * 0.01m;
        }

        public static decimal CalculateDollarReward (decimal famous, long follower)
        {
            return famous + (follower / 100000) + 1;
        }



    }
}
