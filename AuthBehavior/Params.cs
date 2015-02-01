using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuthBehavior
{
    public class Params
    {
        // ALWAYS PLACE YOUR KEYS INSIDE YOUR .config files
        public static string _EncryptionKey = "ABCDEFGHIJKLMNOPQRSTUV";

        // For how long a single request can be valid +- 5 minutes in UTC
        public static int _PaddingMinutes = 5;
    }
}
