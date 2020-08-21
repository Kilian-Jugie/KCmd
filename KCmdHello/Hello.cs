using System;
using KCmdCommand;

namespace KCmdHello {
    public class Hello : ICommand {
        public string Name { get => "hello"; }

        public string Description { get => "Desc"; }

        public int Exec(ICall args) {
            Console.WriteLine("cctvvmb");
            return 0;
        }
    }
}
