using KCmdCommand;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace KCmdGet {
    public class KCmdGet : ICommand {
        public string Name { get => "get"; }

        public string Description { get => "Obtain a new command from remote"; }


        public enum GetCmdReturnCode {
            SUCCESS = 0,
            DOWNLOAD_ERROR,
            FILE_ALREADY_EXISTS,
            DELETION_ERROR
        }

        private int GetCmd(ICall args) {

            bool over = args.Modifier.Contains("y");
            string localFile = Directory.GetCurrentDirectory() + "\\" + CMD_FOLDER + "\\" + args.Arguments[1] + ".dll";
            if (File.Exists(localFile)) {
                if(!over) {
                    Console.Write("File already exists, do you want to override ? (y/n) ");
                    if (Console.ReadKey().Key != ConsoleKey.Y) {
                        Console.WriteLine();
                        return (int)GetCmdReturnCode.FILE_ALREADY_EXISTS;
                    }
                    Console.WriteLine();
                }
                try {
                    File.Delete(localFile);
                }
                catch(Exception e) {
                    Console.WriteLine($"Error during {localFile} deletion : {e.Message}");
                    return (int)GetCmdReturnCode.DELETION_ERROR;
                }
            }
            try {
                WebClient.DownloadFile(REMOTE_PROTO + REMOTE_SERV_DEFAULT + "/" + REMOTE_SERV_MAINFOLDER + "/" + REMOTE_SERV_CMDS + "/" + args.Arguments[1] + ".dll",
                    localFile);
            }
            catch(Exception e) {
                Console.WriteLine($"Error during downloading {args.Arguments[1]}: {e.Message}");
                return (int)GetCmdReturnCode.DOWNLOAD_ERROR;
            }
            return (int)GetCmdReturnCode.SUCCESS;
        }

        private int ConfRemote(ICall args) {
            return 0;
        }

        public void InitSelector() {
            WebClient = new WebClient();
            WebClient.Encoding = System.Text.Encoding.UTF8;
            ArgSelector = new Dictionary<string, Func<ICall, int>> {
                { ARG_CMD, GetCmd },
                { ARG_REMOTE, ConfRemote }
            };
        }

        private void ShowHelp() {
            Console.WriteLine("Usage: ");
            Console.WriteLine("\tcmd {name}\tGet a cmd by its name");
            Console.WriteLine("\tremote (add/remove) {remote}\tConfigure the remote server");
        }

        public int Exec(ICall args) {
            if (ArgSelector == null) InitSelector();
            if(args.Arguments.Count == 0) {
                ShowHelp();
                return 1;
            }
            return ArgSelector[args.Arguments[0]].Invoke(args);
        }

        private Dictionary<string, Func<ICall, int>> ArgSelector;
        private WebClient WebClient;

        public const string ARG_CMD     = "cmd";
        public const string ARG_REMOTE  = "remote";

        private const string REMOTE_SERV_DEFAULT    = "kwserv.ddns.net";
        private const string REMOTE_SERV_MAINFOLDER = "kcmd";
        private const string REMOTE_SERV_CMDS       = "cmds";
        private const string REMOTE_PROTO           = "http://";

        private const string CMD_FOLDER = "cmds";
    }
}
