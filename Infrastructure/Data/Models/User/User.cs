namespace Infrastructure.Data.Models.User
{
    public class User
    {
        public string Login { get; set; }
        public byte[] Hash { get; set; }
        public byte[] Salt { get; set; }
    }
}