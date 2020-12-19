namespace NebzzClient.Messages
{
    public struct User
    {
        public string Username { get; private set; }

        public User(string username)
        {
            Username = username;
        }
    }
}
