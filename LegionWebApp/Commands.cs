using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steam.Net;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace LegionWebApp
{
    public class Commands : BaseCommandModule
    {
        [Command("rank")]
        public async Task GetRank(CommandContext ctx, ulong SteamID)
        {
            SteamClient steamClient = new SteamClient("DE88A3075869202895093D261FB66F7D");
            SteamApp dbd = steamClient.GetSteamApp(381210);
            SteamUser steamUser = steamClient.GetSteamUser(SteamID);
            var playerStats = steamUser.GetUserStatsForGame(dbd);

            var KillerRank = GetRank(playerStats.Playerstats.Stats.FirstOrDefault(x => x.Name == "DBD_KillerSkulls").Value);
            var SurrivorRank = GetRank(playerStats.Playerstats.Stats.FirstOrDefault(x => x.Name == "DBD_CamperSkulls").Value);
            
            await ctx.Channel.SendMessageAsync($":knife: {KillerRank} | :man_running: {SurrivorRank}").ConfigureAwait(false);

            //await member.ModifyAsync(x => x.Nickname = ctx.Member.Nickname + $":knife: {KillerRank} | :man_running: {SurrivorRank}");
        }

        [Command("nick"), Description("Gives someone a new nickname.")]
        public async Task ChangeNickname(CommandContext ctx, [Description("Member to change the nickname for.")] DiscordMember member)
        {
            ulong SteamID = 76561198249679092;
            SteamClient steamClient = new SteamClient("DE88A3075869202895093D261FB66F7D");
            SteamApp dbd = steamClient.GetSteamApp(381210);
            SteamUser steamUser = steamClient.GetSteamUser(SteamID);
            var playerStats = steamUser.GetUserStatsForGame(dbd);
            var KillerRank = GetRank(playerStats.Playerstats.Stats.FirstOrDefault(x => x.Name == "DBD_KillerSkulls").Value);
            var SurrivorRank = GetRank(playerStats.Playerstats.Stats.FirstOrDefault(x => x.Name == "DBD_CamperSkulls").Value);



            // let's trigger a typing indicator to let
            // users know we're working
            await ctx.TriggerTypingAsync();

            try
            {
                // let's change the nickname, and tell the 
                // audit logs who did it.
                string oldnick;
                if (member.Nickname != null)
                {
                    oldnick = member.Nickname;
                }
                else
                {
                    oldnick = member.Username;
                }
                
                await member.ModifyAsync(x =>
                {
                    x.Nickname = oldnick + " 🔪" + KillerRank + "| 🏃‍" + SurrivorRank;
                    //x.AuditLogReason = $"Changed by {ctx.User.Username} ({ctx.User.Id}).";
                });

                // let's make a simple response.
                var emoji = DiscordEmoji.FromName(ctx.Client, ":+1:");

                // and respond with it.
                await ctx.RespondAsync(emoji);
            }
            catch (Exception e)
            {
                // oh no, something failed, let the invoker now
                var emoji = DiscordEmoji.FromName(ctx.Client, ":-1:");
                await ctx.RespondAsync(emoji + e.Message);
            }
        }

    

    private static string GetRank(long Pips)
        {
            string rank = string.Empty;
            if (Pips < 3)
            {
                rank = "Ash 4";
            }
            else if (Pips >= 3 && Pips < 6)
            {
                rank = "Ash 3";
            }
            else if (Pips >= 6 && Pips < 10)
            {
                rank = "Ash 2";
            }
            else if (Pips >= 10 && Pips < 14)
            {
                rank = "Ash 1";
            }
            else if (Pips >= 14 && Pips < 18)
            {
                rank = "Bronze 4";
            }
            else if (Pips >= 18 && Pips < 22)
            {
                rank = "Bronze 3";
            }
            else if (Pips >= 22 && Pips < 26)
            {
                rank = "Bronze 2";
            }
            else if (Pips >= 26 && Pips < 30)
            {
                rank = "Bronze 1";
            }
            else if (Pips >= 30 && Pips < 35)
            {
                rank = "Silver 4";
            }
            else if (Pips >= 35 && Pips < 40)
            {
                rank = "Silver 3";
            }
            else if (Pips >= 40 && Pips < 45)
            {
                rank = "Silver 2";
            }
            else if (Pips >= 45 && Pips < 50)
            {
                rank = "Silver 1";
            }
            else if (Pips >= 50 && Pips < 55)
            {
                rank = "Gold 4";
            }
            else if (Pips >= 55 && Pips < 60)
            {
                rank = "Gold 3";
            }
            else if (Pips >= 60 && Pips < 65)
            {
                rank = "Gold 2";
            }
            else if (Pips >= 65 && Pips < 70)
            {
                rank = "Gold 1";
            }
            else if (Pips >= 70 && Pips < 75)
            {
                rank = "Iridescent 4";
            }
            else if (Pips >= 75 && Pips < 80)
            {
                rank = "Iridescent 3";
            }
            else if (Pips >= 80 && Pips < 85)
            {
                rank = "Iridescent 2";
            }
            else if (Pips >= 85)
            {
                rank = "Iridescent 1";
            }
            return rank;
        }
    }
}
