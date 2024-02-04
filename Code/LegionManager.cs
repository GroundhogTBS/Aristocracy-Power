using System.Collections.Generic;
using AristocracyAndPower;
namespace AristocracyAndPower;

public class LegionManager : UnitGroupManager
{
    public float timer_check_military=5f;
    public float interval_check_military=5f;
    public void update(float pElapsed)
	{
        base.update(pElapsed);
		if (this.timer_check_military > 0f)
		{
			this.timer_check_military -= pElapsed;
			return;
		}
		this.timer_check_military = this.interval_check_military;
		int i = 0;
		// while (i < this.groups.Count)
		// {
		// 	UnitGroup unitGroup = this.groups[i];
		// 	unitGroup.update(pElapsed);
		// 	if (!unitGroup.city.isAlive())
		// 	{
		// 		unitGroup.disband();
		// 		this.groups.RemoveAt(i);
		// 	}
		// 	else
		// 	{
		// 		i++;
		// 	}
		// }
	}

	public UnitGroup createNewGroup(City pCity)
	{
		UnitGroup unitGroup = new UnitGroup(pCity);
		UnitGroup unitGroup2 = unitGroup;
		int num = this.last_id;
		this.last_id = num + 1;
		unitGroup2.id = num;
		this.groups.Add(unitGroup);
		return unitGroup;
	}

	public void clear()
	{
		foreach (UnitGroup unitGroup in this.groups)
		{
			unitGroup.clear();
			unitGroup.Dispose();
		}
		this.groups.Clear();
	}
}