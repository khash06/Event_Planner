using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Belt.Models
{
    public class User
    {
        public int userId {get; set;}
        
        [Required(ErrorMessage="First Name Required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use only letters")]
        [MinLength(2, ErrorMessage = "Minimum two characters")]
        public string FirstName {get; set;}
        
        [Required(ErrorMessage="Last Name Required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use only letters")]
        [MinLength(2, ErrorMessage = "Minimum two characters")]
        public string LastName {get; set;}
        
        [Required(ErrorMessage="Email Address Required")]
        [EmailAddress]
        public string Email {get; set;}
        
        [Required(ErrorMessage="Password Required")]
        [MinLength(8, ErrorMessage="Minimum 8 characters")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$", ErrorMessage="Must have 1 number, 1 letter, and 1 special character")]
        public string Password {get; set;}
        
        [Required(ErrorMessage="Confirm Password Required")]
        [Compare("Password", ErrorMessage="Passwords do not match")]
        public string C_Password {get; set;}
    }

    public class User_Reg
    {
        [Key]
       public int UserId {get; set;} 
       public string FirstName {get; set;}
       public string LastName {get; set;}
        
       public string Email {get; set;}
       public string Password {get; set;}
       public List<Attendee> Attendee {get;set;}
       public User_Reg()
       {
           Attendee = new List<Attendee>();
       }
    }

    public class Activities
    {
        public int ActivitiesId {get;set;}

        [Required]
        [MyDate(ErrorMessage = "Date must be in the future")]
        public DateTime Date {get;set;}

        [Required]
        public TimeSpan StartTime {get;set;}

        [Required(ErrorMessage="Title is required")]
        [MinLength(2, ErrorMessage="Title must contain more than 2 characters")]
        public string Title {get;set;}
        public int Participants {get;set;}
        
        public int Creator {get;set;}
        
        [Required(ErrorMessage="Description required")]
        [MinLength(10, ErrorMessage="Description must be more than 10 characters")]
        public string Descripton {get;set;}


        public int Duration {get;set;}

        public string HrMin {get;set;}
        
        public List<Attendee> Reserver {get;set;}
        public Activities()
        {
            Reserver = new List<Attendee>();
        }
    }


    public class MyDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime d = Convert.ToDateTime(value);
            return d >= DateTime.Now;
        }
    }    

    public class Attendee
    {
        public int AttendeeId {get;set;}
        public int UserId {get;set;}
        public User_Reg User_Reg {get;set;}
        public int ActivitiesId {get;set;}
        public Activities Activities {get;set;}
        
    }
}
