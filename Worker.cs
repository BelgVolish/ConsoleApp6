namespace ConsoleApp6;

public struct Worker
{
    private const Char Separator = '#';
    
    public int Id { get; private init; }
    public DateTime AddTime { get; private init; }
    public string Name { get; private init; }
    public int Years => Convert.ToInt32(Math.Round((DateTime.Today - BirthDate).TotalDays / 365));
    public int Height { get; private init; }
    public DateTime BirthDate { get; private set; }
    public string BirthPlace { get; private init; }

    public string ConvertToString()
    {
        return $"{Id}{Separator}{Name}{Separator}{AddTime.ToLongDateString()}{Separator}{Height}{Separator}{BirthPlace}{Separator}{BirthDate}";
    }

    public static Worker ConvertFromString(string? str)
    {
        if (str == null)
            return new Worker();
        
        List<string> data = str.Split(Separator).ToList();
        Worker worker = new Worker
        {
            Id = Int32.Parse(data.ElementAt(0)),
            Name = data.ElementAt(1),
            AddTime = DateTime.Parse(data.ElementAt(2)),
            Height = Int32.Parse(data.ElementAt(3)),
            BirthPlace = data.ElementAt(4),
            BirthDate = DateTime.Parse(data.ElementAt(5))
        };

        worker.BirthDate = worker.BirthDate.Date;
        
        return worker;
    }

    public Worker()
    {
        AddTime = DateTime.Now;
        Id = -1;
        Name = string.Empty;
        Height = 0;
        BirthDate = DateTime.MinValue;
        BirthPlace = string.Empty;
        BirthDate = BirthDate.Date;
    }

    public Worker(int id, string name, int height, DateTime birthDate, string birthPlace)
    {
        Id = id;
        Name = name;
        Height = height;
        BirthDate = birthDate;
        BirthPlace = birthPlace;
        AddTime = DateTime.Now;
    }

}