using KCmdCommand;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Linq;

namespace KCmd {
    class CommandLoader {
        public static void LoadFromPath(string path, Dictionary<string, ICommand> cmds) {

            string[] files = Directory.GetFiles(path);

            foreach(var file in files) {
                Assembly asm;
                try {
                    asm = Assembly.LoadFrom(file);
                }
                catch (Exception e) {
                    Console.WriteLine($"Error during loading {file} in {path} : {e.Message}");
                    continue;
                }
                foreach (var type in asm.GetTypes()) {
                    if(type.GetInterfaces().Contains(typeof(ICommand))) {
                        object obj = Activator.CreateInstance(type);
                        cmds.Add(((ICommand)obj).Name, (ICommand)obj);
                    }
                }
            }
        }
    }
}
