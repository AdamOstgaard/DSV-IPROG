namespace IPROG.Uppgifter.uppg3_1_2
{
    /// <summary>
    /// POCO object for guest
    /// </summary>
    public class Guest
    {
        public string Name { get;  }

        public string Email { get; }

        public string Homepage { get; }
        
        public string Comment { get; }

        /// <summary>
        /// Creates a new <see cref="Guest"/> instance
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="homePage"></param>
        /// <param name="comment"></param>
        public Guest(string name, string email, string homePage, string comment)
        {
            Name = name;
            Homepage = homePage;
            Email = email;
            Comment = comment;
        }

        /// <summary>
        /// Get a string representation of the object.
        /// </summary>
        /// <returns>A string representation of the guest.</returns>
        public override string ToString()
        {
            return $"name: {Name} phone: {Homepage} email: {Email} comment {Comment}";
        }
    }
}
