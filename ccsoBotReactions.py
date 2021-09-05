import discord
import os
import time

# Intents library required for the on_raw_reaction_add function
intents = discord.Intents.default()
intents.reactions = True

client = discord.Client()


def setClientToken(token):
    print("Client set in Reactions")
    global client
    client = token
    # sets global var pog

'''
def setRoleChannelId(channelId):
    print("Channel ID set for Roles")
    global roleChannelId
    roleChannelId = channelId
'''


# Emojis for roles

dynamicRoles = [
    '1️⃣',
    '2️⃣',
    '3️⃣',
    '4️⃣',
    '⭐'
]

staticRoles = [
    '⚔️',
    '🛡️',
    '🏳️',
    '🎮',
]


def getRoleEmojis():
    return dynamicRoles + staticRoles


def rolePicker(string, message):
    ctfRole = discord.utils.get(message.guild.roles, name="CTF")
    cptcRole = discord.utils.get(message.guild.roles, name="Offense (CPTC)")
    ccdcRole = discord.utils.get(message.guild.roles, name="Defense (CCDC)")
    gamingRole = discord.utils.get(message.guild.roles, name="Gaming")

    firstYear = discord.utils.get(message.guild.roles, name="First Year")
    secondYear = discord.utils.get(message.guild.roles, name="Second Year")
    thirdYear = discord.utils.get(message.guild.roles, name="Third Year")
    fourthYear = discord.utils.get(message.guild.roles, name="Fourth Year")
    alumni = discord.utils.get(message.guild.roles, name="Alumni / Other")

    switcher = {
        '🏳️': ctfRole,
        '⚔️': cptcRole,
        '🛡️': ccdcRole,
        '🎮': gamingRole,
        '1️⃣': firstYear,
        '2️⃣': secondYear,
        '3️⃣': thirdYear,
        '4️⃣': fourthYear,
        '⭐': alumni
    }
    return switcher.get(string, None)


async def addReactionsToMessage(dynamicMessage, staticMessage):
    # Adds the emoji reactions to the message initially

    for role in dynamicRoles:
        await dynamicMessage.add_reaction(role)

    for role in staticRoles:
        await staticMessage.add_reaction(role)




async def addRoleReaction(payload):
    print("Reaction seen")
    print(payload.channel_id)
    print(client)
    print(client.get_channel(payload.channel_id))

    message = await client.get_channel(payload.channel_id).fetch_message(payload.message_id)
    message_channel = client.get_channel(payload.channel_id)

    emoji = str(payload.emoji)
    user = payload.member
    userId = payload.user_id
    print(emoji, user)

    # Check to make sure the user isnt the bot, so the bot doesn't react to the message then immeadietly remove the react
    if userId != client.user.id:
        # Check to see if the react is in the correct channel so it doesnt trigger in other channels
        # REPLACE THIS CHANNEL NAME AS NEEDED
        if discord.utils.get(client.get_all_channels(), name="role-selection") == message_channel:
            try:
                # Gets the role based on the emoji and adds it to the user w/ a print
                role = rolePicker(emoji, message)

                if role in user.roles:
                    await user.remove_roles(role, message)
                    print("Removed role " + str(role))
                else:
                    # check if its static or dynamic
                    # if dynamic, remove other dynamic roles before adding

                    for dynamicRole in dynamicRoles:

                        if emoji == dynamicRole:

                            firstYear = discord.utils.get(
                                message.guild.roles, name="First Year")
                            secondYear = discord.utils.get(
                                message.guild.roles, name="Second Year")
                            thirdYear = discord.utils.get(
                                message.guild.roles, name="Third Year")
                            fourthYear = discord.utils.get(
                                message.guild.roles, name="Fourth Year")
                            alumni = discord.utils.get(
                                message.guild.roles, name="Alumni / Other")
                            rolesToRemove = [
                                firstYear, secondYear, thirdYear, fourthYear, alumni]
                            roleToKeep = rolePicker(emoji, message)

                            rolesToRemove.remove(roleToKeep)
                            for toRemove in rolesToRemove:
                                await user.remove_roles(toRemove)

                    await user.add_roles(role, message)
                    print("Added role " + str(role))

            # Error handling
            except AttributeError as err:
                print(err)
                print("Role does not exist")
            except discord.errors.NotFound:
                print("Tried to add role user already has")

            await message.remove_reaction(emoji, user)


async def embedRoleMessage():
        mf1 = getRoleEmojis()[0]
        mf2 = getRoleEmojis()[1]
        mf3 = getRoleEmojis()[2]
        mf4 = getRoleEmojis()[3]
        mf5 = getRoleEmojis()[4]

        rolesText = "{} **1st Year**".format(mf1) + "\n" + "{} **Second Year**".format(mf2) + "\n" + "{} **Third Year**".format(mf3) + "\n" + "{} **Fourth Year**".format(mf4) + "\n" + "{} **Alumni / Other**".format(mf5)

        rolesChannel = discord.utils.get(client.get_all_channels(), name="role-selection")
        await rolesChannel.purge()
        dynamicRoles = discord.Embed(
            title="Class Roles", description="**React below to select your class/year!**" + "\n" + "\n" + rolesText)
        await rolesChannel.send(embed=dynamicRoles)

        dynamic_id = rolesChannel.last_message_id

        cr1 = getRoleEmojis()[5]
        cr2 = getRoleEmojis()[6]
        cr3 = getRoleEmojis()[7]
        cr4 = getRoleEmojis()[8]

        staticRoles = discord.Embed(
            title="Club Roles", description="**React to receive a role that you are interested in!**")
        staticRoles.add_field(
            name="{} Offense (CPTC)".format(cr1), value="For people interested in offensive security and or penetration testing. This is also for people interested in our CPTC competition team!", inline=False)
        staticRoles.add_field(
            name="{} Defense (CCDC)".format(cr2), value="For people interested in defensive security and or security monitoring. This is also for people interested in our CCDC competition team!", inline=False)
        staticRoles.add_field(
            name="{} CTF".format(cr3), value="For people interested in competing in CTFs such as NCL and PicoCTF. This is also for people interested in other platforms such as Hack the box, TryHackMe, and Blue team labs online!", inline=False)
        staticRoles.add_field(
            name="{} Gaming".format(cr4), value="For people interested in hanging out with club members and playing video games. Some of the games include Minecraft, Among Us, and Escape From Tarkov!", inline=False)

        await rolesChannel.send(embed=staticRoles)

        static_id = rolesChannel.last_message_id

        # function that adds reactions if the message is deleted
        # get the messages, get the ids
        roleChannelId = 879214780764798996
        print(static_id)
        print(dynamic_id)

        dynamicMessage = await client.get_channel(roleChannelId).fetch_message(dynamic_id)
        staticMessage = await client.get_channel(roleChannelId).fetch_message(static_id)

        await addReactionsToMessage(dynamicMessage, staticMessage)

