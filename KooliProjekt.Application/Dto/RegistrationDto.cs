using System;
using System.Collections.Generic;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Dto
{
    public class RegistrationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public DateTime EventStartDateTime { get; set; }
        public decimal EventPrice { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
