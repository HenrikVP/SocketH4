namespace HelperLib
{
    public class Package
    {
        public int Id { get; set; }
        public string? User { get; set; }
        public string? Message { get; set; }
        public bool IsUpdate { get; set; } = false;

        public Package() { }

        public Package(int id, bool isUpdate)
        {
            Id = id;
            IsUpdate = isUpdate;
        }

        public Package(string user, string message)
        {
            User = user;
            Message = message;
        }
    }
}
