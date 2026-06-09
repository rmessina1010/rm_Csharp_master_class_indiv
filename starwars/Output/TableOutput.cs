using System.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace Output.TableOutput{
    public  class TablePrinter<T>{
        string TableHeader ;
        string Divider ;
        IEnumerable<T> Data;
        PropertyInfo[] ColumnProperties;

        string[] Whitelist = Array.Empty<string>();
        public int ColumnWidth = 21;
            
            
            [MemberNotNull(nameof(ColumnProperties), nameof(Divider), nameof(TableHeader))]
            public void GenerateHeader(){
            PropertyInfo[] allProperties = typeof(T).GetProperties();
            ColumnProperties = allProperties
                .Where(p => Whitelist.Length == 0 || Whitelist.Contains(p.Name))
                .ToArray();

            var allHeaders = ColumnProperties 
                .Select(p=>p.Name.Trim().PadRight(ColumnWidth));
            TableHeader = string.Join("|", allHeaders);
            Divider = new string('_', TableHeader.Length);
        }

        public void PrintHeader(){
            Console.WriteLine (TableHeader);
            Console.WriteLine (Divider);
        }

        public void PrintBody(){
            foreach (T row in Data){
                var allCols = ColumnProperties
                    .Select( c => (c.GetValue(row)?.ToString() ?? "").PadRight(ColumnWidth));
                Console.WriteLine(string.Join("|", allCols));
            }
        }

        public void PrintTable(){
            PrintHeader();
            PrintBody();
        }

        public TablePrinter (IEnumerable<T> data, string[]? whitelist = null){
            Data = data;
            Whitelist = whitelist ?? Array.Empty<string>();
            GenerateHeader();
        }

    }
}
