using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLimbus
{
    enum eItemGrade
    {
        C,B,A,S,SS
    }
    enum eItemType
    {
        Weapon,Armor,Accessory
    }
    class Gacha
    {
        static Random rnd = new Random();
        Dictionary<eItemGrade, int> gachaPercentDic = new Dictionary<eItemGrade, int>
        {
            {eItemGrade.C,60 },
            {eItemGrade.B,20 },
            {eItemGrade.A,10 },
            {eItemGrade.S,7 },
            {eItemGrade.SS,3 }
        };
        //가챠 시작
        public List<Item> DoGacha(int count)
        {
            List<Item> itemList = new List<Item>();
            Console.WriteLine("가챠 시작");
            for(int i=0; i< count; i++)
            {
                //Item item = new Item();
                //랜덤으로 enum중에 하나를 골라야함
                eItemGrade itemGradeBuf = GetWeightRandomGrade(gachaPercentDic);
                eItemType itemTypeBuf = GetRandom<eItemType>();
                ItemFactory itemFactory = new ItemFactory();
                Item itemBuf = itemFactory.Create(itemTypeBuf,itemGradeBuf);
                itemList.Add(itemBuf);
            }
            Console.WriteLine("가챠 끝");
            return itemList;
        }
        public static eItemGrade GetWeightRandomGrade(Dictionary<eItemGrade,int> dic)
        {
            int total = dic.Values.Sum();
            int randomValue = rnd.Next(0, total);

            int cumulative = 0;
            foreach(var kvp in dic)
            {
                cumulative += kvp.Value;
                if (randomValue < cumulative) return kvp.Key;
            }
            return dic.Keys.First();
        }
        public static T GetRandom<T>() where T : Enum
        {
            var item = Enum.GetValues(typeof(T));
            return (T)item.GetValue(rnd.Next(item.Length));
        }
    }

    class ItemFactory
    {
        public Item Create(eItemType itemType, eItemGrade itemGrade)
        {
            Item itembuf;
            switch(itemType)
            {
                case eItemType.Weapon:
                    itembuf = new WeaponItem(itemGrade);
                    break;
                case eItemType.Armor:
                    itembuf = new ArmorItem(itemGrade);
                    break;
                case eItemType.Accessory:
                    itembuf = new Accessory(itemGrade);
                    break;
                default:
                    itembuf = null;
                    break;
            }
            return itembuf;
        }
    }

    abstract class Item
    {
        public eItemGrade itemGrade { get; private set; }
        public abstract eItemType itemType { get; }
        string name;
        int value;

        public Item(eItemGrade _itemGrade)
        {
            itemGrade = _itemGrade;
        }

    }
    class WeaponItem:Item
    {
        public override eItemType itemType => eItemType.Weapon;
        public WeaponItem(eItemGrade _itemGrade) : base(_itemGrade) { }
    }
    class ArmorItem : Item
    {
        public override eItemType itemType => eItemType.Armor;
        public ArmorItem(eItemGrade _itemGrade) : base(_itemGrade) { }
    }
    class Accessory : Item
    {
        public override eItemType itemType => eItemType.Accessory;
        public Accessory(eItemGrade _itemGrade) : base(_itemGrade) { }
    }

    class Inventory
    {
        //Dictionary<Item, int> inventoryDic = new Dictionary<Item, int>();
        //딕셔너리에 튜플넣어서 해보세요
        Dictionary<(eItemGrade, eItemType), int> inventoryDic = new Dictionary<(eItemGrade, eItemType), int>();

        public void AddItems(List<Item> items)
        {
            foreach(var kv in items)
            {
                var key = (kv.itemGrade, kv.itemType);

                if (inventoryDic.ContainsKey(key))
                {
                    inventoryDic[key]++;
                }
                else
                {
                    inventoryDic.Add(key,1);
                    //inventoryDic.Add(key, 1);
                }
            }
        }
        public void PrintInventory()
        {
            Console.WriteLine("===인벤토리 목록===");
            foreach(var kv in inventoryDic)
            {
                Console.WriteLine($"아이템등급:{kv.Key.Item1} 아이템타입:{kv.Key.Item2} 갯수: {kv.Value}");
            }
        }
    }
}
