using DiscordRPC;
using DiscordRPC.Message;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy_Swapper_v2.Workspace.Other
{
    public static class RPC
    {
        public static RichPresence Richprec;
        public static DiscordRpcClient Client { get; private set; }
        public static void StartRPC()
        {
            if (Workspace.Other.Colors.DiscordRPC() == "true")
                return;
            JObject parse = JObject.Parse(Workspace.global.ApiReturn("Rpc"));
            Client = new DiscordRpcClient(parse["RPCKey"].ToString());
            Client.Initialize();
            Client.OnReady += delegate (object sender, ReadyMessage e)
            {
                Workspace.global.Username = e.User.Username;
                Workspace.global.UserPfp = e.User.GetAvatarURL(User.AvatarFormat.PNG);
            };
            Richprec = new RichPresence()
            {
                Details = "Dashboard",
                State = parse["Details"].ToString(),
                Timestamps = Timestamps.Now,

                Assets = new Assets()
                {
                    LargeImageKey = parse["LargeImageKey"].ToString(),
                    LargeImageText = parse["LargeImageText"].ToString(),
                    SmallImageKey = parse["SmallImageKey"].ToString(),
                    SmallImageText = parse["SmallImageText"].ToString()
                }
            };
            Client.SetPresence(Richprec);
            SetRPC("Dashboard");
        }
        public static void SetRPC(string Details)
        {
            if (Workspace.Other.Colors.DiscordRPC() == "true")
                return;
            Richprec.Details = Details;
            Client.SetPresence(Richprec);
        }
    }
}
