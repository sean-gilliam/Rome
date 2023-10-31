namespace CommandersCall.Models.Resources
{
	using System.Collections.Generic;
	using CommandersCall.Models.Units;

	public class City
	{
		public ICollection<CombatUnit> Units { get; private set; }
		public string Name { get; private set; }
		public long FoodPerTick { get; private set; }
		public long EquipmentPerTick { get; private set; }
		public long CurrencyPerTick { get; private set; }

		public City(string name, ICollection<CombatUnit> units, long food, long equipment, long currency)
		{
			Name = name;
			Units = units;
			FoodPerTick = food;
			EquipmentPerTick = equipment;
			CurrencyPerTick = currency;
		}
	}
}
