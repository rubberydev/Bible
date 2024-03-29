﻿namespace Bible.Models
{
    using SQLite.Net.Attributes;

    public class User
    {
        [PrimaryKey]
        public int UserId { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
       
        public string Email { get; set; }
        
        public string Telephone { get; set; }
        
        public string ImagePath { get; set; }

        public int? UserTypeId { get; set; }
        
        public byte[] ImageArray { get; set; }
        
        public string Password { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImagePath))
                {
                    return "noimage";
                }

                if (this.UserTypeId == 1)
                {
                    return string.Format(
                        "http://knowingworldapi.azurewebsites.net/{0}",
                        ImagePath.Substring(1));
                }

                return ImagePath;
            }
        }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }

        public override int GetHashCode()
        {
            return UserId;
        }
    }
}
