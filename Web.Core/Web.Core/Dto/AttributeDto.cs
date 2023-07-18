using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Core.Dto
{
    public class AttributeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<ProductAttributeDto> ProductAttributes { get; set; }
    }
}
