
using passwordgen; 

for(int i = 0; i < 10; i++)
{
    Console.WriteLine(Pwd.Generate(5, 10, false));
}
Console.ReadKey();

