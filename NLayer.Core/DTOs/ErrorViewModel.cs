using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.DTOs
{
    public class ErrorViewModel //kullanıcı arayüzünde hataları göstermek için kullanılır
    {
        public List<string> Errors { get; set; } = new List<string>();
    }
}
