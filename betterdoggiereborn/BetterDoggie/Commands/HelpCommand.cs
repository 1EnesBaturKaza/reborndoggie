using CommandSystem;
using System;

namespace BetterDoggie.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class HelpCommand : ICommand
    {
        public string Command { get; } = "doggiehelp";
        public string[] Aliases { get; } = Array.Empty<string>();
        public string Description { get; } = "Shows help for the Better Doggie plugin";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = BetterDoggie.Singleton.Config.helpcommandtext;
            return true;
        }
    }
}
