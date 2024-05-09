namespace DDD_CQRS_Sample.Infrastructure.Email
{
    public class EmailSettingOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
