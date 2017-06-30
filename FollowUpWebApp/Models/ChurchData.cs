using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FollowUpWebApp.Models
{
    public class ChurchData
    {
        public string ChurchDataId { get; set; }

        public string ParishName { get; set; }

        [Display(Name = "Street Number")]
        public string ChurchStreetNo { get; set; }

        [Display(Name = "Street Name")]
        public string ChurchStreetName { get; set; }

        [Display(Name = "City/Town")]
        public string ChurchTown { get; set; }

        [Display(Name = "State")]
        public State ChurchState { get; set; }

        //[Display(Name = "Postal Code")]
        //public string ChurchPostalCode { get; set; }

        public virtual ICollection<Member> Members { get; set; }


    }
}