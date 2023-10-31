namespace CommandersCall.Models.Players
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CommandersCall.Models.Resources;

	public class Commander
	{
		public long Currency { get; set; }
		public long Food { get; set; }
		public long Equipment { get; set; }
		public DateTime CreationDate { get; private set; }
		public string CallSign { get; private set; }

		private ICollection<City> _citiesOwned;
		public ICollection<City> CitiesOwned
		{
			get
			{
				if (_citiesOwned == null)
					_citiesOwned = new List<City>();

				return _citiesOwned;
			}
			set
			{
				_citiesOwned = value;
			}
		}

		public Commander(long currency, long food, long equipment, DateTime creationDate, string callsign, IEnumerable<City> cities)
		{
			Currency = currency;
			Food = food;
			Equipment = equipment;
			CreationDate = creationDate;
			CallSign = callsign;
			_citiesOwned = CitiesOwned = _citiesOwned?.Concat(cities).ToList() ?? Enumerable.Empty<City>().ToList();
		}
	}
}
