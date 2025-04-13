using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Careblock.Model.Web.Examination
{
    public class ExaminationPackageReviewCreationDto
    {
        public Guid Id { get; set; }
     
        public Guid ExaminationPackageId { get; set; }
        public Guid? ResultId { get; set; }

        public Guid? AppointmentId { get; set; }
        public Guid UserId { get; set; }
        public Guid ParentId { get; set; }
        public string Content { get; set; }

        public string? SignHash { get; set; }

        public int Rating { get; set; }
    }
}
