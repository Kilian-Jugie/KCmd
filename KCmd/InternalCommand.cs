using KCmdCommand;
using System;
using System.Collections.Generic;
using System.Text;

namespace KCmd {
    class InternalCommand : ICommand {
        public InternalCommand(string name, string description, Func<ICall, int> exec) {
            ExecFunc = exec;
            Name = name;
            Description = description;
        }



        Func<ICall, int> ExecFunc;

        public string Name { get; }

        public string Description { get; }

        public int Exec(ICall args) {
            return ExecFunc(args);
        }
    }
}
