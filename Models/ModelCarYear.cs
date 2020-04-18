using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarsLab
{
    public partial class ModelCarYear
    {
        public int Id { get; set; }
        public int IdCar { get; set; }
        public int IdYear { get; set; }
        [Display(Name = "Назва моделі авто")]
        public virtual ModelCar IdCarNavigation { get; set; }
        [Display(Name = "Рік випуску ")]
        public virtual YearOfIssue IdYearNavigation { get; set; }
    }
}
