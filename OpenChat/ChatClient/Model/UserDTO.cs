using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenChatClient.Models
{
    public class UserDTO
    {
        public string Username { get; set; }
        public bool IsSelected { get; set; }

        public static explicit operator UserDTO(User r)
        {
            return new UserDTO() { Username = r.Username, IsSelected = false };
        }
    }
}