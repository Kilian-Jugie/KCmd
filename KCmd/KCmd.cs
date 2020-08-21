using KCmdCommand;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace KCmd {
    class KCmd {
        public enum ExitInstruction {
            NOEXIT,
            EXIT,
            RESTART
        }
        
        private KCmd() { }
        static KCmd() { }

        public static KCmd Instance { get; } = new KCmd();

        static void Main(string[] args) {
            ExitCode = 0;
            Commands = new Dictionary<string, ICommand>();
            do {
                Commands.Clear();
                Instance.Exit = ExitInstruction.NOEXIT;
                Instance.RegisterInternalCommands();
                CommandLoader.LoadFromPath(COMMANDS_PATH, Commands);
                Instance.Loop();
            } while (Instance.Exit == ExitInstruction.RESTART);
            Environment.ExitCode = ExitCode;
        }

        public void RegisterInternalCommands() {
            Commands.Add(CMD_EXIT, new InternalCommand(CMD_EXIT, "Exit the cmd", (ICall a) => { return KCmd.RCODE_EXIT; }));
            Commands.Add(CMD_RESTART, new InternalCommand(CMD_RESTART, "Restart the cmd", (ICall a) => { return KCmd.RCODE_RESTART; }));
            Commands.Add(CMD_CD, new InternalCommand(CMD_CD, "Change the active directory", (ICall a) => {
                try {
                    Directory.SetCurrentDirectory(a.Arguments[0]);
                }catch(Exception e) {
                    Console.WriteLine(e.Message);
                }
                return 0;
            }));
            Commands.Add(CMD_LS, new InternalCommand(CMD_LS, "Display the directory content", (ICall a) => {
                var files = Directory.EnumerateFileSystemEntries(Directory.GetCurrentDirectory());
                foreach (var v in files) {
                    Console.WriteLine(v.Substring(v.LastIndexOf('\\')+1));
                }
                return 0;
            }));
            Commands.Add(CMD_DIR, Commands[CMD_LS]);
            Commands.Add(CMD_HELP, new InternalCommand(CMD_HELP, "Get Help", (ICall a) => {
                foreach(var v in Commands) {
                    Console.WriteLine($"{v.Key} : {v.Value.Description}");
                }

                return 0;
            }));
        }

        private void InputParser(string input) {
            bool hasParams = true;
            int spIndex = input.IndexOf(' ');
            if (spIndex < 0) {
                spIndex = input.Length;
                hasParams = false;
            }
            string first = input.Substring(0, spIndex);

            if (!Commands.ContainsKey(first)) {
                Console.WriteLine($"ERROR: {first} is not recognized as internal command !");
                return;
            }

            int rcode = 0;

            if (hasParams) {
                input = input.Substring(spIndex + 1);
                rcode = Commands[first].Exec(new CmdCall(input.Split(' ')));
            }
            else {
                rcode = Commands[first].Exec(new CmdCall(null));
            }

            switch (rcode) {
                case RCODE_EXIT: 
                        Exit = ExitInstruction.EXIT;
                        break;
                case RCODE_RESTART: 
                        Exit = ExitInstruction.RESTART;
                        break;
                default: break;
            }
        }

        public void Loop() {
            string input;
            while(Exit == ExitInstruction.NOEXIT) {
                Console.Write(Directory.GetCurrentDirectory()+"> ");
                input = Console.ReadLine();
                InputParser(input);
            }
        }

        public const string COMMANDS_PATH = "cmds";

        public const string CMD_EXIT = "exit";
        public const string CMD_RESTART = "restart";
        public const string CMD_CD = "cd";
        public const string CMD_LS = "ls";
        public const string CMD_DIR = "dir";
        public const string CMD_HELP = "help";

        private static int ExitCode;
        private ExitInstruction Exit;
        
        static Dictionary<string, ICommand> Commands;

        public const int RCODE_EXIT = int.MinValue;
        public const int RCODE_RESTART = RCODE_EXIT + 1;
    }
}
