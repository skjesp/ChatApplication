using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public class Message
    {
        public int Id { get; set; }

        //[Required, StringLength(20)]
        public string UserName { get; set; }

        public string Msg { get; set; }
    }
}