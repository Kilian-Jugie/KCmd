using System;
using System.Collections.Generic;
using System.Text;
using KCmdCommand;

namespace KCmd {
    class CmdCall : ICall {

        public CmdCall(string[] args) {
            Modifier = new List<string>();
            Arguments = new List<string>();
            Raw = new List<string>();
            if (args == null) return;
            foreach(string a in args) {
                if (a[0] == ICall.MODIFIER_CHAR)
                    Modifier.Add(a.Substring(1));
                else
                    Arguments.Add(a);
                Raw.Add(a);
            }
        }
        public List<string> Modifier { get; }

        public List<string> Arguments { get; }

        public List<string> Raw { get; }
    }
}
