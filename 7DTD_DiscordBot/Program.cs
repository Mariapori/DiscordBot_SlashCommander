using Discord;
using Discord.WebSocket;
using System.Reactive.Linq;

var bot = new DiscordSocketClient();
var discordToken = string.Empty;

if (args.Any())
{
    discordToken = args[0];
}

Dictionary<SlashCommandBuilder, Action<SocketSlashCommand>> CommandActions = new Dictionary<SlashCommandBuilder, Action<SocketSlashCommand>>();

bot.Ready += Bot_Ready;
bot.SlashCommandExecuted += Bot_SlashCommandExecuted;

BuildCommand("komento1", "Tämä komento on täysin testi", async ( komento ) =>
{
    await komento.RespondAsync("No moro vaan!");
});


async Task Bot_SlashCommandExecuted(SocketSlashCommand arg)
{
    var command = CommandActions.First(o => o.Key.Name == arg.CommandName);
    await command.Value.ToAsync().Invoke(arg);
}

async Task Bot_Ready()
{
    foreach (var commandToRegister in CommandActions.Keys)
    {
        await bot.CreateGlobalApplicationCommandAsync(commandToRegister.Build());
    }
    Console.WriteLine("Bot ready for commands!");
}

await MainAsync();

async Task MainAsync()
{
    await bot.LoginAsync(Discord.TokenType.Bot, discordToken, true);
    await bot.StartAsync();
    await Task.Delay(Timeout.Infinite);
}

void BuildCommand(string commandName,string commandDescription, Action<SocketSlashCommand> commandCallback)
{
    var komento = new SlashCommandBuilder();
    komento.Name = commandName.ToLower();
    komento.Description = commandDescription;
    komento.IsNsfw = false;
    CommandActions.Add(komento, commandCallback);
}