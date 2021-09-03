import os

# *API and Token*: Used to pull the strings of the API Link / Bot Token. DO NOT PUT ON GITHUB. Share it with developers and hosting sources.
#This has been changed and renamed
# ------------------------------------------------------------------------------------------------------------


def getDiscordKey():
    discordKey = "INSERT DISCORD KEY HERE"
    discordKey = os.environ.get('DISC_KEY')
    return discordKey

def getGCPKey():
    gcpKey = os.environ.get('GCP_KEY')
    return gcpKey 

def getWebhookUrl():
    webhookUrl = "INSERT WEBHOOK KEY HERE"
    webhookUrl = os.environ.get('WHOOK')
    return webhookUrl