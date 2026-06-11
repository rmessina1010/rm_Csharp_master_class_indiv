using ApiData;

namespace SWLogic{ 
    public static class PerformSWLogic{

            public static (string Name, int Val, string UnitName) getMinMaxSW(string prop, StarWarsRow[] data, bool isMax = true){
            Func<StarWarsRow, int>? selector = prop switch
            {
                "surface_water"   => planet => planet.surface_water,
                "population"      => planet => planet.population,
                "diameter"        => planet => planet.diameter,
                "rotation_period" => planet => planet.rotation_period,
                _                 => null
            };

            if (selector == null || data == null || data.Length == 0) { return ("No Planet", -2, ""); }        
                StarWarsRow selected = isMax ? 
                        data.MaxBy(selector) 
                        : data.MinBy(selector);
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