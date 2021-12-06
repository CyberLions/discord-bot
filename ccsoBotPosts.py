import discord

#Used this to create the code:
#https://cog-creators.github.io/discord-embed-sandbox/
#Added in some custom stuff since the code generator is limited

def getRulePost():

	embed=discord.Embed(title="Server Rules")
	embed.add_field(name="General Rules", value="**- To be verified as a member, your Discord nickname must contain either your first name or your IRL nickname**"
                                        + "\n" + "- The member role is used to help mitigate against bots and spam on the server"
                                        + "\n" + "- To verify your membership, please message one of the moderators or executive officers"
                                        + "\n" + "- No promoting illegal and or unethical behavior. This includes but is not limited to: dicussing how to execute real-world cyber attacks, discussing ways to evade authorities, promoting methods of purchasing illicit goods, or other illegal and or unethical behaviors"
                                        + "\n", inline=False)
	embed.add_field(name="Text Chat Rules", value="- No spamming or flooding the chat"
                                        + "\n" + "- No racist or sexist content"
                                        + "\n" + "- No offensive content specifically meant to degrade a culture or subculture"
                                        + "\n" + "- No harassment or hazing"
                                        + "\n" + "- No adult content"
                                        + "\n" + "- No disgusting / offensive images"
                                        + "\n" + "- No advertising without moderator / officer permission (or second moderator / officer permission if you are one)"
                                        + "\n" + "- No unnecessary use of mentions (@someone / @role)"
                                        + "\n", inline=False)
	embed.add_field(name="Voice Chat Rules (Inherits from text chat)", value="- No voice chat surfing or switching channels repeatedly"
                                                                    + "\n" + "- No annoying, loud, or high-pitched sounds"
                                                                    + "\n" + "- You may be removed if your audio quaility is bothering other members", inline=False)
	
	return embed

def getRulePost2():
	
	embed=discord.Embed(title="Category and Channel Descriptions", description="Try to post things that correspond with each channel and or category." + "\n" + "\n" + "Use #role-selection to assign yourself roles to access these categories")
	embed.add_field(name="Important", value="Contains channels that members should look at on a regular basis. This category includes the server rules and upcoming annoucements for CCSO", inline=False)
	embed.add_field(name="Resources", value="Contains channels that offer helpful content that can everyone can use to learn new things and discover something that they didn't know about before"
                            + "\n" + "\n" + "*Note: Only Moderators and Executive officers can add resources. if you have found something that you think will benefit everyone, please message one of the moderators / officers to see if they can add the resource you found*", inline=False)
	embed.add_field(name="General", value="This is where everyone that joins can message and talk to other people", inline=False)
	embed.add_field(name="War Room", value="This is where everyone who is interested in gaming and can post information regarding that", inline=False)
	embed.add_field(name="Offense", value="This is for people interested in offensive security and our CPTC competition team", inline=False)
	embed.add_field(name="Defense", value="This is for people interested in defensive security and our CCDC competition team", inline=False)
	embed.add_field(name="CTF Competitions", value="This is for people interested in CTFs as well as platforms such as TryHackMe and HackTheBox", inline=False)

	return embed

def getRulePost3():

	embed=discord.Embed(title="Member Role Benefits", description="Everyone is welcome in our server. We do respect people's privacy but we would like to know how to address you formally. The officers have decided to incentivize the member role. The permissions below are only allowed through the member role and are not turned on for default users.")
	embed.add_field(name="File Upload", value="You will be able to upload files", inline=False)
	embed.add_field(name="Embedded Links", value="The links that you post will grant automatic embeds. This includes automactially posting an image from a link", inline=False)
	embed.add_field(name="Create Invites", value="You will be able to create invite links to share with your friends who would be interested in joining our server", inline=False)
	embed.add_field(name="External Emojis and Stickers", value="You will be able to add external reactions from other servers you are apart of", inline=False)
	embed.add_field(name="Mentions", value="You will be able to use mentions (please refer back to text chat rules)", inline=False)

	return embed

def getRulePost4():

	embed=discord.Embed(title="Note", description="Created 4/19/2019, Updated 11/7/2021")

	return embed

'''
def getPlatformPost():
'''



