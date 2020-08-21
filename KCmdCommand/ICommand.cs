using System;

namespace KCmdCommand {
    public interface ICommand {
        public string Name  { get;}
        public string Description { get; }
        public int Exec(ICall args);
    }
}
