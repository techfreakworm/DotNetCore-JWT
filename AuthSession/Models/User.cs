using System;
using System.Collections.Generic;

namespace AuthSession.Models
{
    public partial class User
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
