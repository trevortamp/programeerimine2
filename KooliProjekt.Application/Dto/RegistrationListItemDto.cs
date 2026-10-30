using System;
using System.Collections.Generic;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Dto
{
    public class RegistrationListItemDto
    {
        public int Id { get; set; }
        public string UserFullName { get; set; }
        public string EventName { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
