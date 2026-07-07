namespace passwordgen{

public  class PasswordGenerator
{

    private readonly IRandom _random; 
    public PasswordGenerator(IRandom random){
        _random = random;
    }

    public  string Generate(
        int minLength, int maxLength, bool shallUseSpecialCharacters)
    {
        Validate(minLength,maxLength);
        var passwordLength =  setPasswordLengh(minLength, maxLength + 1);
        return GeneratePasswordString( passwordLength, shallUseSpecialCharacters); 
    }

    private static void Validate(int minLength, int maxLength){
        if (minLength < 1)
        {
            throw new ArgumentOutOfRangeException(
                $"{nameof(minLength)}   must be greater than 0");
        }
        if (maxLength < minLength)
        {
            throw new ArgumentOutOfRangeException(
                $"{nameof(minLength)}  must be smaller than {nameof(maxLength)} ");
        }
    }

    private int setPasswordLengh(int minLength, int maxLength){
        return _random.Next(minLength, maxLength);
    }

    private string GeneratePasswordString( 
        int passwordLength,
        bool shallUseSpecialCharacters)
    {
        var characterSet = shallUseSpecialCharacters ?
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_-+=" :
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable
            .Repeat(characterSet, passwordLength)
            .Select(charSet => charSet[_random.Next(charSet.Length)])
            .ToArray());
    }
}
}


public interface IRandom{
     int Next(int min, int max);
     int Next(int max);
}

public class RandomWrapper : IRandom{
    private readonly Random _random = new Random();
    public int Next  (int min, int max){
        return _random.Next(min,max);
    }
    public  int Next  (int max){
        return _random.Next(max);  
    }
}