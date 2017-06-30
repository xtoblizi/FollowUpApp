namespace FollowUpWebApp.Models
{
    public enum Salutation
    {
        Dr = 1, Nurse, Mr, Mrs, Miss, Engr, Pastor
    }
    public enum Relationship
    {
        Father = 1, Mother, Sister, Brother, Friend, Others
    }
    public enum Gender
    {
        Male = 1, Female
    }
    public enum AddressType
    {
        Home = 1, Office, HomeTown
    }
    public enum Status
    {
        Single = 1, Married, Widowed, Divorced
    }

    public enum DeptType
    {
        Group = 1, Cell,
    }

    public enum MessageType
    {
        Absent = 1, Birthday, WeddingAnniversay, GroupLeader, FirstTimer, SecondTimer, ThirdTimer
    }
    public enum State
    {
        Abia, Adamawa, AkwaIbom, Anambra, Bauchi, Bayelsa, Benue, Borno, CrossRiver, Delta, Ebonyi, Edo, Ekiti,
        Abuja, Gombe, Imo, Jigawa, Kaduna, Kano, Katsina, Kebbi, Kogi, Kwara, Lagos, Nasarawa, Niger, Ogun, Ondo, Osun,
        Oyo, Plateau, Rivers, Sokoto, Taraba, Yobe, Zamfara
    }
}