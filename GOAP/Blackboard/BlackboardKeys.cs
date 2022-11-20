
namespace GOAP
{
    class BlackboardKeys
    {
        /* Use this class as enum of strings. */

        private BlackboardKeys(string value) { Str = value; }

        public string Str { get; private set; }

        public static BlackboardKeys BBTargetDist{ get { return new BlackboardKeys("TargetDist"); } }
        public static BlackboardKeys BBTargetName { get { return new BlackboardKeys("TargetName"); } }
    }
}
