namespace HelperLib
{
    public class Package
    {
        public int Id { get; set; }
        public string? User { get; set; }
        public string? Message { get; set; }
        public bool IsUpdate { get; set; } = false;
        public DateTime MsgDT { get; set; }

        public Package()
        {
            
        }

        public Package(int id, string user, string message)
        {
            Id = id;
            User = user;
            Message = message;
        }


    }
}
