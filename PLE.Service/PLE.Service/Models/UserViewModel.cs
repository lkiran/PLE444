namespace PLE.Service.Models
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string ProfilePhoto { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool? IsFriend { get; set; }

        public string FullName()
        {
            return Name + " " + Surname;
        }
    }
}