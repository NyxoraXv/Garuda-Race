
using static CarStatistic;

public class PlayerInformation
{
    public string Username { get; set; }
    public string UserID { get; set; }
    public string DeviceModel { get; set; }
}

public class LevelInformation
{
    public string LevelName { get; set; }
    public string LevelID { get; set; }
    public enum LevelDifficuly
    {
        Easy,
        Medium,
        Hard
    }
}


public class Statistic
{
    public int Level;
    public int Exp;
    public int Highscore;
    public LevelInformation[] finishedLevelInformation;
}

public class LevelStatus
{
    public bool isComplated;
    public int Score;
}

public class CarStatistic
{
    public enum CarModel
    {

    }
    public enum CarRarity
    {
        Legendary,
        Rare,
        Common
    }

    public float Speed { get; set; }
    public float Acceleration { get; set; }
    public float Handling { get; set; }
    public float Braking { get; set; }
    public float Weight { get; set; }
}

public class CarList
{
    public CarModel[] carModel;
}