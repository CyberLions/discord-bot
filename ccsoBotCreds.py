import os

# *API and Token*: Used to pull the strings of the API Link / Bot Token. DO NOT PUT ON GITHUB. Share it with developers and hosting sources.

# ------------------------------------------------------------------------------------------------------------


def getDiscordKey():
    discordKey = "ODUyNzI0MjcxOTk3Nzc5OTg4.YMK_XQ.22iiYdwZf5gn8ZFEWfyTOS3HIUU"
    # is the api key expired? whack 

    discordKey = 'ODc1MzAzMTE5NjgzMTk0ODgw.YRTjjg.KGKrfQXXAtdu0AVMQHj0F6Rgbio'
    # second key 

    return discordKey

def getGCPKey():
    gcpKey = os.environ.get('GCP_KEY')
    return gcpKey 

def getWebhookUrl():
    webhookUrl = "https://discord.com/api/webhooks/875255639385985044/2NUyLhV5G7ZDcz_ILdkzsywI8avp04OmX7DB9MtOLx_irXfUi0NYjo0WZ8B1E1j0M-Ww"
    return webhookUrl

#add more as needed 