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
    webhookUrl = "https://discord.com/api/webhooks/877404142178557963/LduzUUPzaxzVCJV2u5uTPzkH4-aapzWHI_1GEuZj4KKMJRxkxOXXrnM5YGt2UyFnvbx3"
    return webhookUrl

#add more as needed 