using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenChatClient.Models
{
    public class CreateRoomDTO
    {
        public string Name { get; set; }
        public List<int> Users { get; set; }
    }
}
