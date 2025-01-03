﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebsiteNoiThat.Models
{
    public class UserModelView
    {
        public int UserId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        public string Phone { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(32)]
        public string Password { get; set; }
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email không hợp lệ")]

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [StringLength(50)]
        public string GroupId { get; set; }

        public bool Status { get; set; }
        public int? NumberCard { get; set; }
        public int? Indentification { get; set; }
    }
}