using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLimbus
{
    abstract class Character
    {
        protected string name;
        private int maxHP;
        protected int currentHp;

        protected int mentality;
        protected int stagger;
        protected int speed;
        protected int level;
        protected int money;
        protected SkillManager characterSkill = new SkillManager();
        protected Equipment equipment = new Equipment(); //모든 캐릭터는 장비를 가지고있음

        public int Mentality => mentality;
        public int CurrentHp => currentHp;
        public int MaxHp => maxHP;
        public string Name => name;
        public int Level => level;
        public Equipment Equip { get { return equipment; } }
        public SkillManager CharacterSkill => characterSkill;
        public Character(string _name, int _maxHp, int _level)
        {
            name = _name;
            maxHP = _maxHp + (_level * 2);  //1레벨 hp + level * hp상승량
            currentHp = maxHP; //초기 hp량
            mentality = 0; //초기 0
        }
        public void TakeDamage(int damage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            currentHp = Math.Max(0, currentHp - damage); //체력이 0이하로 떨어지지않게
            Console.WriteLine($"{Name}가 {damage}데미지 받음! 남은 hp:{currentHp}");
            Console.ResetColor();
        }
        public void Healing(int amount)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            int sumHpValue = currentHp + amount;
            currentHp = (sumHpValue > maxHP) ? maxHP : sumHpValue; //MaxHp보다 더한값이 크다면 max값 적용
            Console.WriteLine($"{Name}가 {amount}힐 받음! 남은 hp:{currentHp}");
            Console.ResetColor();
        }

        public void SetPlayerSkills(SkillManager sm)
        {
            characterSkill = sm;
        }

        public void SetLevel(int value)
        {
            level += value;
            if (level < 0) level = 0;
        }
        public void SetMaxHP(Item item) //장신구가 바뀌었을때 최대HP 갱신을 위해서 사용
        {
            Item itemAccessory = equipment.GetEquipment(eItemType.Accessory);
            maxHP -= itemAccessory.value;
            maxHP += item.value;
            if (maxHP < 0) maxHP = 1;
            if (maxHP < currentHp) currentHp = maxHP;
        } 
    }
    class Enemy : Character
    {
        public Enemy(string _name, int _maxHp, int _level) : base(_name, _maxHp, _level)
        {
        }
    }
    class Player : Character
    {
        public int Money { get { return money; } }
        Inventory inventory = new Inventory();

        public Player(string _name, int _maxHp, int _level) : base(_name, _maxHp, _level)
        {
            money = 10000; //초기 돈
        }
        public bool SetMoney(int value)
        {
            if (money + value < 0)
            {
                Console.WriteLine("금액부족!!");
                return false; //음수인경우 false
            }
            money += value;
            return true;
        }
        public void SetName(string _name)
        {
            name = _name;
        }
        public void AddInventoryItem(Item _item) //메서드 오버로딩1
        {
            ItemFactory itemFactory = new ItemFactory();
            Item itemBuf = itemFactory.Create(_item.itemType, _item.itemGrade, _item.name, _item.value);
            List<Item> itemListBuf = new List<Item>();
            itemListBuf.Add(itemBuf);
            inventory.AddItems(itemListBuf);
        }
        public void AddInventoryItem(List<Item> _items) //메서드 오버로딩2
        {
            inventory.AddItems(_items); //이 함수 쓰기전에 _items가 얕은복사여도 괜찮은지 확인해야함
        }
        public void DeleteInventoryItem(Item _item)
        {
            inventory.DeleteItem(_item);
        }
        public bool IsPlayerHaveItem(Item _item)
        {
            return inventory.HaveItem(_item);
        }
        public Item GetInventoryItem(string _name)
        {
            return inventory.GetItem(_name);
        }
        public Item GetInventoryItem(int index)
        {
            return inventory.GetItem(index);
        }
        public void PrintInventoryItem()
        {
            inventory.PrintInventoryBuf();
        }

    }
}
