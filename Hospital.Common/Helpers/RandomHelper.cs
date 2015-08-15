using System;

namespace Hospital.Common.Helpers
{
    public class RandomHelper
    {
        private static RandomHelper _instance;

        public static RandomHelper Instance
        {
            get { return _instance ?? (_instance = new RandomHelper()); }
        }


        private readonly Random _rand;
        private RandomHelper()
        {
            _rand = new Random();
        }

        public int Random(int max)
        {
            return _rand.Next(max);
        }

        public int Random(int min, int max)
        {
            return _rand.Next(min, max);
        }
    }
}