import discord
import os
import time

# Intents library required for the on_raw_reaction_add function
intents = discord.Intents.default()
intents.reactions = True

client = discord.Client()


def sendClientToken(token):
    print("client set!")
    global client
    client = token
    # sets global var pog


# Emojis for roles
emojis = {
    '👍',
    '👀',
    '🍉',
    '<:coolspot:854115885013663774>',
    '🏳️',
    '⚔️',
    '🛡️',
    '🎮',
    '⚪',
    '🔵',
    '🟤',
    '⚫',

}

staticRoles = [
    '1️⃣',
    '2️⃣',
    '3️⃣',
    '4️⃣',
    '⭐'
]

dynamicRoles = [
    '⚔️',
    '🛡️',
    '🏳️',
    '🎮',
]


def getRoleEmojis():
    return staticRoles + dynamicRoles


def rolePicker(string, message):
    role1 = discord.utils.get(message.guild.roles, name="test role 1")
    role2 = discord.utils.get(message.guild.roles, name="balls")
    role3 = discord.utils.get(message.guild.roles, name="super")
    role4 = discord.utils.get(message.guild.roles, name="server-manager"),

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
        '👍': role1,
        '👀': role2,
        '🍉': role3,
        '<:coolspot:854115885013663774>': role4,
        '🏳️': ctfRole,
        '⚔️': cptcRole,
        '🛡️': ccdcRole,
        '🎮': gamingRole,

        # '⚪': firstYear,
        # '🔵': secondYear,
        # '🟤': thirdYear,
        # '⚫': fourthYear,
        '1️⃣': firstYear,
        '2️⃣': secondYear,
        '3️⃣': thirdYear,
        '4️⃣': fourthYear,
        '⭐': alumni
    }
    return switcher.get(string, None)


async def addReactionsToMessage(staticMessage, dynamicMessage):

    # Adds the emoji reactions to the message initially
    for emoji in emojis:
        # await message.add_reaction(emoji)
        print("")

    # to be in order
    # this doesn't work since add_reaction is a coroutine
    # staticRoles = ['1️⃣',
    #                '2️⃣',
    #                '3️⃣',
    #                '4️⃣',
    #                '⭐']

    # staticMessage.add_reaction(staticRoles[0])
    # staticMessage.add_reaction(staticRoles[1])
    # staticMessage.add_reaction(staticRoles[2])
    # staticMessage.add_reaction(staticRoles[3])
    # staticMessage.add_reaction(staticRoles[4])

    for role in staticRoles:
        await staticMessage.add_reaction(role)

    for role in dynamicRoles:
        await dynamicMessage.add_reaction(role)


async def addRoleReaction(payload, a):

    # client = a

    print("Adding role")
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
        if discord.utils.get(client.get_all_channels(), name="bot-react-post-test") == message_channel:
            try:
                # Gets the role based on the emoji and adds it to the user w/ a print
                role = rolePicker(emoji, message)

                if role in user.roles:
                    await user.remove_roles(role, message)
                    print("removed role " + str(role))
                else:
                    # check if its static or dynamic
                    # if dynamic, remove other dynamic roles before adding

                    for staticRole in staticRoles:

                        if emoji == staticRole:

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
                    print("added role " + str(role))

            # Error handling
            except AttributeError as err:
                print(err)
                print("role does not exist")
            except discord.errors.NotFound:
                print("tried to add role user already has")

            await message.remove_reaction(emoji, user)


async def embedrolemessage():
        mf1 = getRoleEmojis()[0]
        mf2 = getRoleEmojis()[1]
        mf3 = getRoleEmojis()[2]
        mf4 = getRoleEmojis()[3]
        mf5 = getRoleEmojis()[4]

        rulesText = "{} **1st Year**".format(mf1) + "\n" + "{} **Second Year**".format(mf2) + "\n" + "{} **Third Year**".format(mf3) + "\n" + "{} **Fourth Year**".format(mf4) + "\n" + "{} **Alumni / Other**".format(mf5)

        rulesChannel = discord.utils.get(
            client.get_all_channels(), name="bot-react-post-test")
        # await rulesChannel.purge()
        staticRoles = discord.Embed(
            title="Class Roles", description="**React below to select your class/year!**" + "\n" + "\n" + rulesText)
        await rulesChannel.send(embed=staticRoles)

        static_id = rulesChannel.last_message_id

        cr1 = getRoleEmojis()[5]
        cr2 = getRoleEmojis()[6]
        cr3 = getRoleEmojis()[7]
        cr4 = getRoleEmojis()[8]

        dynamicRoles = discord.Embed(
            title="Club Roles", description="**React to receive a role that you are interested in!**")
        dynamicRoles.add_field(
            name="{} Offense (CPTC)".format(cr1), value="For people interested in offensive security and or penetration testing. This is also for people interested in our CPTC competition team!", inline=False)
        dynamicRoles.add_field(
            name="{} Defense (CCDC)".format(cr2), value="For people interested in defensive security and or security monitoring. This is also for people interested in our CCDC competition team!", inline=False)
        dynamicRoles.add_field(
            name="{} CTF".format(cr3), value="For people interested in competing in CTFs such as NCL and PicoCTF. This is also for people interested in other platforms such as Hack the box, TryHackMe, and Blue team labs online!", inline=False)
        dynamicRoles.add_field(
            name="{} Gaming".format(cr4), value="For people interested in hanging out with club members and playing video games. Some of the games include Minecraft, Among Us, and Escape From Tarkov!", inline=False)

        await rulesChannel.send(embed=dynamicRoles)

        dynamic_id = rulesChannel.last_message_id

        # function that adds reactions if the message is deleted
        # get the messages, get the ids
        # IS AN INT NOT A STRING HAHAHAHA
        rulesChanId = 878313648618098709
        print(static_id)
        print(dynamic_id)

        staticMessage = await client.get_channel(rulesChanId).fetch_message(static_id)
        dynamicMessage = await client.get_channel(rulesChanId).fetch_message(dynamic_id)

        await addReactionsToMessage(staticMessage, dynamicMessage)

