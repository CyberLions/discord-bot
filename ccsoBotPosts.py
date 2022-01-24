import discord
import array as arr

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

def getPlatformPost():

    embed=discord.Embed(title="Top 5 Resources")

    return embed

def getPlatformPost1():
    embed=discord.Embed(title="TryHackMe", url="https://tryhackme.com", description="Best platform overall. Provides learning topics such as the basics and more advanced topics within offensive and defensive security. Great for any beginner.")

    return embed

def getPlatformPost2():
    embed=discord.Embed(title="TCM Academy", url="https://academy.tcm-sec.com/", description="Provides good courses that you can get a discount for or completely free. Most of the courses focus on offensive security but they plan on covering more subjects in the future. Some courses are beginner to intermediate level.")

    return embed

def getPlatformPost3():
    embed=discord.Embed(title="HackTheBox", url="https://www.hackthebox.com/", description="Best offensive security platform overall. This is the go to platform when it comes to pen testing. However, they mainly specialize with offensive security and lack defensive content. High learning curve, recommended for intermediates.")

    return embed

def getPlatformPost4():
    embed=discord.Embed(title="OverTheWire", url="https://overthewire.org", description="Good platform to practice linux skills. Recommended for all beginners and for experienced users to brush up on some things. Also check out its cousin: https://underthewire.tech/")

    return embed

def getPlatformPost5():
    embed=discord.Embed(title="INE", url="https://ine.com/", description="Provide free learning content. They host training for their eLearnSecurity certifications on the site as well. Beginner level.")

    return embed

def getPlatformPost6():
    embed=discord.Embed(title="Offensive Security Resources", color= 15158332)

    return embed

def getPlatformPost7():
    embed=discord.Embed(title="PortSwigger", url="https://portswigger.net/web-security", description="Provides an academy for web app pen testing. They are also the creators of BurpSuite.", color= 15158332)

    return embed

def getPlatformPost8():
    embed=discord.Embed(title="PentesterLab", url="https://pentesterlab.com/", description="Host a large collection of web app pen testing exercises.", color= 15158332)

    return embed

def getPlatformPost9():
    embed=discord.Embed(title="JuiceShop", url="https://owasp.org/www-project-juice-shop/", description="Web app designed for pen testing beginners. This was created by the people behind the OWASP Top 10 framework.", color= 15158332)

    return embed

def getPlatformPost10():
    embed=discord.Embed(title="CyberSecLabs", url="https://www.cyberseclabs.co.uk/", description="Another platform similar to TryHackMe. Based in the UK.", color= 15158332)

    return embed

def getPlatformPost11():
    embed=discord.Embed(title="VulnHub", url="https://www.vulnhub.com/", description="User created VMs that you can download and hack. Highly recommended for anyone that would like to work with VMs more and or interested in making a home lab.", color= 15158332)

    return embed

def getPlatformPost12():
    embed=discord.Embed(title="HackThisSite", url="https://www.hackthissite.org/", description="Site that hosts web pen testing exercises as well.", color= 15158332)

    return embed

def getPlatformPost13():
    embed=discord.Embed(title="Hack.Me", url="https://hack.me/", description="Web app pen testing resource.", color= 15158332)

    return embed

def getPlatformPost14():
    embed=discord.Embed(title="Defensive Security Resources", color= 3447003)

    return embed

def getPlatformPost15():
    embed=discord.Embed(title="Blue Team Labs Online", url="https://blueteamlabs.online/", description="Only defensive security platform that has been found. They host challenges and exercises that are heavy in incident response and forensics. Made by Security Blue Team.", color= 3447003)

    return embed

def getPlatformPost16():
    embed=discord.Embed(title="VulnHub", url="https://www.vulnhub.com/", description="This is listed as defensive since you can download the VMs and try to harden them against the vulnerabilities you exploited. Best path to take is to look for guides on how to exploit the VMs and then research how to patch that certain exploit.", color= 3447003)

    return embed

def getPlatformPost17():
    embed=discord.Embed(title="Digital Forensics Association", url="http://www.digitalforensicsassociation.org/evidence-files/", description="Collection of forensic files that are free to use.", color= 3447003)

    return embed

def getPlatformPost18():
    embed=discord.Embed(title="Metasploitable 2 Hardening Guide", url="https://akvilekiskis.com/work/metasploitable/index.html", description="Guide on how to patch and fix vulnerabilities on Metasploitable 2. This overall represents what CCDC focuses on.", color= 3447003)

    return embed

def getPlatformPost19():
    embed=discord.Embed(title="CTF Resources", color= 15105570)

    return embed

def getPlatformPost20():
    embed=discord.Embed(title="PicoCTF", url="https://www.picoctf.org/", description="A free beginner CTF that has a yearly competition. They also have a gymnasium full of all of their previous competitions.", color= 15105570)

    return embed

def getPlatformPost21():
    embed=discord.Embed(title="National Cyber League", url="https://nationalcyberleague.org/", description="Beginner CTF that you have to pay for. Provides great challenges to solve.", color= 15105570)

    return embed

def getPlatformPost22():
    embed=discord.Embed(title="CTF Time", url="https://ctftime.org/", description="Website dedicated to listing all future CTF events.", color= 15105570)

    return embed

