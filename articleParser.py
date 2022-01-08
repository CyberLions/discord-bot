import discord
import feedparser
from discord import webhook as hook


'''--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------'''


def postDarkReadingArticle(enNum):

    dReadLink = "https://www.darkreading.com/rss.xml"  # NEW SOURCE

    dReadArticle = feedparser.parse(dReadLink)

    dReadEmbedArticle = discord.Embed(

        title=dReadArticle.entries[enNum].title,
        # Can't find image within feed or entries for Dark reading
        image_url='https://img.deusm.com/darkreading/dr_staff_125x125.jpg',
        url=dReadArticle.entries[enNum].link,
        description=dReadArticle.entries[enNum].summary,
        color=1

    )

    dReadEmbedArticle.set_footer(text=dReadArticle.entries[enNum].published)
    dReadEmbedArticle.set_author(name=dReadArticle.feed.title)
    dReadEmbedArticle.add_field(
        name="Author", value=dReadArticle.entries[enNum].author, inline=True)

# ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    postArticle(dReadEmbedArticle)

    print(dReadArticle.entries[enNum])

    print(len(dReadArticle.entries))  # 100 Entries (Articles)

# postDarkReadingArticle(74)


'''--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------'''


def postAttackBreachDarkReadingArticles():

    dReadLink = "https://www.darkreading.com/rss.xml"  # NEW SOURCE

    attackbreachDReadArticle = feedparser.parse(dReadLink)

    i = 0
    articleList = []
    while i < len(dReadArticle.entries):
        print("Checking entry(article) " + str(i + 1) + " out of " +
              str(len(dReadArticle.entries)) + " within Attacks / Breaches category")
        if 'attacks-breaches' in dReadArticle.entries[i].link:
            articleList.append(i)
            print("Article " + str(i + 1) + " is related to category")
        i += 1

    print("Total amount of articles found: " + str(len(articleList)))

# ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    j = 0
    while j < len(articleList):

        dReadAttackBreachEmbedArticle = discord.Embed(

            title=attackbreachDReadArticle.entries[articleList[j]].title,
            # Can't find image within feed or entries for Dark reading
            image_url='https://img.deusm.com/darkreading/dr_staff_125x125.jpg',
            url=attackbreachDReadArticle.entries[articleList[j]].link,
            description=attackbreachDReadArticle.entries[articleList[j]].summary,
            color=1

        )

        dReadAttackBreachEmbedArticle.set_footer(
            text=dReadArticle.entries[articleList[j]].published)

        dReadAttackBreachEmbedArticle.set_author(name=dReadArticle.feed.title)

        try:
            dReadAttackBreachEmbedArticle.add_field(
                name="Author", value=dReadArticle.entries[articleList[j]].author, inline=True)
        except:
            print("No Author Found for article " + str(j + 1))
            dReadAttackBreachEmbedArticle.add_field(
                name="No Author Found", value="*blank*", inline=True)

# ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        # time.sleep(.5)
        postArticle(dReadAttackBreachEmbedArticle)

        print("Article " + str(j + 1) +
              " was posted out of " + str(len(articleList)))

        j += 1


postAttackBreachDarkReadingArticles()

'''--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------'''


def postKrebsOnSecurityArticle(enNum):

    # Make custom scrapper to find first image (html or something) within entry summary
    krebLink = "https://krebsonsecurity.com/feed/"

    krebArticle = feedparser.parse(krebLink)

    krebEmbedArticle = discord.Embed(

        title=krebArticle.entries[enNum].title,
        # Might have to do with entries.content
        image_url='https://krebsonsecurity.com/wp-content/uploads/2021/03/kos-27-03-2021.jpg',
        url=krebArticle.entries[enNum].link,
        description=krebArticle.entries[enNum].summary,
        color=9936031

    )

    krebEmbedArticle.set_footer(text=krebArticle.entries[enNum].published)
    krebEmbedArticle.set_author(name=krebArticle.feed.title)
    krebEmbedArticle.add_field(name="Author", value='Brian Krebs', inline=True)
# ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    postArticle(krebEmbedArticle)

    print(krebArticle.entries[enNum])

    print(len(krebArticle.entries))  # 10 Entries (Articles)

# postKrebsOnSecurityArticle(0)


def postArticle(Article):

    hook.send(embed=Article)
