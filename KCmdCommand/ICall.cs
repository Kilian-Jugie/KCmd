using System;
using System.Collections.Generic;
using System.Text;

namespace KCmdCommand {
    public interface ICall {

        public List<string> Modifier { get; }
        public List<string> Arguments { get; }
        public List<string> Raw { get; }

        public static char MODIFIER_CHAR = '-';
    }
}
