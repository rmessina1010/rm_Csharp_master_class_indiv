IDataDownloader dataDownloader = new SlowDataDownloader();

Console.WriteLine(dataDownloader.DownloadData("id1"));
Console.WriteLine(dataDownloader.DownloadData("id2"));
Console.WriteLine(dataDownloader.DownloadData("id3"));
Console.WriteLine(dataDownloader.DownloadData("id1"));
Console.WriteLine(dataDownloader.DownloadData("id3"));
Console.WriteLine(dataDownloader.DownloadData("id1"));
Console.WriteLine(dataDownloader.DownloadData("id2"));
Console.WriteLine(dataDownloader.DownloadData("id3"));
Console.WriteLine(dataDownloader.DownloadData("id3"));
Console.WriteLine(dataDownloader.DownloadData("id1"));
Console.WriteLine(dataDownloader.DownloadData("id2"));
Console.WriteLine(dataDownloader.DownloadData("id1"));
Console.WriteLine(dataDownloader.DownloadData("id2"));

Console.ReadKey();

public interface IDataDownloader
{
    string DownloadData(string resourceId);
}


public class SlowDataDownloader : IDataDownloader
{   
     
    private Cache<string, string> _cacheObj = new Cache<string, string>(3);

     public string DownloadData(string resourceId)
    {
        if (_cacheObj.Confirm(resourceId)){
            return _cacheObj.Retrieve(resourceId);
        }
        Thread.Sleep(2000);
        return _cacheObj.Add( resourceId, $"Some data for {resourceId}");
    }
}

public class Cache<TKey, TData> where TKey: notnull{

    private readonly int _maxSize;

    private Dictionary <TKey, (TData Payload, DateTime Age)> _cacheHold;

    public int MaxSize{
        get => _maxSize;
    }

    public Cache(int size=2) { 
           _maxSize = size;
           _cacheHold = new Dictionary <TKey, (TData Payload, DateTime Age)>();
    }

    public TData Add(TKey id, TData payload){
        CacheLimiter();       
        _cacheHold[id] = (payload, DateTime.UtcNow);
        return payload;
    }

    public bool Confirm(TKey id){
        return _cacheHold.ContainsKey(id);
    }

    public TData Retrieve(TKey id){
        return _cacheHold[id].Payload;
    }

    private void CacheLimiter(){
        while (_cacheHold.Count >= _maxSize) { RemoveOldest(); }
    }

    private void RemoveOldest(){
        if (_cacheHold.Count == 0){ return; }

        bool first = true;
        KeyValuePair <TKey, (TData Payload, DateTime Age)> oldest = default;
        foreach (var row in  _cacheHold){
            if (first || row.Value.Age < oldest.Value.Age){
                oldest = row;
                first = false;
            }
        }
        _cacheHold.Remove(oldest.Key);
    }

}