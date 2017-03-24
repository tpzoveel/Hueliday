using System;
namespace Hueliday
{
	public class HolidaySchedule : Schedule
	{
		public string SetName(string newName)
		{
			this.Name = Config.NamePrefix + newName;
			return this.Name;
		}

	}
}
