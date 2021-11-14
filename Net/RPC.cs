using DiscordRPC;
using DiscordRPC.Logging;
using FH5RP.Data;
using System;
using LogLevel = DiscordRPC.Logging.LogLevel;

namespace FH5RP.Net
{
    public class RPC
    {
        private static string ClientId = "909362638918668319";
        private static DiscordRpcClient Client { get; set; }

        public static void Initialize()
        {
            Client = new DiscordRpcClient(ClientId);
            Client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };
            Client.OnReady += (s, e) => Console.WriteLine($"[DiscordRPC] {s.ToString()} :: {e.Type} - {e.User} (ver: {e.Version})");
            Client.OnConnectionFailed += (s, e) => Console.WriteLine($"DiscordRPC connection failed.\n\t{e}");
            Client.Initialize();
        }

        public static void UpdatePresence(TelemetryData data)
        {
            if (data is null) return;

            Client.SetPresence(new RichPresence()
            {
                State = $"{(int)data.GetMPH()} MPH ({(int)data.GetKPH()} KPH)",
                Details = "Exploring México",
                Assets = new Assets()
                {
                    LargeImageKey = "logo",
                    SmallImageKey = $"carclass-{data.Vehicle.Index.ToString().ToLower()}",
                    SmallImageText = $"{data.Vehicle.Index.ToString()} | {data.Vehicle.PIValue}"
                }
            });
        }
    }
}
