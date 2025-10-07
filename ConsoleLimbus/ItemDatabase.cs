using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLimbus
{
    class ItemDatabase
    {
        //모든 아이템에 대한 정보를 가지고있는 데이터베이스 클래스 -> 스태틱 대신 싱글톤?
        private static ItemDatabase _instance;
        public static ItemDatabase Instance
        {
            get
            {
                if (_instance == null) _instance = new ItemDatabase();
                return _instance;
            }
        }

        public Dictionary<string, Item> itemAllInfoDic { get; } //readOnly는 안될듯??
        ItemFactory itemFactory = new ItemFactory();

        private ItemDatabase()  //싱글톤에서 private 막아놔야함(혼동될수있기때문에)
        {
            itemAllInfoDic = new Dictionary<string, Item>();
            InitDictionary();
        }
        public Item GetItem(string name)
        {
            Item itemBuf = itemAllInfoDic[name];
            Item itemReturnBuf = itemFactory.Create(itemBuf.itemType, itemBuf.itemGrade, itemBuf.name, itemBuf.value);
            return itemReturnBuf;
        }

        public void InitDictionary()
        {
            //Json데이터를 읽어올 수 있게? 처리하는 방법도 있다고함
            AddItem(eItemGrade.C, eItemType.Weapon, "녹슨검", 1);
            AddItem(eItemGrade.C, eItemType.Weapon, "나무검", 1);
            AddItem(eItemGrade.C, eItemType.Armor, "녹슨갑옷", 1);
            AddItem(eItemGrade.C, eItemType.Armor, "나무갑옷", 1);
            AddItem(eItemGrade.C, eItemType.Accessory, "낡은반지", 1);
            AddItem(eItemGrade.C, eItemType.Accessory, "낡은목걸이", 1);
            AddItem(eItemGrade.B, eItemType.Weapon, "무딘철검", 3);
            AddItem(eItemGrade.B, eItemType.Weapon, "동검", 2);
            AddItem(eItemGrade.B, eItemType.Armor, "낡은갑옷", 2);
            AddItem(eItemGrade.B, eItemType.Armor, "두꺼운나무갑옷", 2);
            AddItem(eItemGrade.B, eItemType.Accessory, "동반지", 2);
            AddItem(eItemGrade.B, eItemType.Accessory, "낡은은반지", 3);
            AddItem(eItemGrade.A, eItemType.Weapon, "철검", 4);
            AddItem(eItemGrade.A, eItemType.Armor, "철갑옷", 4);
            AddItem(eItemGrade.A, eItemType.Accessory, "철반지", 4);
            AddItem(eItemGrade.S, eItemType.Weapon, "수호자의검", 5);
            AddItem(eItemGrade.S, eItemType.Weapon, "공격자의검", 5);
            AddItem(eItemGrade.S, eItemType.Armor, "수호자의갑옷", 5);
            AddItem(eItemGrade.S, eItemType.Armor, "공격자의갑옷", 5);
            AddItem(eItemGrade.S, eItemType.Accessory, "수호자의반지", 5);
            AddItem(eItemGrade.SS, eItemType.Weapon, "삼위일체", 6);
            AddItem(eItemGrade.SS, eItemType.Weapon, "무한의대검", 6);
            AddItem(eItemGrade.SS, eItemType.Armor, "가시갑옷", 6);
            AddItem(eItemGrade.SS, eItemType.Accessory, "무효화반지", 6);
        }

        private void AddItem(eItemGrade itemGrade, eItemType itemType, string name, int value)
        {
            Item itemBuf = itemFactory.Create(itemType, itemGrade, name, value);
            itemAllInfoDic[itemBuf.name] = itemBuf;
        }
    }
}
