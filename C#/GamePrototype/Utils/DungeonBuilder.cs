using GamePrototype.Dungeon;
using GamePrototype.Items.EconomicItems;

namespace GamePrototype.Utils
{
    public static class DungeonBuilder
    {
        public static DungeonRoom BuildEasyDungeon()
        {
            var enter = new DungeonRoom(GameConstants.Enter);
            var monsterRoom = new DungeonRoom(GameConstants.Monster, UnitFactoryDemo.CreateGoblinEnemy());
            var emptyRoom = new DungeonRoom(GameConstants.Empty);
            var lootRoom = new DungeonRoom(GameConstants.Loot, new Gold());
            var lootStoneRoom = new DungeonRoom(GameConstants.Loot, new Grindstone("Stone"));
            var finalRoom = new DungeonRoom(GameConstants.Final, new Grindstone("Stone1"));

            enter.TrySetDirection(Direction.Right, monsterRoom);
            enter.TrySetDirection(Direction.Left, emptyRoom);

            monsterRoom.TrySetDirection(Direction.Forward, lootRoom);
            monsterRoom.TrySetDirection(Direction.Left, emptyRoom);

            emptyRoom.TrySetDirection(Direction.Forward, lootStoneRoom);

            lootRoom.TrySetDirection(Direction.Forward, finalRoom);
            lootStoneRoom.TrySetDirection(Direction.Forward, finalRoom);

            return enter;
        }

        public static DungeonRoom BuildHardDungeon()
        {
            var enter = new DungeonRoom(GameConstants.Enter);
            var monsterRoom1 = new DungeonRoom(GameConstants.Monster, UnitFactoryDemo.CreateEliteGoblinEnemy());
            var monsterRoom2 = new DungeonRoom(GameConstants.Monster, UnitFactoryDemo.CreateGoblinEnemy());
            var emptyRoom = new DungeonRoom(GameConstants.Empty);
            var lootRoom1 = new DungeonRoom(GameConstants.Loot, new Gold());
            var lootRoom2 = new DungeonRoom(GameConstants.Loot, new Grindstone("Magic Stone"));
            var lootRoom3 = new DungeonRoom(GameConstants.Loot, new Gold());
            var finalBossRoom = new DungeonRoom(GameConstants.Final, UnitFactoryDemo.CreateEliteGoblinEnemy());

            enter.TrySetDirection(Direction.Right, monsterRoom1);
            enter.TrySetDirection(Direction.Left, monsterRoom2);
            enter.TrySetDirection(Direction.Forward, emptyRoom);

            monsterRoom1.TrySetDirection(Direction.Forward, lootRoom1);
            monsterRoom1.TrySetDirection(Direction.Right, lootRoom2);

            monsterRoom2.TrySetDirection(Direction.Forward, lootRoom3);
            monsterRoom2.TrySetDirection(Direction.Left, emptyRoom);

            lootRoom1.TrySetDirection(Direction.Forward, finalBossRoom);
            lootRoom2.TrySetDirection(Direction.Forward, finalBossRoom);
            lootRoom3.TrySetDirection(Direction.Forward, finalBossRoom);
            emptyRoom.TrySetDirection(Direction.Forward, finalBossRoom);

            return enter;
        }
    }
}
