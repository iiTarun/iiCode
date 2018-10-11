using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nom1Done.DTO
{   
        public class EmailQueueDto
        {
            public int Id { get; set; }
            public string ToUserID { get; set; }
            public string Subject { get; set; }
            public string Email { get; set; }
            public string Recipient { get; set; }
            public string CC { get; set; }
            public string Bcc { get; set; }
            public bool IsSent { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public System.DateTime SentDate { get; set; }
        }
    
}
