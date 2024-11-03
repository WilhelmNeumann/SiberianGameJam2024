using System.Collections.Generic;
using System.Linq;

namespace Quests
{
    public class Location
    {
        public int ID;
        public string Name;
        public LocationState State;

        public void SetState(LocationState state) => State = state;

        public static Location GetById(int id) => Location.Locations.First(x => x.ID == id);

        public static readonly List<Location> Locations = new()
        {
            new Location { ID = 1, Name = "Болото", State = LocationState.Neutral },
            new Location { ID = 2, Name = "Мельница", State = LocationState.Neutral }
        };
    }

    public enum LocationState
    {
        Neutral,
        Good,
        Bad
    }
}