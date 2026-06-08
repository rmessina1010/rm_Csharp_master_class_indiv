

CSVTable myCSV = new CSVTable("sampleData.csv");
myCSV.Show(10);
public class CSVTable {

    private string[] _columns;
    private Dictionary<string, int > _schema;

    private List<string[]> _dataTable;

    private string _file;
    private string _path;

    public string File{
        get { return _file; }
        init { _file = value; }
    }
    public string Path{
        get  {return _path; }
        init  { _path = value; }
    }

    public CSVTable(string file, string path=""){
         _path = path;
         _file = file;
         Load();
    }

    public string ConvertTableToString( List<string[]> dataList){
        return string.Join(
            Environment.NewLine, 
            dataList.Select(row => string.Join(",", row))
            .Where(rowString => !string.IsNullOrEmpty(rowString))
            );    
    }

    
    public List<string[]> ConvertStringToTable(string dataString){
        return dataString
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(row => row.Split(','))
            .ToList();
    }

    public string[] GetRowOfString(int row){
       if (row<1  || row > _dataTable.Count() -1 ) {
        return new string[_columns.Count()];
       }
       return _dataTable[row];
    }

    public string GetCellValueString(int row, string col){
       if (row<1  || row > _dataTable.Count() -1 || !_schema.TryGetValue(col, out int value)) {
        return "";
       }
       return _dataTable[row][_schema[col]];
    }

    // public string[] GetRow(int row){
    // }
    // public string[] GetCell(int row, string col){
    // }

    public void Load(){
         string filedata = FileHandler.ReadFile(_file, _path);
         if (!string.IsNullOrEmpty(filedata)){
            _dataTable = ConvertStringToTable(filedata);
         }
    }

    public void Save(){

    }

    public void Show(int limit= -1){
        List<string[]> rowsToShow =  (limit > 0 ) ? _dataTable.Take(limit).ToList() : _dataTable;
         foreach (string[] row in rowsToShow){
            Console.WriteLine(string.Join(',', row));
        }
    }
}

public static class FileHandler{
    
    public static void WriteFile(  List<string[]> data, string filename, string filepath ="" ){

    }
    public static string ReadFile( string filename, string filepath= "" ){
        string fullPath = BuildFullPath (filename,filepath);
        if (!File.Exists(fullPath)){
            throw new FileNotFoundException($"Could not find the file at: {fullPath}");
        }
        using (StreamReader reader = new StreamReader(fullPath)){
            return reader.ReadToEnd(); 
        }
    }

    public static string BuildFullPath(  string filename, string filepath= ""){
        return string.IsNullOrEmpty(filepath) ? filename
                : filepath + (filepath[^1] == '/' ? "/" : "") + filename;
    }
}