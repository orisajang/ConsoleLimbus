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
        Dictionary<(eItemGrade, eItemType), List<Item>> itemPools = new Dictionary<(eItemGrade, eItemType), List<Item>>();
        
        public Gacha()
        {
            InitGachaItem();
        }
        public void InitGachaItem()
        {
            //수동적으로 Grade와 장비Type을 선택해서 추가
            //개선 필요할듯.. 수동으로 값 입력하는건 장비 커스텀 설정상 어쩔수없을거같긴한데 찾아봐야함
            //Init에서 Weapon이나 Armor가 하나라도 없으면 에러발생(좋은 에러인듯 터지는게 맞음)
            //Json데이터를 읽어오기?
            InitGachaHelper(eItemGrade.C, eItemType.Weapon, "녹슨검", 1);
            InitGachaHelper(eItemGrade.C, eItemType.Weapon, "나무검", 1);
            InitGachaHelper(eItemGrade.C, eItemType.Armor, "녹슨갑옷", 1);
            InitGachaHelper(eItemGrade.C, eItemType.Armor, "나무갑옷", 1);
            InitGachaHelper(eItemGrade.C, eItemType.Accessory, "낡은반지", 1);
            InitGachaHelper(eItemGrade.C, eItemType.Accessory, "낡은목걸이", 1);
            InitGachaHelper(eItemGrade.B, eItemType.Weapon, "무딘철검", 3);
            InitGachaHelper(eItemGrade.B, eItemType.Weapon, "동검", 2);
            InitGachaHelper(eItemGrade.B, eItemType.Armor, "낡은갑옷", 2);
            InitGachaHelper(eItemGrade.B, eItemType.Armor, "두꺼운나무갑옷", 2);
            InitGachaHelper(eItemGrade.B, eItemType.Accessory, "동반지", 2);
            InitGachaHelper(eItemGrade.B, eItemType.Accessory, "낡은은반지", 3);
            InitGachaHelper(eItemGrade.A, eItemType.Weapon, "철검", 4);
            InitGachaHelper(eItemGrade.A, eItemType.Armor, "철갑옷", 4);
            InitGachaHelper(eItemGrade.A, eItemType.Accessory, "철반지", 4);
            InitGachaHelper(eItemGrade.S, eItemType.Weapon, "수호자의검", 5);
            InitGachaHelper(eItemGrade.S, eItemType.Weapon, "공격자의검", 5);
            InitGachaHelper(eItemGrade.S, eItemType.Armor, "수호자의갑옷", 5);
            InitGachaHelper(eItemGrade.S, eItemType.Armor, "공격자의갑옷", 5);
            InitGachaHelper(eItemGrade.S, eItemType.Accessory, "수호자의반지", 5);
            InitGachaHelper(eItemGrade.SS, eItemType.Weapon, "삼위일체", 6);
            InitGachaHelper(eItemGrade.SS, eItemType.Weapon, "무한의대검", 6);
            InitGachaHelper(eItemGrade.SS, eItemType.Armor, "가시갑옷", 6);
            InitGachaHelper(eItemGrade.SS, eItemType.Accessory, "무효화반지", 6);
        }

        public void InitGachaHelper(eItemGrade itemGrade, eItemType itemType, string name, int value) 
        {
            //등급, 장비종류별로 아이템 추가할때 사용하는 함수
            if(!itemPools.ContainsKey((itemGrade,itemType)))
            {
                itemPools[(itemGrade, itemType)] = new List<Item>();
            }
            ItemFactory itemFactory = new ItemFactory();
            Item itemBuf = itemFactory.Create(itemType, itemGrade, name, value);
            itemPools[(itemGrade, itemType)].Add(itemBuf);
        }

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
                //랜덤으로 enum중에 하나를 골라야함
                eItemGrade itemGradeBuf = GetWeightRandomGrade(gachaPercentDic);
                eItemType itemTypeBuf = GetRandom<eItemType>();
                
                int itemCount = itemPools[(itemGradeBuf, itemTypeBuf)].Count;
                int rndGradeTypeIndex = rnd.Next(0, itemCount);
                Item itemFromPool = itemPools[(itemGradeBuf, itemTypeBuf)][rndGradeTypeIndex];
                ItemFactory itemFactory = new ItemFactory();
                Item itemBuf = itemFactory.Create(itemTypeBuf, itemGradeBuf, itemFromPool.name, itemFromPool.value);

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
        public Item Create(eItemType itemType, eItemGrade itemGrade, string name, int value)
        {
            Item itembuf;
            switch(itemType)
            {
                case eItemType.Weapon:
                    itembuf = new WeaponItem(itemGrade,name,value);
                    break;
                case eItemType.Armor:
                    itembuf = new ArmorItem(itemGrade, name, value);
                    break;
                case eItemType.Accessory:
                    itembuf = new Accessory(itemGrade, name, value);
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
        public string name { get; }
        public int value { get; }

        public Item(eItemGrade _itemGrade,string _name, int _value)
        {
            itemGrade = _itemGrade;
            name = _name;
            value = _value;
        }

    }
    class WeaponItem:Item
    {
        public override eItemType itemType => eItemType.Weapon;
        public WeaponItem(eItemGrade _itemGrade, string _name, int _value) : base(_itemGrade, _name, _value) { }
    }
    class ArmorItem : Item
    {
        public override eItemType itemType => eItemType.Armor;
        public ArmorItem(eItemGrade _itemGrade, string _name, int _value) : base(_itemGrade, _name, _value) { }
    }
    class Accessory : Item
    {
        public override eItemType itemType => eItemType.Accessory;
        public Accessory(eItemGrade _itemGrade, string _name, int _value) : base(_itemGrade, _name, _value) { }
    }

    class Inventory
    {
        //Dictionary<Item, int> inventoryDic = new Dictionary<Item, int>();
        //딕셔너리에 튜플넣어서 해보세요
        Dictionary<(eItemGrade, eItemType), int> inventoryDic = new Dictionary<(eItemGrade, eItemType), int>();
        List<Item> itemList = new List<Item>();

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

        public Item GetItem(int index)
        {
            if (index >= itemList.Count)
            {
                Console.WriteLine($"인덱스 초과! {itemList.Count}개 까지만 가능합니다");
                return null;
            }


            return itemList[index];
        }

        public void AddItems(List<Item> items)
        {
            foreach(var kv in items)
            {
                var key = (kv.itemGrade, kv.itemType);

                //if (inventoryDic.ContainsKey(key)) //코드 변경해서init에서 무조건 키값 다 추가해서 ContainsKey필요없음.
                {
                    inventoryDic[key]++; //전체 갯수가 몇개인지 확인하기 위한 딕셔너리
                }
                itemList.Add(kv); //상세 아이템을 가지고 있기 위한 리스트
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
