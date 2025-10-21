using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Entities
{
    public abstract class BaseEntity
    {
        public DateTime CreatedOn { get; set; }= DateTime.Now;
        public String CreatedBy { get; set; } = null!;
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsDeleted { get; set; }



    }
}
