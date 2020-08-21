using System;
using KCmdCommand;

namespace KCmdAsm {
    public class KCmdAsm : ICommand {
        public string Name { get => "asm"; }

        public string Description { get => "Assembly utility"; }

        public int Exec(ICall args) {

            return 0;
        }
    }
}
