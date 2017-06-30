using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Web;

namespace FollowUpWebApp.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        public string ChurchDataId { get; set; }
        public Salutation Prefix { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        // [Required]
        [StringLength(50, ErrorMessage = "Middle name cannot be longer than 50 characters.")]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Gender")]
        public Gender Gender { get; set; }


        [Display(Name = "Date Of Birth")]
        //[Required(ErrorMessage = "Date of Birth is Required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Marital Status")]
        public Status MaritalStatus { get; set; }


        [Display(Name = "Marital Event Date")]
        //[Required(ErrorMessage = "Date of Birth is Required")]
        [DataType(DataType.Date)]
        public DateTime? MaritalDate { get; set; }

        //[Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [DataType(DataType.PhoneNumber)]
        //[Required(ErrorMessage = "Your Phone Number is required")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "House Number")]
        public string AddressHouseNo { get; set; }

        [Display(Name = "Street Name")]
        public string AddressStreetName { get; set; }

        [Display(Name = "City/Town")]
        public string AddressTown { get; set; }

        [Display(Name = "State")]
        public State AddressState { get; set; }


        [Display(Name = "Full Name")]
        public string UserName => LastName + " " + FirstName;

        [Display(Name = "Full Name")]
        public string FullName => LastName + " " + FirstName + " " + MiddleName;

        [Display(Name = "Town Of Birth")]
        public string TownOfBirth { get; set; }

        [Display(Name = "State of Origin")]
        public State StateOfOrigin { get; set; }

        [Display(Name = "Tribe/Language")]
        public string Tribe { get; set; }

        [Display(Name = "Nationality")]
        public string Nationality { get; set; }

        [Display(Name = "Country of Birth")]
        public string CountryOfBirth { get; set; }
        public bool IsANewMember { get; set; }


        public int Age
        {
            get
            {
                var t = DateTime.Now - DateOfBirth;
                return Age = (int)t.Days / 365;
            }
            set { }
        }
        public byte[] Passport { get; set; }

        [Display(Name = "Upload A Passport/Picture")]
        [ValidateFile(ErrorMessage = "Please select a PNG/JPEG image smaller than 20kb")]
        [NotMapped]
        public HttpPostedFileBase File
        {
            get
            {
                return null;
            }

            set
            {
                try
                {
                    MemoryStream target = new MemoryStream();

                    if (value.InputStream == null)
                        return;

                    value.InputStream.CopyTo(target);
                    Passport = target.ToArray();
                }
                catch (Exception)
                {
                    //logger.Error(ex.Message);
                    //logger.Error(ex.StackTrace);
                }
            }
        }

        //public ICollection<MemberAddress> MemberAddresses { get; set; }
        public virtual ChurchData ChurchData { get; set; }
        public virtual ICollection<GroupCellList> GroupCellLists { get; set; }

        public virtual ICollection<Attendance> Attendances { get; set; }
    }
}