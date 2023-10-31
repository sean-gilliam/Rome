namespace CommandersCall.Core.Units
{
	public abstract class CombatUnit
	{
		private readonly double _strength;
		public double Strength { get { return _strength; } }

		private readonly double _morale;
		public double Morale { get { return _morale; } }

		private readonly double _defense;
		public double Defense { get { return _defense; } }

		protected CombatUnit(double strength, double morale, double defense)
		{
			_strength = strength;
			_morale = morale;
			_defense = defense;
		}
	}
}
