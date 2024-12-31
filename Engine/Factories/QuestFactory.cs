using Engine.Models;

namespace Engine.Factories;

internal static class QuestFactory
{
    private static readonly List<Quest> _quests = new List<Quest>();

    static QuestFactory()
    {
        //items needed to complete the quest
        List<ItemQuantity> itemsToComplete = new List<ItemQuantity>();
        List<ItemQuantity> rewardItems = new List<ItemQuantity>();
        itemsToComplete.Add(new ItemQuantity(300, 5));
        itemsToComplete.Add(new ItemQuantity(110, 1));

        _quests.Add(new Quest(1,
                              "Just like them",
                              "Kill 5 bandits and obtain a Bronze Axe",
                              itemsToComplete,
                              25,
                              10,
                              rewardItems));
    }

    internal static Quest GetQuestById(int id)
    {
        return _quests.FirstOrDefault(quest => quest.ID == id) ?? throw new Exception("That quest doesn't exist");
    }
}