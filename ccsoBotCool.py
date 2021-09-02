import discord
import random

'''TODO add functionality in main
    Have first command be !pop and the next following be !smoke 
    maybe post a gif 
    flag = w00_b4ck_b4by'''
async def youCannotSayPopAndForgetTheSmoke(message):
        popSmokeSongs = ["https://www.youtube.com/watch?v=AzQJO6AyfaQ", # Coupe
                         "https://www.youtube.com/watch?v=Q9pjm4cNsfc", # For the night 
                         "https://www.youtube.com/watch?v=uuodbSVO3z0", # What u know bout love <3
                         "https://www.youtube.com/watch?v=usu0XY4QNB0", # Welcome to the party
                         "https://www.youtube.com/watch?v=kx7P_ENnDPE", # Gatti
                         "https://www.youtube.com/watch?v=wP1PpQT4oC8", # Get Back grrrt
                         "https://www.youtube.com/watch?v=oorVWW9ywG0", # Dior
                         "https://www.youtube.com/watch?v=EZkNUmVXg6U", # Element
                         "https://www.youtube.com/watch?v=Yr2Nq-7mQoY", # AP SPICY I BUST A CHECK ON MY NIKES
                         "https://www.youtube.com/watch?v=XLQ3O_SXw1I", # Ordinary
                         ]
        aSong = random.choice(popSmokeSongs)
        await message.channel.send("Woo back baby 💫💫" + "\n" + aSong)



