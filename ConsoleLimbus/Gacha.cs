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
            Console.WriteLine("======가챠 시작======");
            for(int i=0; i< count; i++)
            {
                //Item item = new Item();
                //랜덤으로 enum중에 하나를 골라야함
                eItemGrade itemGradeBuf = GetWeightRandomGrade(gachaPercentDic);
                eItemType itemTypeBuf = GetRandom<eItemType>();
                ItemFactory itemFactory = new ItemFactory();
                Item itemBuf = itemFactory.Create(itemTypeBuf,itemGradeBuf);
                PrintGachaItem(itemGradeBuf, itemTypeBuf); //뽑은 아이템 출력
                itemList.Add(itemBuf);
            }
            Console.WriteLine($"======가챠 끝 {count * 1000} 원을 사용했습니다======");
            return itemList;
        }
        public void PrintGachaItem(eItemGrade itemGrade, eItemType itemType)
        {
            if (itemGrade == eItemGrade.C) Console.ForegroundColor = ConsoleColor.White;
            else if (itemGrade == eItemGrade.B) Console.ForegroundColor = ConsoleColor.Blue;
            else if (itemGrade == eItemGrade.A) Console.ForegroundColor = ConsoleColor.Magenta;
            else if (itemGrade == eItemGrade.S) Console.ForegroundColor = ConsoleColor.Red;
            else if (itemGrade == eItemGrade.SS) Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{itemGrade}등급의 {itemType}을 뽑았습니다!!");
            Console.ResetColor();
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

        public Inventory()
        {
            InitDic(); //딕셔너리 초기화 (딕셔너리 내부 PrintInventory() 했을때 정렬되서 출력하기 위해)
        }
        public void InitDic()
        {
            var gradeArray = Enum.GetValues(typeof(eItemGrade));
            var typeArray = Enum.GetValues(typeof(eItemType));
            foreach (eItemGrade item in gradeArray)
            {
                foreach(eItemType item2 in typeArray)
                {
                    inventoryDic.Add((item, item2), 0);
                }
            }
        }


        public void AddItems(List<Item> items)
        {
            foreach(var kv in items)
            {
                var key = (kv.itemGrade, kv.itemType);

                //if (inventoryDic.ContainsKey(key)) //코드 변경해서init에서 무조건 키값 다 추가해서 ContainsKey필요없음.
                {
                    inventoryDic[key]++;
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
