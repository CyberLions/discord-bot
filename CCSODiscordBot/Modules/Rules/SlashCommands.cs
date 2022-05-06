using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace CCSODiscordBot.Modules.Rules
{
	public class SlashCommands : InteractionModuleBase<ShardedInteractionContext>
	{
		[SlashCommand("postrules", "Post the rules.")]
		[EnabledInDm(false)]
		[DefaultMemberPermissions(GuildPermission.Administrator)]
		public async Task PostRules([Summary(description: "The channel to post the rules in.")] SocketTextChannel? channel = null)
        {
			// Check to see if channel was defined:
			if(channel is null)
            {
				// Set to the rules channel:
				channel = Context.Guild.RulesChannel;
            }

			// Alert the executor of the command that the rules will be posted
			await RespondAsync("Posting rules to the "+channel.Name+" channel", ephemeral: true);

			// Rule Embed:
			var mainRulesEmbed = new EmbedBuilder();
			mainRulesEmbed.WithTitle("Server Rules");
			mainRulesEmbed.AddField("General Rules",
				"**- To be verified as a member, your Discord nickname must contain either your first name or your IRL nickname**"
				+ "\n" + "- The member role is used to help mitigate against bots and spam on the server"
				+ "\n" + "- To verify your membership, please message one of the moderators or executive officers"
				+ "\n" + "- No promoting illegal and or unethical behavior. This includes but is not limited to: dicussing how to execute real-world cyber attacks, discussing ways to evade authorities, promoting methods of purchasing illicit goods, or other illegal and or unethical behaviors"
				+ "\n"
			);
			mainRulesEmbed.AddField("Text Chat Rules",
				"- No spamming or flooding the chat"
				+ "\n" + "- No racist or sexist content"
				+ "\n" + "- No offensive content specifically meant to degrade a culture or subculture"
				+ "\n" + "- No harassment or hazing"
				+ "\n" + "- No adult content"
				+ "\n" + "- No disgusting / offensive images"
				+ "\n" + "- No advertising without moderator / officer permission (or second moderator / officer permission if you are one)"
				+ "\n" + "- No unnecessary use of mentions (@someone / @role)"
				+ "\n"
			);
			mainRulesEmbed.AddField("Voice Chat Rules (Inherits from text chat)",
				"- No voice chat surfing or switching channels repeatedly"
				+ "\n" + "- No annoying, loud, or high-pitched sounds"
				+ "\n" + "- You may be removed if your audio quaility is bothering other members"
			);
			mainRulesEmbed.WithColor(Color.Red);

			// Channel and category desc:
			var channelAndCatEmbed = new EmbedBuilder
			{
				Title = "Category and Channel Descriptions",
				Description = "Try to post things that correspond with each channel and or category." + "\n" + "\n" + "Use #role-selection to assign yourself roles to access these categories"
			};
			channelAndCatEmbed.AddField("Important", "Contains channels that members should look at on a regular basis. This category includes the server rules and upcoming annoucements for CCSO");
			channelAndCatEmbed.AddField("Resources", "Contains channels that offer helpful content that can everyone can use to learn new things and discover something that they didn't know about before"
							+ "\n" + "\n" + "*Note: Only Moderators and Executive officers can add resources. if you have found something that you think will benefit everyone, please message one of the moderators / officers to see if they can add the resource you found*");
			channelAndCatEmbed.AddField("General", "This is where everyone that joins can message and talk to other people");
			channelAndCatEmbed.AddField("War Room", "This is where everyone who is interested in gaming and can post information regarding that");
			channelAndCatEmbed.AddField("Offense", "This is for people interested in offensive security and our CPTC competition team");
			channelAndCatEmbed.AddField("Defense", "This is for people interested in defensive security and our CCDC competition team");
			channelAndCatEmbed.AddField("CTF Competitions", "This is for people interested in CTFs as well as platforms such as TryHackMe and HackTheBox");
			channelAndCatEmbed.WithColor(Color.Blue);

			// Member Role Benefits:
			var memeberEmbed = new EmbedBuilder
			{
				Title = "Member Role Benefits",
				Description = "Everyone is welcome in our server. We do respect people's privacy but we would like to know how to address you formally. The officers have decided to incentivize the member role. The permissions below are only allowed through the member role and are not turned on for default users."
			};
			memeberEmbed.AddField("File Upload", "You will be able to upload files");
			memeberEmbed.AddField("Embedded Links", "The links that you post will grant automatic embeds. This includes automactially posting an image from a link");
			memeberEmbed.AddField("Create Invites", "You will be able to create invite links to share with your friends who would be interested in joining our server");
			memeberEmbed.AddField("External Emojis and Stickers", "You will be able to add external reactions from other servers you are apart of");
			memeberEmbed.AddField("Mentions", "You will be able to use mentions (please refer back to text chat rules)");
			memeberEmbed.WithColor(Color.Blue);

			// Updated Embed:
			var updateEmbed = new EmbedBuilder { Title = "Note", Description = "Created 4/19/2019, Updated 11/7/2021" };
			updateEmbed.WithColor(Color.LightGrey);

			// Compile all embeds into a post and send it:
			await channel.SendMessageAsync(embeds: new Embed[] { mainRulesEmbed.Build(), channelAndCatEmbed.Build(), memeberEmbed.Build(), updateEmbed.Build()});
		}
	}
}

