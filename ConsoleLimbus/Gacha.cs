using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLimbus2
{
    class Gacha
    {
        //뽑기로 장비를 얻고 캐릭터가 장비를 장착. 초기 시작시 100뽑 지행
        Random rnd = new Random();

        Dictionary<eItemGrade, int> itemPickPercent = new Dictionary<eItemGrade, int>()
        {
            {eItemGrade.C,65 },
            {eItemGrade.B,20 },
            {eItemGrade.A,10 },
            {eItemGrade.S,4 },
            {eItemGrade.SS,1 }
        };

        public eItemGrade GetRandomGrade() //랜덤한 아이템 등급 얻기
        {
            int percent = rnd.Next(0, 100); //0~99;
            int sum = 0;

            foreach(var item in itemPickPercent)
            {
                sum += item.Value;
                if(percent < sum)
                {
                    return item.Key;
                }
            }

            throw new Exception("뭔가 잘못됨. 여기 오면 안됨");
            return eItemGrade.C;
        }
        public eEquipment GetRandomEquipType() //랜덤한 장비타입 얻기
        {
            //Weapon, Armor, Accessory중 하나를 얻음
            Array array = Enum.GetValues(typeof(eEquipment));  //Enum의 value를 배열형태로
            int randomNum = rnd.Next(0, array.Length); //랜덤숫자 0~ 3
            return (eEquipment)array.GetValue(randomNum);
        }

        public List<Item> DoGacha(int count)
        {
            List<Item> items = new List<Item>();
            //팩토리 패턴 만들어볼까? -> 등급과 아이템 타입이 주어지면 해당 값을 넣는 거

            //count가 뽑기 횟수
            Console.WriteLine("======뽑기 시작!======");
            for(int i=0; i< count; i++)
            {
                eEquipment eRandomEquipType = GetRandomEquipType();
                eItemGrade eRandomGrade = GetRandomGrade();

                if(eRandomGrade == eItemGrade.C)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (eRandomGrade == eItemGrade.B)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else if (eRandomGrade == eItemGrade.A)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                }
                else if (eRandomGrade == eItemGrade.S)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (eRandomGrade == eItemGrade.SS)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                Console.WriteLine($"{eRandomGrade}등급의 {eRandomEquipType}를 얻었습니다!!");
                Console.ResetColor();

                //인벤토리에 저장시키기
                Item itemBuf = MakeItem(eRandomEquipType, eRandomGrade);
                items.Add(itemBuf);
            }
            Console.WriteLine("======뽑기 종료!======");
            return items;
        }
        public Item MakeItem(eEquipment equipmentType, eItemGrade eItemGrade)
        {
            Item item;
            switch(equipmentType)
            {
                case eEquipment.Weapon:
                    item = new WeaponItem();
                    break;
                case eEquipment.Armor:
                    item = new ArmorItem();
                    break;
                case eEquipment.Accessory:
                    item = new AccessoryItem();
                    break;
                default:
                    item = null;
                    break;
            }
            return item;
        }
    }

    
    enum eItemGrade
    {
        C,B,A,S,SS
    }
    enum eEquipment
    {
        Weapon, Armor, Accessory
    }
    abstract class Item
    {
        public eItemGrade eGradeType; //아이템 등급
        int value; //효과. 아이템별로 다름

        public abstract eEquipment eEquipType { get; }
    }
    class WeaponItem :Item
    {
        public override eEquipment eEquipType { get; } = eEquipment.Weapon; //장비타입
    }
    class ArmorItem : Item
    {
        public override eEquipment eEquipType { get;} = eEquipment.Armor; //장비타입
    }
    class AccessoryItem : Item
    {
        public override eEquipment eEquipType { get; } = eEquipment.Accessory; //장비타입
    }
    struct StructTypeKey
    {
        eEquipment eEquipment;
        eItemGrade eItemGrade;
        public StructTypeKey(eEquipment equipment, eItemGrade itemGrade)
        {
            eEquipment = equipment;
            eItemGrade = itemGrade;
        }
    }
    interface InterfaceTypeKey
    {
        eEquipment eEquipment { get; set; }
        eItemGrade eItemGrade { get; set; }
    }
    abstract class ClassTypeKey
    {
        eEquipment eEquipment;
        eItemGrade eItemGrade;

        public ClassTypeKey(eEquipment equipment, eItemGrade itemGrade)
        {
            eEquipment = equipment;
            eItemGrade = itemGrade;
        }
    }

    class Inventory
    {
        List<Item> items = new List<Item>();
        Dictionary<Item, int> itemsDic = new Dictionary<Item, int>();
        Dictionary<StructTypeKey, int> itemDicStruct = new Dictionary<StructTypeKey, int>();

        public void AddItems(List<Item> itemList)  //해당메서드 제너릭으로 만들어보기
        {
            for(int i=0; i< itemList.Count; i++)
            {
                items.Add(itemList[i]);

                //▼테스트용
                Item itembuf = itemList[i];
                itemsDic[itembuf] += 1; //딕셔너리 테스트용
                StructTypeKey structTypeKey = new StructTypeKey(eEquipment.Armor,eItemGrade.A);
                //ClassTypeKey cls = new ClassTypeKey();
            }
        }
    }
    class CharacterEquipment
    {
        //캐릭터가 장착한 것에 대한 정보
        Dictionary<eEquipment, Item> equipMentDic = new Dictionary<eEquipment, Item>();
    }
}
