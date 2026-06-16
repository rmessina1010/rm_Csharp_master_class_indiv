using ApiData;

namespace SWLogic{ 
    using PlanetStats = (string Name, int Val, string UnitName);
    public static class PerformSWLogic{

            public static PlanetStats getMinMaxSW(string prop, StarWarsRow[] data, bool isMax = true){
            var  selectors = new Dictionary <string, Func<StarWarsRow, int>>{
                {"surface_water"   , planet => planet.surface_water},
                {"population"      , planet => planet.population},
                {"diameter"        , planet => planet.diameter},
                {"rotation_period" , planet => planet.rotation_period}
            };

            if (!selectors.ContainsKey(prop) || data == null || data.Length == 0) { return ("No Planet", -2, ""); }        
                StarWarsRow selected = isMax ? 
                        data.MaxBy(selectors[prop]) 
                        : data.MinBy(selectors[prop]);
            return prop switch            
            {
                "surface_water"   => ( selected.name, selected.surface_water, "% coverage"),
                "population"      => (  selected.name,  selected.population, "people"),
                "diameter"        => ( selected.name, selected.diameter, "kilometers"),
                "rotation_period" => (  selected.name, selected.rotation_period, "hours"),
                _                 => ("No Planet", -2, "")
            };                    
        }
    }
}