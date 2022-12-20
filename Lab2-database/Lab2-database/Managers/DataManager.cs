using Lab2_database.DataModels;

namespace Lab2_database.Managers;

public class DataManager
{
    public MusicLabb2Context MusicLabb2Context { get; }

    public DataManager(MusicLabb2Context musicLabb2Context)
    {
        MusicLabb2Context = musicLabb2Context;
    }
}